using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;
using PlayFab.ClientModels;
using PlayFab;
using Photon.Pun.Demo.Cockpit;
using Unity.VisualScripting;
using Photon.Pun.Demo.PunBasics;

public class GameManager : MonoBehaviourPunCallbacks
{

    public List<string> cashItem;
    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }
    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    public GameObject playerPrefab; // ������ �÷��̾� ĳ���� ������
    public Vector3 spawnPosition;   // �÷��̾� ���� ������
    public GameObject[] targetPlayer; //�÷��̾� ����Ʈ
    public bool isGameover { get; private set; } // ���� ���� ����
    public bool inputLock;  // �Է��� ���� �� �ִ� ����. true�� ���� �ɷ� �Է� �Ұ�

    public Transform shopPosition; // �� ���̺� ������Ʈ�Ǵ� ������ Ʈ������
    public int playerDieCount;
    [Header("Game Info")]

    public string playerNickName;
    public string playerLevel;
    public string playerClass;
    public int playerSkin;





    // ��ȯ �߰�

    [Header("Game Setting")]

    // Junoh �߰�
    public NoticeController noticeController;
    public Volume volume;

    public int round = 1;               // ���� ����
    public int wave = 1;                // ���� ���̺�
    [SerializeField] public int MaxWave = 5;//������ ���̺�
    public int player = 4;              // �÷��̾� �ο� ��
    public int difficulty = 0;          // ���̵� 0: ���� 1: ����� 2: ����
    public int currentZombieCount = 0;  // ���� ���� ��
    public bool isZedTime = false;      // ���� Ÿ��
    public bool isSpawnZombie = false;  // ���� ��ȯ ����� Ȯ��
    public bool isCheck = false;        // ���� ���̺갡 ���� Ȯ��
    public bool isZedTimeCheck = false;
    public List<Transform> shops = new List<Transform>();
    public bool isShop = false;
    private bool isRespawn = false;
    [SerializeField] private Transform zombieParent;

    public bool isWave = false;
    private bool isRest = false;



    private bool GMMode = false;
    // Junoh �߰�
    
    [Header("Debug")]
    public bool palyerTestMode; // ��ȯ : �÷��̾� �׽�Ʈ�� 

    private void Awake()
    {
        cashItem = new List<string>();
        //cashItem �� ����

        //
        GetPlayerData();    // ���� �÷��̾� ������ ��������

        PhotonNetwork.AutomaticallySyncScene = true;


        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
        // ������ ���� ��ġ ����
        spawnPosition = new Vector3(0f, 1f, 0f);
        Quaternion newRotation = Quaternion.Euler(0, -90, 0);


        if (SceneManager.GetActiveScene().name == "Main")
        {
            Debug.Log("���ξ� ����");
            spawnPosition = new Vector3(135.0f, -6.0f, 200.0f);
        }

        // ��Ʈ��ũ ���� ��� Ŭ���̾�Ʈ�鿡�� ���� ����
        // ��, �ش� ���� ������Ʈ�� �ֵ�����, ���� �޼��带 ���� ������ Ŭ���̾�Ʈ���� ����
        //PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);

        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, newRotation);


        if (GMMode)
        {
            Debug.Log("GM Mode");
            Transform bossRoomPos = FindAnyObjectByType<StartColiderScripts>().transform;
            newPlayer.transform.position = bossRoomPos.transform.position;
        }
        //newPlayer.transform.SetParent(GameObject.Find("Players").transform);

    }

    // Ű���� �Է��� �����ϰ� ���� ������ ��
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && !palyerTestMode)    // ��ȯ : �÷��̾� �׽�Ʈ �����ϰ�� ��ŵ
        {
            ZombieCountCheck();
            ZombieCount(currentZombieCount);

            if (GameObject.Find("LoadManager").GetComponent<LoadSceneAsync>().isCheck)
            {
                if (isWave)
                {
                    isWave = false;
                    WaveStart();
                }
                else if (isRest)
                {
                    isRest = false;
                    WaveChange();
                }
            }
        }

        SetPlayer();
        shopPosition = shops[wave - 1];
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            LeaveServer();
        }
        if (isRespawn)
        {
            OnRespawn();    // ������ ������ ������ ���ֱ�
        }

        if (Input.GetKey(KeyCode.L) && isReset)
        {
            StartCoroutine(WaveReset());
        }
    }

    private bool isReset = false;

    private IEnumerator WaveReset()
    {
        isReset = true;

        WaveStart();

        yield return new WaitForSeconds(10);

        isReset = false;
    }

    #region WaveController
    public void WaveChange()
    {
        photonView.RPC("MasterChange", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void MasterChange()
    {
        photonView.RPC("SyncChange", RpcTarget.All);
    }

    [PunRPC]
    public void SyncChange()
    {
        StartCoroutine(ChangeWave());
    }

    public void WaveStart()
    {
        photonView.RPC("MasterStart", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void MasterStart()
    {
        photonView.RPC("SyncStart", RpcTarget.All);
    }

    [PunRPC]
    public void SyncStart()
    {
        StartCoroutine(StartWave());
    }
    #endregion

    #region ZombieCount
    public void ZombieCount(int _currentZombieCount)
    {
        photonView.RPC("MasterCount", RpcTarget.MasterClient, _currentZombieCount);
    }

    [PunRPC]
    public void MasterCount(int _currentZombieCount)
    {
        photonView.RPC("SyncCount", RpcTarget.All, _currentZombieCount);
    }

    [PunRPC]
    public void SyncCount(int _currentZombieCount)
    {
        currentZombieCount = _currentZombieCount;
    }

    public void SetPlayer()
    {
        //if(playerCount == PhotonNetwork.CurrentRoom.PlayerCount)
        targetPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject players in targetPlayer)
        {
            players.transform.SetParent(GameObject.Find("Players").transform);
        }
    }
    #endregion

    public void LeaveServer()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LoginScene");

        ////[Mijeong] 231001 : LocalPlayer Date �ٽ� �ҷ���
        //NetworkManager.net_instance.SetLocalPlayerData();
    }

    // ���� ������ �ڵ� ����Ǵ� �޼���
    public override void OnLeftRoom()
    {
        // ���� ������ �κ� ������ ���ư�
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("LoginScene");

        ////[Mijeong] 231001 : LocalPlayer Date �ٽ� �ҷ���
        //NetworkManager.net_instance.SetLocalPlayerData();
    }

    // ��ȯ �߰�
    public void GetPlayerData()
    {
        Debug.Log("�÷��̾� ������" + NetworkManager.net_instance.localPlayerName + "" + NetworkManager.net_instance.localPlayerLv);
        playerNickName = string.Format(NetworkManager.net_instance.localPlayerName);
        playerLevel = string.Format(NetworkManager.net_instance.localPlayerLv);
        playerClass = NetworkManager.net_instance.localPlayerClass;
        playerSkin = NetworkManager.net_instance.localCashItem;

    }

    // Junoh �߰�
    private void ZombieCountCheck()
    {
        int count = 0;

        for (int i = 0; i < zombieParent.childCount; i++)
        {
            for (int j = 0; j < zombieParent.GetChild(i).childCount; j++)
            {
                count += 1;
            }
        }

        currentZombieCount = count;
    }

    public void SetWave(int _num)
    {
        wave += _num;
    }

    private IEnumerator StartWave()
    {
        isCheck = true;

        PlayerUIManager.instance.CountUI.SetActive(true);
        PlayerUIManager.instance.TimerUI.SetActive(false);

        PlayerUIManager.instance.zombieCountText.gameObject.SetActive(true);
        PlayerUIManager.instance.timerCountText.gameObject.SetActive(false);

        while (true)
        {
            if (currentZombieCount > 0) { break; }

            yield return null;
        }

        PlayerUIManager.instance.SetStartNotice("Start Wave");
        StartCoroutine(noticeController.CoroutineManager(false));

        while (0 < currentZombieCount)
        {
            PlayerUIManager.instance.SetZombieCount(currentZombieCount);

            yield return null;
        }

        PlayerUIManager.instance.SetZombieCount(currentZombieCount);

        if (PhotonNetwork.IsMasterClient)
        {
            isRest = true;
        }
    }

    private IEnumerator ChangeWave()
    {
        if (MaxWave == wave)
        {
            PlayerUIManager.instance.SetEndNotice("Clear");
            PlayerUIManager.instance.SetNoticeLogo("Congratulate");
        }
        else
        {
            PlayerUIManager.instance.SetEndNotice("Wave Clear");
            PlayerUIManager.instance.SetNoticeLogo("Go to Shop");
        }

        isShop = true;
        isRespawn = true;

        PlayerUIManager.instance.CountUI.SetActive(false);
        PlayerUIManager.instance.TimerUI.SetActive(true);

        PlayerUIManager.instance.zombieCountText.gameObject.SetActive(false);
        PlayerUIManager.instance.timerCountText.gameObject.SetActive(true);

        StartCoroutine(noticeController.CoroutineManager(true));

        int timeElapsed = 70;


        while (0 < timeElapsed)
        {
            timeElapsed -= 1;

            PlayerUIManager.instance.SetTimerCount(timeElapsed);


            yield return new WaitForSeconds(1);
        }

        isShop = false;
        SetWave(1);

        if (PhotonNetwork.IsMasterClient)
        {
            isWave = true;
        }
    }
    // Junoh �߰�

    public void OnRespawn()
    {
        isRespawn = false;

        if (!isGameover)
        {
            // ������� ī��Ʈ �ʱ�ȭ
            playerDieCount = 0;

            for (int i = 0; i <= targetPlayer.Length; i++)
            {
                PlayerHealth player = targetPlayer[i].GetComponent<PlayerHealth>();
                // �÷��̾ �׾��� ��� ������ �����ֱ�
                if (player.dead)
                {
                    player.Respawn();
                }
            }
        }

    }

    public void OnGameOver()
    {
        isGameover = true;
        PlayerUIManager.instance.gameOverUI.SetActive(true);
        PlayerUIManager.instance.leaveButton.SetActive(true);
        StartCoroutine(noticeController.GameOver(true));
        Debug.Log("���ӿ���");
    }
}
