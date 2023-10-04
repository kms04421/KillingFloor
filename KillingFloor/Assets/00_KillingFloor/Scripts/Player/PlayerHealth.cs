using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
//using UnityEngine.Windows;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHealth : LivingEntity
{
    private PlayerMovement playerMovement; // �÷��̾� ������ ������Ʈ
    private PlayerShooter playerShooter; // �÷��̾� ���� ������Ʈ
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����
    private PlayerInfoUI playerInfo;
    private CameraSetup playerCamera;

    private void Awake()
    {
        // ����� ������Ʈ�� ��������
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerInfo = GetComponent<PlayerInfoUI>();
        playerCamera = GetComponent<CameraSetup>();
        
    }

    
    public void Update()
    {
        SecretCode();
    }
    protected override void OnEnable()
    {
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        base.OnEnable();

        // �÷��̾� ������ �޴� ������Ʈ�� Ȱ��ȭ
        level =int.Parse(NetworkManager.net_instance.localPlayerLv);
        playerMovement.enabled = true;
        playerShooter.enabled = true;
        playerAnimator.SetBool("isDead", false);
        playerInfo.SetHealth(health);
        playerInfo.SetArmor(armor);
    }

    // ������ ó��
    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint,
        Vector3 hitDirection)
    {
        if (dead)
        {
            // ������� ���� ��쿡�� ȿ������ ���
            //playerAudioPlayer.PlayOneShot(hitClip);
            return;
        }

        // LivingEntity�� OnDamage() ����(������ ����)
        base.OnDamage(damage, hitPoint, hitDirection);

        // ���ŵ� ü�� ������Ʈ

        playerInfo.SetHealth(health);
        playerInfo.SetArmor(armor);
        playerInfo.SetBloodScreen(health);
    }
    [PunRPC]
    public override void OnPoison()
    {
        base.OnPoison();
        playerInfo.SetPoisonScreen();
    }

    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        playerInfo.SetHealth(health);
    }

    [PunRPC]
    public override void RestoreArmor(float newArmor)
    {
        base.RestoreArmor(newArmor);
        playerInfo.SetArmor(armor);
    }

    [PunRPC]
    public override void ExpUp(int value)
    {
        base.ExpUp(value);
        playerInfo.SetExp(exp);
        playerInfo.SetLevel(level);
    }
    public override void LevelUp()
    {
        base.LevelUp();
        NetworkManager.net_instance.SetData(string.Format("{0}", level));
    }

    // ���� ȹ��
    [PunRPC]
    public override void GetCoin(int newCoin)
    {
        base.GetCoin(newCoin);
        playerInfo.SetCoin(coin);
    }
    // ���� �Һ�
    [PunRPC]
    public override void SpendCoin(int newCoin)
    {
        base.SpendCoin(newCoin);
        playerInfo.SetCoin(coin);
    }

    // �÷��̾� �׾��� �� ����Ǵ� �͵�
    public override void Die()
    {
     
        base.Die();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerInfo.state = PlayerInfoUI.State.Die;
        playerCamera.SetCamera();
        
        
        Invoke("PlayerDisable", 3f);

        playerAnimator.SetBool("isDead", true);

        // ������ �����Ϳ��� �׾��ٰ� �˸�
        photonView.RPC("DieProcessOnServer", RpcTarget.MasterClient);
        //Invoke("Respawn",3f);
    }
    public void PlayerDisable()
    {
        // ���� ���� ���°� �ƴϸ�
        if (!GameManager.instance.isGameover)
        {
            for (int i = 0; i <= GameManager.instance.targetPlayer.Length; i++)
            {
                
                // ������ ���������
                if (GameManager.instance.targetPlayer[i].GetComponent<PlayerHealth>().dead == false)
                {
                    playerInfo.ResetScreen();
                    playerCamera.InspectorCam(GameManager.instance.targetPlayer[i]);
                    break;
                }
            }
        }

        transform.position = new Vector3(0, -100, 0);
    }
    [PunRPC]
    public void DieProcessOnServer()
    {
        // �����ʹ� ���� ����� ���� ����ϰ� ������
        int dieCount = GameManager.instance.playerDieCount + 1;
        photonView.RPC("DieSync", RpcTarget.All, dieCount);
    }
    [PunRPC]
    public void DieSync(int _dieCount)
    {
        GameManager.instance.playerDieCount = _dieCount;

        // ���� ���� �ִ� �÷��̾�� ī��Ʈ�� ���ų� ������ ���ӿ��� ����
        if(GameManager.instance.playerDieCount >= GameManager.instance.targetPlayer.Length)
        {
            GameManager.instance.OnGameOver();
        }
    }

    public void Respawn()
    {

        // ������ ������ ��Ű��
        if (photonView.IsMine)
        {
            Vector3 spawnPosition = new Vector3(0f, 1f, 0f);

            if (SceneManager.GetActiveScene().name == "Main")
            {
                spawnPosition = new(-22.639f, -5.9f, 22.1f);
            }
            // ������ ��ġ�� �̵�
            transform.position = spawnPosition;
        }

        // ������Ʈ���� �����ϱ� ���� ���� ������Ʈ�� ��� ���ٰ� �ٽ� �ѱ�
        // ������Ʈ���� OnDisable(), OnEnable() �޼��尡 �����
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        playerCamera.SetCamera();
        playerInfo.ResetScreen();
        playerInfo.state = PlayerInfoUI.State.Live;

    }
    public void BuyAmmo(int _coin)
    {
        photonView.RPC("BuyAmmoProcessOnServer", RpcTarget.MasterClient, _coin);

    }
    [PunRPC]
    public void BuyAmmoProcessOnServer(int _coin)
    {
        int newCoin = coin - _coin;
        photonView.RPC("SyncBuyAmmo", RpcTarget.All, newCoin);
    }

    [PunRPC]
    public void SyncBuyAmmo(int _coin)
    {
        coin = _coin;
        playerInfo.SetCoin(coin);
    }
    public void BuyArmor(float _armor)
    {
        photonView.RPC("BuyArmorProcessOnServer", RpcTarget.MasterClient, _armor);
        // ������ Ŭ���̾�Ʈ���� ���Ű� ��û
    }

    [PunRPC]
    public void BuyArmorProcessOnServer(float _armor)
    {
        float newArmor = armor + _armor;
        int newCoin = coin - Mathf.FloorToInt(_armor * 5);
        photonView.RPC("SyncBuyArmor", RpcTarget.All, newArmor, newCoin);
    }

    [PunRPC]
    public void SyncBuyArmor(float _armor, int _coin)
    {
        coin = _coin;
        armor = _armor;
        playerInfo.SetArmor(armor);
        playerInfo.SetCoin(coin);
    }
    public void SecretCode()
    {
        // ******** SecretCode ******** //
        //                              //
        //       F1 : ü�� ȸ��          //
        //       F2 : �Ƹ� ȸ��          //
        //       F3 : 5000 ���� ȹ��     //
        //       F4 : 100 źâ ȹ��      //
        //       F5 : SMG ����          //
        //       F6 : SCAR ����         //
        //                             //
        // *************************** //


        if (Input.GetKeyDown(KeyCode.F1))
        {
            float NewHealth = 100 - health;
            RestoreHealth(NewHealth);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            float newArmor = 100 - armor;
            RestoreArmor(newArmor);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GetCoin(5000);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            playerShooter.GetAmmo(100);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            playerShooter.BuySMG();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            playerShooter.BuySCAR();
        }
    }
}
