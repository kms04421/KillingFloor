using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using static UnityEngine.Rendering.DebugUI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerUIManager>();
            }

            return m_instance;
        }
    }
    private static PlayerUIManager m_instance; // �̱����� �Ҵ�� ����
    [Header("Player")]
    public TMP_Text playerLevel;
    public TMP_Text hpText;         // ü�� ǥ��
    public TMP_Text shiedldText;    // �ǵ� ǥ��
    public TMP_Text ammoText;       // ź�� ǥ��
    public TMP_Text totalAmmoText;  // ���� ź��
    public TMP_Text grenadeText;    // ���� ����ź
    public TMP_Text coinText;       // ���� ��ȭ
    public TMP_Text weightText;     // ���� ����       
    public GameObject[] classIcon;  // Ŭ���� ������
    public Slider expSlider;        // ����ġ

    // ���� ����ȿ�� ���� ����
    private int coin;
    private int targetCoin;

    [Header("Shop")]
    public TMP_Text shopDistance;   // �������� �Ÿ�
    public Slider shopUpRotation;     // ���� ����
    public Slider shopDownRotation;     // ���� ����
    public Slider healSlider;       // �� �����̴�
    public GameObject equipUI;      // ��ȣ�ۿ� UI
    public GameObject shopUI;       // ���� ��ȣ�ۿ� UI
    public GameObject pauseUI;       // ���� UI

    [Header("BloodScreen")]
    public float playerHealth;  // ���彺ũ���� ������ �ֱ����� �÷��̾��� ü��
    public Image bloodScreen;   // �� ������ ��ũ��
    public Image poisonScreen;  // �� ������ ��ũ��
    public float bloodScreenValue;
    public float poisonScreenValue;
    public bool isPoison; // ���� ������ �������� Ȯ��

    [Header("Pause")]
    public Slider mouseSensitive;
    public TMP_Text mouseSensitiveValue;
    public bool isShopState;
    public bool isPauseState;

    public GameObject gameOverUI;
    public GameObject leaveButton;

    [Header("Wave")]
    //JunOh
    public Text warningEndText;   // ���̺� ���� ����
    public Text warningStartText; // ���̺� ���� ����
    public Text noticeTextText;   // �˸� �ΰ� ����
    public TMP_Text noticeCountText;  // �˸� ���̺� ����
    public TMP_Text zombieCountText;  // ���� ��
    public TMP_Text timerCountText;   // Ÿ�̸�
    public TMP_Text zombieWaveText;   // ���� ���̺� ����
    public GameObject CountUI;
    public GameObject TimerUI;

    public void Update()
    {
        SetNoticeWave();
        SetZombieWave();
        Pause();
        DamageScreenUpdate();
    }

    // ü�� �ؽ�Ʈ ����
    public void SetLevel(string value)
    {
        playerLevel.text = value;
    }
    public void SetHP(float value)
    {
        hpText.text = string.Format("{0}", value);
    }
    // �ǵ� �ؽ�Ʈ ����
    public void SetArmor(float value)
    {
        shiedldText.text = string.Format("{0}", value);
    }
    public void SetAmmo(float value)
    {
        if (value == 999)
        { ammoText.text = string.Format("��"); }
        else
            ammoText.text = string.Format("{0}", value);
    }
    public void SetRemainingAmmo(float value)
    {
        if (value == 999)
        { totalAmmoText.text = string.Format("��"); }
        else
            totalAmmoText.text = string.Format("{0}", value);
    }
    public void SetGrenade(float value)
    {
        grenadeText.text = string.Format("{0}", value);
    }
    public void SetHeal(float value)
    {
        healSlider.value = value;
    }
    // ���� ȹ��
    public void SetCoin(int value)
    {
        targetCoin = value;
    }
    public void SetClass(string playerClass)
    {
        switch (playerClass)
        {
            case "Commando":
                classIcon[0].gameObject.SetActive(true); 
                classIcon[1].gameObject.SetActive(false); 
                break;
            case "Demolitionist":
                classIcon[0].gameObject.SetActive(false);
                classIcon[1].gameObject.SetActive(true);
                break;
        }
    }
    public void SetExp(int value)
    {
        expSlider.value = value;
    }

    // ���� ���� ������Ʈ
    public void CoinUpdate()
    {

        if (coin < targetCoin)
        {
            coin += Mathf.CeilToInt(1f * Time.deltaTime); // �ʴ� ���� ������Ʈ
            if (coin >= targetCoin)
            {
                coin = targetCoin; // ���� ���ο� �����ϸ� ����
            }
            coinText.text = string.Format("{0}", coin);
        }

        else
        {
            coin -= Mathf.CeilToInt(1f * Time.deltaTime); // �ʴ� ���� ������Ʈ
            if (coin <= targetCoin)
            {
                coin = targetCoin; // ���� ���ο� �����ϸ� ����
            }
            coinText.text = string.Format("{0}", coin);
        }


    }
    public void SetWeight(float value)
    {
        weightText.text = string.Format("{0}", value);
    }
    public void SetShopDistance(float value)
    {
        shopDistance.text = string.Format("{0}M", value);
    }
    public void SetShopRotation(float value, bool isUp)
    {  
        shopUpRotation.value = value;
        shopDownRotation.value = value;

        // �� �Ʒ� ���� 
        shopUpRotation.gameObject.SetActive(isUp);
        shopDownRotation.gameObject.SetActive(!isUp);

    }

    // ��ũ�� ������ ������Ʈ
    public void DamageScreenUpdate()
    {
        // ���� ��ũ���� ���� ���� ��� 0�� �ɶ����� ����
        if (0 < bloodScreenValue)
        {
            bloodScreenValue -= Mathf.CeilToInt(1 * Time.deltaTime);
            bloodScreen.color = new Color(255, 255, 255, bloodScreenValue/1000);
        }
        // ���� ��ũ���� ���� ���� ��� 0�� �ɶ����� ����
        if (0 < poisonScreenValue)
        {
            poisonScreenValue -= Mathf.CeilToInt(1 * Time.deltaTime);
            poisonScreen.color = new Color(255, 255, 255, bloodScreenValue / 100);
        }
    }
    // ���� ��ũ���� �� ����
    public void SetBloodScreen(float _health)
    {
        // ü���� ���� ��� �� �Ӿ����� �ϱ� ���� ��
        // ü���� 100�̸� ���Ծ���
        float newHealth = (-1*_health+100);

        bloodScreenValue += 200 + newHealth;
        if(1000f < bloodScreenValue)
        { bloodScreenValue = 1000f;}
    }
    // ������ ��ũ���� �� ����
    public void SetPoisonScreen()
    {
        poisonScreenValue += 200;
    }

    // ���� ������Ʈ
    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isShopState && !isPauseState)
        {
            isPauseState = true;
            pauseUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GameManager.instance.inputLock = true;  // ��ǲ���� �ɾ��ֱ�
        }
        // ���� �ݱ� ESC
        else if (Input.GetKeyDown(KeyCode.Escape) && isPauseState)
        {
            OffPause();
        }
        MouseSensitiveUpdate();

    }
    // ���� �ѱ�
    public void OnPause()
    {
        isPauseState = true;
    }
    // ���� ����
    public void OffPause()
    {
        isPauseState = false;
        pauseUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.instance.inputLock = false;  // ��ǲ���� Ǯ���ֱ�

    }
    // ���� ������ ��ư
    public void LeaveRoomButton()
    {
        GameManager.instance.LeaveServer();
    }
    // ���콺 ����Ƽ�� ������Ʈ
    public void MouseSensitiveUpdate()
    {
        mouseSensitiveValue.text = string.Format("{0}", Mathf.FloorToInt(mouseSensitive.value));
    }



    //JunOh

    public void SetEndNotice(string value)
    {
        warningEndText.text = string.Format("{0}", value);
    }

    public void SetStartNotice(string value)
    {
        warningStartText.text = string.Format("{0}", value);
    }

    public void SetNoticeWave()
    {
        noticeCountText.text = string.Format("[ {0}/ {1} ]", GameManager.instance.round, GameManager.instance.wave);
    }

    public void SetNoticeLogo(string noticeTextValue)
    {
        noticeTextText.text = string.Format("{0}", noticeTextValue);
    }

    public void SetZombieCount(float countValue)
    {
        zombieCountText.text = string.Format("{0}", countValue);
    }

    public void SetTimerCount(int value)
    {
        timerCountText.text = string.Format("{0}:{1:D2}", value / 60, value % 60);
    }

    public void SetZombieWave()
    {
        zombieWaveText.text = string.Format("{0}/ {1}", GameManager.instance.round, GameManager.instance.wave);
    }
    //JunOh
}