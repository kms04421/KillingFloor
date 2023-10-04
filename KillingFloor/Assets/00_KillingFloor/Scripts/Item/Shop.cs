using JetBrains.Annotations;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class Shop : MonoBehaviour
{
    PlayerInputs input;
    PlayerHealth playerInfo;
    PlayerShooter shooter;

    [Header("Shop State")]
    public GameObject shopUI;
    bool isShopOpen;

    [Header("Shop Info")]
    public TMP_Text playerCoin;
    public TMP_Text wave;
    public TMP_Text waveTime;

    [SerializeField] private int MaxAmmo;
    [SerializeField] private int remaining;
    [SerializeField] private int magazineAmmo;
    [SerializeField] private int waveShop;
    [SerializeField] private int subWaveShop;

    [Header("Armor")]
    public Text armorValueText;     // �Ƹ� ��
    public Text armorPriceText;     // �Ƹ� ����
    public Button buyArmorBtn;
    public GameObject buyArmorDisableBtn;

    [Header("Grenade")]
    public Text grenadeValueText;   // ����ź ��
    public Text grenadePriceText;   // ����ź ����
    public Button buyGrenadeBtn;
    public GameObject buyGrenadeDisableBBtn;

    [Header("Pistol")]
    public Text pistolValueText;    // ���� ��
    public Text pistolPriceText;    // ���� ����
    public Text pistolNameText;
    public Button buyPistolBtn;
    public GameObject buyPistolDisableBtn;
    public GameObject[] pistolIcon;

    [Header("Rifle")]
    public Text rifleValueText;     // �ֹ��� ��
    public Text riflePriceText;     // �ֹ��� ����
    public Text rifleNameText;
    public Button buyRifleBtn;
    public GameObject buyRifleDisableBtn;
    public GameObject[] rifleIcon;

    [Header("SubWeapon")]
    public Text subWeaponValueText;     // �ֹ��� ��
    public Text subWeaponPriceText;     // �ֹ��� ����
    public Text subWeaponNameText;
    public Button buysubWeaponBtn;
    public GameObject buysubWeaponDisableBtn;
    public GameObject[] subWeaponIcon;


    [Header("BuyWeapon")]
    public Button buyScarBtn;
    public GameObject buyScarDisableBtn;
    public Text SmgBtnText;
    public Button buySmgBtn;
    public GameObject buySmgDisableBtn;
    public Text ScarBtnText;



    // Update is called once per frame
    void Update()
    {

        // �÷��̾��� ������ �ְ�, ���� IsMine�� ��� ������Ʈ
        if (playerInfo != null &&playerInfo.photonView.IsMine)
        {
            ShopUpdate();
            
            ShopOpen();
        }
    }

    public void ShopUpdate()
    {
        waveTime.text = PlayerUIManager.instance.timerCountText.text;   // ���̺� �ð� ������Ʈ
        wave.text = PlayerUIManager.instance.zombieWaveText.text;       // ���̺� ������ ������Ʈ
        playerCoin.text = PlayerUIManager.instance.coinText.text;       // �÷��̾� �ݾ� ������Ʈ


        //  ================================================ �Ƹ� ������Ʈ ================================================
        armorValueText.text = string.Format("{0}/100", playerInfo.armor);   // ���� �Ƹӷ� ������Ʈ

        // _armor�� ������ �Ƹ��� ��. 100�� �ƽ��̹Ƿ� 100- ���� �Ƹ�
        int _armor = Mathf.FloorToInt(100 - playerInfo.armor);
        //���� �ƸӸ� ������ ���� ���ٸ� 
        if (playerInfo.coin < _armor * 5)
        {
            _armor = Mathf.FloorToInt(playerInfo.coin / 5);
        }
        armorPriceText.text = string.Format("{0}", _armor * 5); // ���� ���� ������Ʈ

        // ���� �ƸӰ� ��á����� MAX ǥ��
        if (100 <= playerInfo.armor)
        { armorPriceText.text = string.Format("MAX"); }

        // ��ư ��Ȱ��ȭ
        if (100 <= playerInfo.armor || playerInfo.coin < _armor || playerInfo.coin == 0)
        {
            buyArmorBtn.interactable = false;
            buyArmorDisableBtn.SetActive(true);
        }
        else
        {
            buyArmorBtn.interactable = true;
            buyArmorDisableBtn.SetActive(false);
        }

        //  ================================================ ���� ����ź ������Ʈ ================================================
        grenadeValueText.text = string.Format("{0}/5",shooter.grenade);
        grenadePriceText.text = string.Format("300");   // ���Ű��� 300����

         // ����ź�� ��á�� ��� MAX
        if (5 <= shooter.grenade)
        { grenadePriceText.text = string.Format("MAX"); }

        // ��ư ��Ȱ��ȭ
        if (5 <= shooter.grenade || playerInfo.coin < 300)
        {
            buyGrenadeBtn.interactable = false;
            buyGrenadeDisableBBtn.SetActive(true);
        }
        else
        {
            buyGrenadeBtn.interactable = true;
            buyGrenadeDisableBBtn.SetActive(false);
        }

        //  ================================================ �ǽ��� ������Ʈ ================================================
        pistolValueText.text = string.Format(shooter.tpsPistol.remainingAmmo + "/" + shooter.tpsPistol.maxAmmo);    // ���� ź ���� ������Ʈ
        pistolPriceText.text = string.Format("250");   // ���Ű��� 250����

        // �ǽ����� ��á�� ��� MAX
        if (shooter.tpsPistol.remainingAmmo >= shooter.tpsPistol.maxAmmo)
        { grenadePriceText.text = string.Format("MAX"); }

        // ��ư ��Ȱ��ȭ
        if (shooter.tpsPistol.remainingAmmo >= shooter.tpsPistol.maxAmmo || playerInfo.coin < 250)  // ź�� ���ų� ���� ������ ��ư ��Ȱ��ȭ
        {
            buyPistolBtn.interactable = false;
            buyPistolDisableBtn.SetActive(true);
        }
        else
        {
            buyPistolBtn.interactable = true;
            buyPistolDisableBtn.SetActive(false);

        }
        pistolIcon[0].SetActive(true);
        pistolIcon[1].SetActive(false);

        // �ǽ��� ������ ������Ʈ
        if (shooter.isSMG)
        {
            pistolNameText.text = string.Format("SMG");
            pistolIcon[0].SetActive(false);
            pistolIcon[1].SetActive(true);
        }

        //  ================================================ ������ ������Ʈ ================================================
        rifleValueText.text = string.Format(shooter.tpsRifle.remainingAmmo + "/" + shooter.tpsRifle.maxAmmo);  // ���� ź ���� ������Ʈ
        riflePriceText.text = string.Format("250");   // ���Ű��� 250����

        // �������� ��á�� ��� MAX
        if (shooter.tpsRifle.remainingAmmo >= shooter.tpsRifle.maxAmmo)
        { riflePriceText.text = string.Format("MAX"); }

        // ��ư ��Ȱ��ȭ
        if (shooter.tpsRifle.remainingAmmo >= shooter.tpsRifle.maxAmmo || playerInfo.coin < 250 )  // ź�� ���ų� ���� ������ ��ư ��Ȱ��ȭ
        {
            buyRifleBtn.interactable = false;
            buyRifleDisableBtn.SetActive(true);
        }
        else
        {
            buyRifleBtn.interactable = true;
            buyRifleDisableBtn.SetActive(false);

        }

        // ������ ������ ������Ʈ
        rifleIcon[0].SetActive(false);
        rifleIcon[1].SetActive(false);
        rifleIcon[2].SetActive(false);

        if (shooter.weaponClass == PlayerShooter.WeaponClass.Commando && !shooter.isSCAR)
        {
            rifleNameText.text = string.Format("AR-15 Varmint Rifle");
            rifleIcon[0].SetActive(true);
        }
        else if (shooter.weaponClass == PlayerShooter.WeaponClass.Demolitionist && !shooter.isSCAR)
        {
            rifleNameText.text = string.Format("Grenade Gun");
            rifleIcon[1].SetActive(true);
        }
        if (shooter.isSCAR)
        {
            rifleNameText.text = string.Format("SCAR");
            rifleIcon[2].SetActive(true);
        }


        // SMG ���� ������Ʈ
        if (playerInfo.coin < 2000 || shooter.isSMG)
        {
            buySmgBtn.interactable = false;
            buySmgDisableBtn.SetActive(true);
            if(shooter.isSMG)
            {
                SmgBtnText.text = string.Format("SOLD");
            }

        }
        else
        {
            buySmgBtn.interactable = true;
            buySmgDisableBtn.SetActive(false);
        }


        // ��ī ���� ������Ʈ
        if (playerInfo.coin < 5000 || shooter.isSCAR)
        {
            buyScarBtn.interactable = false;
            buyScarDisableBtn.SetActive(true);
            if (shooter.isSCAR)
            {
                ScarBtnText.text = string.Format("SOLD");
            }
        }
        else
        {
            buyScarBtn.interactable = true;
            buyScarDisableBtn.SetActive(false);
        }

       

    }

    // ���� ����
    public void ShopOpen()
    {
        if (isShopOpen)
        {
            // ������ �������� �� �Է� �����ֱ�
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GameManager.instance.inputLock = true;
            //input.cursorInputForLook = false;
            //input.cursorLocked = false;

            // ���� �ݱ� ESC
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShopClose();
                input.cancle = false;
            }
            else if(!GameManager.instance.isShop)
            {
                ShopClose();
            }
        }
       
    }
    public void ShopCloseButton()
    {
        ShopClose();
        input.cancle = true;
    }
    // ���� �ݱ�
    public void ShopClose()
    {
        Debug.Log("���� ���� ��ư�� ������?");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        shopUI.SetActive(false);
        PlayerUIManager.instance.shopUI.SetActive(true);
        GameManager.instance.inputLock = false;
        //input.cursorInputForLook = true;
        //input.cursorLocked = true;
        isShopOpen = false;
        Invoke("ChangeState", 0.5f);
    }
    private void ChangeState()
    {
        PlayerUIManager.instance.isShopState = false;
    }
    void OnTriggerStay(Collider player)
    {
        //MaxAmmo =(int)player.GetComponent<PlayerShooter>().equipedWeapon.maxAmmo;
        //remaining = (int)player.GetComponent<PlayerShooter>().equipedWeapon.remainingAmmo;
        //magazineAmmo = (int)player.GetComponent<PlayerShooter>().equipedWeapon.magazineSize;
                           

        //if(GameManager.instance.wave == waveShop || GameManager.instance.wave == subWaveShop)
        //{
            if (player.CompareTag("Player") && GameManager.instance.isShop)
            {
                input = player.GetComponent<PlayerInputs>();
                playerInfo = player.GetComponent<PlayerHealth>();
                shooter = player.GetComponent<PlayerShooter>();
                if (playerInfo != null && playerInfo.photonView.IsMine)
                {
                    PlayerUIManager.instance.shopUI.SetActive(true); // �ȳ� UI �ѱ�

                    if (input.equip && !isShopOpen) // ��ư�� ������
                    {
                        shopUI.SetActive(true);
                        PlayerUIManager.instance.shopUI.SetActive(false);
                        PlayerUIManager.instance.isShopState = true;

                        isShopOpen = true;
                        input.equip = false;
                    }
                }

            }
        //}
        // �÷��̾ ��ó�� ������
      
    }
    
    // �÷��̾ ��ó���� �־�����
    private void OnTriggerExit()
    {
        if (playerInfo != null &&playerInfo.photonView.IsMine && GameManager.instance.isShop)
        {
            PlayerUIManager.instance.shopUI.SetActive(false);
            isShopOpen = false;
            input = null;
            playerInfo = null;
            shooter = null;
        }
    }


    public void BuyArmor()
    {
        if(playerInfo != null)
        {
            // �ƸӴ� 100�� �ѵ��� ä�� �� ���� ������ �Ƹ��� ��
            int _armor = Mathf.FloorToInt(100 - playerInfo.armor);
            // ���� ������ ���� ������ �縸ŭ ���� ����
            if(playerInfo.coin < _armor * 5)
            {
                _armor = Mathf.FloorToInt(playerInfo.coin / 5);
            }

            Debug.Log("�÷��̾� �� : "+playerInfo.coin+"�÷��̾� �Ƹ� : "+playerInfo.armor);
            // �Ƹ� ������ 1�� 5��, 100���� ä����� 500�� �ʿ�
            // ���� �ְ� �ƸӰ� 100�� �ƴ� ��� ���� ����
            if (playerInfo.coin >= _armor * 5 && playerInfo.armor != 100)
            {
                Debug.Log("�����ϳ�?");
                //playerInfo.RestoreArmor(_armor);
                //playerInfo.SpendCoin(_armor);
                playerInfo.BuyArmor(_armor);
            }
        }
    }
    public void BuyGrenade()
    {
        if (playerInfo != null)
        {
            // ����ź�� 5������ ���ų� 300���κ��� �� ���� ��� ���� ����
            if(shooter.grenade < 5 && 300 <= playerInfo.coin)
            {

                shooter.grenade++;

                PlayerUIManager.instance.SetGrenade(shooter.grenade); // ����ź ���� �߰�
                playerInfo.BuyAmmo(300);

            }

        }

    }
    public void BuyPistolAmmo()
    {
        if (playerInfo != null)
        {

            if (250 <= playerInfo.coin && shooter.tpsPistol.remainingAmmo < shooter.tpsPistol.maxAmmo)
            {
                if (shooter.tpsPistol.magazineSize + shooter.tpsPistol.remainingAmmo > shooter.tpsPistol.maxAmmo)
                {
                    shooter.tpsPistol.remainingAmmo = shooter.tpsPistol.maxAmmo;
                    //shooter.GetAmmo(remaining - MaxAmmo);
                }
                else
                {
                    //shooter.GetAmmo(magazineAmmo);
                    shooter.tpsPistol.remainingAmmo += shooter.tpsPistol.magazineSize;
                }
                playerInfo.BuyAmmo(250);
                if(shooter.weaponSlot == 1)
                {
                    PlayerUIManager.instance.totalAmmoText.text = string.Format("{0}", shooter.tpsPistol.remainingAmmo); // UI �Ѿ˰��� ������Ʈ
                }
            }
        }
    }
    public void BuyRifleAmmo()
    {
        if (playerInfo != null)
        {

            if (250 <= playerInfo.coin && shooter.tpsRifle.remainingAmmo < shooter.tpsRifle.maxAmmo)
            {
                if (shooter.tpsRifle.magazineSize + shooter.tpsRifle.remainingAmmo > shooter.tpsRifle.maxAmmo)
                {
                    shooter.tpsRifle.remainingAmmo = shooter.tpsRifle.maxAmmo;
                    //shooter.GetAmmo(remaining - MaxAmmo);
                }
                else
                {
                    //shooter.GetAmmo(magazineAmmo);
                    shooter.tpsRifle.remainingAmmo += shooter.tpsRifle.magazineSize;
                }
                playerInfo.BuyAmmo(250);
            }
            if (shooter.weaponSlot == 2)
            {
                PlayerUIManager.instance.totalAmmoText.text = string.Format("{0}", shooter.tpsRifle.remainingAmmo); // UI �Ѿ˰��� ������Ʈ
            }
        }
    }

    public void BuyAmmo()
    {
        if (playerInfo != null)
        {
         
            if (playerInfo.coin >= 250 && MaxAmmo > remaining)
            {
                if (magazineAmmo + remaining > MaxAmmo)
                {
                    shooter.GetAmmo(remaining- MaxAmmo);
                }else
                {
                    shooter.GetAmmo(magazineAmmo);

                }
               
                playerInfo.BuyAmmo(250);
            }
        }
    }


    public void BuySMG()
    {
        if (playerInfo != null)
        {
            if (2000 <= playerInfo.coin)
            {
                shooter.BuySMG();
                playerInfo.BuyAmmo(2000);

            }
        }
    }

    public void BuySCAR()
    {
        if (playerInfo != null)
        {
            if (5000 <= playerInfo.coin)
            {
                shooter.BuySCAR();
                playerInfo.BuyAmmo(5000);

            }
        }
    }
}
