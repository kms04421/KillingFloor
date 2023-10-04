using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using System;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject Lang_Panel, Room_Panel, UserRoom_Panel, Lobby_Panel, Login_Panel, Class_Panel, Store_Panel, Lobby_Screen, Option_Panel, PlayerProfile;

    [Header("Player")]
    public string localPlayerName = default;
    public string localPlayerLv = default;
    public string localPlayerClass = default;
    public int localCashItem = default;

    [Header("Login")]
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();
    public InputField EmailInput, PasswordInput, UsernameInput, LoginIDInput, LoginPWInput;
    public GameObject RegistBox;
    public GameObject LoginBox;
    public Text RegistInfoText;
    public Text LoginInfoText;

    [Header("Lobby")]
    public InputField UserSearchInput;
    public Text LobbyInfoText, UserNickNameText, UserNameText;

    [Header("Room")]
    public InputField SetDataInput;
    public GameObject SetDataBtnObj, MasterStartBtn, OtherReadyBtn;
    public Text UserRoomDataText, RoomNameInfoText, RoomNumInfoText;
    public Button MasterStartButton, OtherReadyButton;
    public GameObject masterStartBtnDisable;

    [Header("Store")]
    public Text CoinsValueText;
    public Text StarsValueText;
    public int coins = default;
    public int stars = default;
    public GameObject playerSample;

    public ItemToBuy[] Items;
    public GameObject ContentArea;

    public GameObject ItemObj;
    public GameObject InventoryContent;
    public enum State { Login, Lobby, Room, Class, Store, Option };
    [Header("Lobby UI")]
    public State state;
    public Image[] buttonBackGround;    // �κ� ��ư ���ÿ���
    public PlayerProfile[] playerInfo;

    bool isLoaded;
    int readyCheck = -1;
    int readyCount = 0;


    public static NetworkManager net_instance;
    void Awake()
    {
        if (net_instance == null)
        {
            net_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (net_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //��ȯ : �÷��̾���� �� ��ũ ���߱�
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        StartCoroutine(UpdatePlayerCount());
    }

    #region �÷�����
    // �̸��� ���� ���� : '@', '.' �� �־����
    // ��й�ȣ ���� ���� : 6~100 ���� ����
    // �̸� ���� ���� : 3~20 ���� ����
    public void Login()
    {
        LoginInfoText.text = "Loading...";
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = LoginIDInput.text, Password = LoginPWInput.text };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            LoginInfoText.text = "������...";

            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������
            SetLocalPlayerData();                   // �����÷��̾���� ����
            GetItemPrices();                        // ������ ���� ������
            UpdateInventory();                      // ���� �κ��丮 ������ ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) =>
            {
                Debug.Log("�α��� ����");
                LoginInfoText.text = "�α��� ����";

            });
    }
    public void Register()
    {
        RegistInfoText.text = "Loading...";
        // �̸���, ��й�ȣ, ���� �̸����� ��� ��û ����
        var request = new RegisterPlayFabUserRequest
        { Email = EmailInput.text, Password = PasswordInput.text, Username = UsernameInput.text, DisplayName = UsernameInput.text };

        PlayFabClientAPI.RegisterPlayFabUser(request, (result) =>
        {
            Debug.Log("ȸ������ ����");
            RegistInfoText.text = "ȸ������ ����";
            SetStat();          // ��� �ʱ�ȭ
            SetData("0");    // ���� ������ �ʱ�ȭ
            SetClass("Commando");
        },
            (error) =>
            {
                Debug.Log("ȸ������ ����");
                RegistInfoText.text = "ȸ������ ����";

            });

    }

    #region TestLogin
    //[Mijeong]230915 �׽�Ʈ�� �α��� �޼��� �߰�
    public void OnLoginTest01()
    {
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = "test01@test.com", Password = "000000" };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������
            SetLocalPlayerData();                   // �����÷��̾���� ����
            GetItemPrices();                        // ������ ���� ������
            UpdateInventory();                      // ���� �κ��丮 ������ ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) => Debug.Log("�α��� ����"));
    }
    public void OnLoginTest02()
    {
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = "test02@test.com", Password = "000000" };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������
            SetLocalPlayerData();                   // �����÷��̾���� ����
            GetItemPrices();                        // ������ ���� ������
            UpdateInventory();                      // ���� �κ��丮 ������ ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) => Debug.Log("�α��� ����"));
    }
    public void OnLoginTest03()
    {
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = "test03@test.com", Password = "000000" };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������
            SetLocalPlayerData();                   // �����÷��̾���� ����
            GetItemPrices();                        // ������ ���� ������
            UpdateInventory();                      // ���� �κ��丮 ������ ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) => Debug.Log("�α��� ����"));
    }
    public void OnLoginTest04()
    {
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = "test04@test.com", Password = "000000" };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������
            SetLocalPlayerData();                   // �����÷��̾���� ����
            GetItemPrices();                        // ������ ���� ������
            UpdateInventory();                      // ���� �κ��丮 ������ ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) => Debug.Log("�α��� ����"));
    }
    public void OnLoginTest05()
    {
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = "test05@test.com", Password = "000000" };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������
            SetLocalPlayerData();                   // �����÷��̾���� ����
            GetItemPrices();                        // ������ ���� ������
            UpdateInventory();                      // ���� �κ��丮 ������ ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) => Debug.Log("�α��� ����"));
    }
    public void OnLoginTest06()
    {
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = "test06@test.com", Password = "000000" };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������
            SetLocalPlayerData();                   // �����÷��̾���� ����
            GetItemPrices();                        // ������ ���� ������
            UpdateInventory();                      // ���� �κ��丮 ������ ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) => Debug.Log("�α��� ����"));
    }
    public void OnLoginTest07()
    {
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = "test07@test.com", Password = "000000" };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������
            SetLocalPlayerData();                   // �����÷��̾���� ����
            GetItemPrices();                        // ������ ���� ������
            UpdateInventory();                      // ���� �κ��丮 ������ ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) => Debug.Log("�α��� ����"));
    }
    public void OnLoginTest08()
    {
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = "test08@test.com", Password = "000000" };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������
            SetLocalPlayerData();                   // �����÷��̾���� ����
            GetItemPrices();                        // ������ ���� ������
            UpdateInventory();                      // ���� �κ��丮 ������ ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) => Debug.Log("�α��� ����"));
    }
    #endregion


    // ���� ��� �ʱ�ȭ
    void SetStat()
    {
        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "IDInfo", Value = 0 } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => { }, (error) => Debug.Log("�� �������"));
    }

    // PlayFab �������� ���� �������� �޼���
    void GetLeaderboard(string myID)
    {
        // PlayFab ���� ����Ʈ �ʱ�ȭ
        PlayFabUserList.Clear();

        for (int i = 0; i < 10; i++)
        {
            // �������� ���� ������ ��û ����
            var request =
                new GetLeaderboardRequest
                {
                    StartPosition = i * 100,    // ��� ���� ��ġ
                    StatisticName = "IDInfo",   // ��� �̸�
                    MaxResultsCount = 100,      // �ִ� ��� ����
                    ProfileConstraints =
                    new PlayerProfileViewConstraints() { ShowDisplayName = true }   // �÷��̾��� ���÷��� �̸� ǥ��
                };

            // PlayFab ���� �������� ���� ��û ����
            PlayFabClientAPI.GetLeaderboard(request, (result) =>
            {
                if (result.Leaderboard.Count == 0) return;

                // �÷��̾� ������ PlayFabUserList�� �߰��ϱ� ���� �������� ��� �ݺ�
                for (int j = 0; j < result.Leaderboard.Count; j++)
                {
                    PlayFabUserList.Add(result.Leaderboard[j]);

                    // �� PlayFab ID�� ã�� MyPlayFabInfo ������ ����
                    if (result.Leaderboard[j].PlayFabId == myID) MyPlayFabInfo = result.Leaderboard[j];
                }
            },
            (error) => { });

        }
    }

    #region ���� ������ ����
    // ���� ������ �����ϴ� �޼���
    public void SetData(string curData)
    {
        // ������Ʈ�� ����� ������ ��û ����
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "HomeLevel", curData } },   // "HomeLevel" Ű�� ���� ������ ����
            Permission = UserDataPermission.Public     // ������ ����
        };

        // PlayFab�� ���� ����� ������ ������Ʈ ��û ����
        PlayFabClientAPI.UpdateUserData(request, (result) =>
        {
            Debug.Log("SetData ���� -> " + result);
            Debug.Log($"curData : {curData}");

            localPlayerLv = curData;
        },
        (error) => Debug.Log("������ ���� ����"));
    }

    public void SetClass(string curData)
    {
        // ������Ʈ�� ����� ������ ��û ����
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "Class", curData } },   // "HomeLevel" Ű�� ���� ������ ����
            Permission = UserDataPermission.Public     // ������ ����
        };

        // PlayFab�� ���� ����� ������ ������Ʈ ��û ����
        PlayFabClientAPI.UpdateUserData(request, (result) =>
        {
            Debug.Log("SetData ���� -> " + result);
            Debug.Log($"curData : {curData}");

            localPlayerClass = curData;
        },
        (error) => Debug.Log("������ ���� ����"));
    }

    //[MiJeong] 231001 �����÷��̾� �� Null ���� ����
    public void SetLocalPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = MyPlayFabInfo.PlayFabId }, (result) =>
        {
            if (MyPlayFabInfo.DisplayName != null && MyPlayFabInfo.DisplayName != localPlayerName)
            {
                PhotonNetwork.LocalPlayer.NickName = MyPlayFabInfo.DisplayName;

                localPlayerName = MyPlayFabInfo.DisplayName;
                localPlayerLv = result.Data["HomeLevel"].Value;
                localPlayerClass = result.Data["Class"].Value;
                UserNameText.text = "Name: " + localPlayerName + "\nLevel: " + localPlayerLv;

                playerInfo[0].nickName.text = localPlayerName;
                playerInfo[0].level.text = localPlayerLv;
                playerInfo[0].className.text = localPlayerClass;
                SetClassIcon(0, result.Data["Class"].Value);
            }
            else
            {
                SetLocalPlayerData();
            }
        },
            (error) => Debug.Log("������ �ҷ����� ����" + error.ErrorMessage));
    }
    #endregion
    #endregion

    #region ���� Currency
    // ���� Currency �������� �޼���
    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }
    // ������ ���� ������ �޼���
    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        coins = result.VirtualCurrency["CN"];
        stars = result.VirtualCurrency["GD"];

        CoinsValueText.text = "Coins: " + coins.ToString();
        StarsValueText.text = "Golds: " + stars.ToString();

        Debug.Log(result);
        Debug.Log(CoinsValueText.text);
    }
    // ���� �߰� ��û �޼���
    public void OnAddVirtualCurrency(/*int addCoin*/)
    {
        int addCoin = 50;

        var request = new AddUserVirtualCurrencyRequest { VirtualCurrency = "CN", Amount = addCoin };
        PlayFabClientAPI.AddUserVirtualCurrency(request, GrantVirtualCurrencySuccess, OnError);
    }
    // ���� �߰� ��û ������ �ݹ�
    void GrantVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Add Coins Granted !");

        GetVirtualCurrencies();
        //coins += 50;
        //CoinsValueText.text = "Coins: " + coins.ToString();
        //StarsValueText.text = "Stars: " + stars.ToString();
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error: " + error.ErrorMessage);
    }
    #endregion

    #region Store_Panel
    public void StoreBtn()
    {
        Store_Panel.SetActive(true);
    }
    public void ExitStore_Panel()
    {
        Store_Panel.SetActive(false);
    }

    public void GetItemPrices()
    {
        GetCatalogItemsRequest request = new GetCatalogItemsRequest();
        request.CatalogVersion = "Player Items Catalog";
        PlayFabClientAPI.GetCatalogItems(request, result =>
        {
            List<CatalogItem> items = result.Catalog;
            foreach (CatalogItem item in items)
            {
                uint cost = item.VirtualCurrencyPrices["CN"];

                foreach (ItemToBuy editorItems in Items)
                {
                    if (editorItems.Name == item.ItemId)
                    {
                        editorItems.Cost = (int)cost;
                    }
                }
            }

            foreach (ItemToBuy i in Items)
            {

                GameObject itemObject = Instantiate(ItemObj, ContentArea.transform.position, Quaternion.identity);
                itemObject.transform.GetChild(1).GetComponent<Text>().text = i.Name;
                itemObject.transform.GetChild(2).GetComponent<Text>().text = "[" + i.Cost + " Coin]";
                itemObject.GetComponent<Image>().sprite = i.GetComponent<Image>().sprite;
                itemObject.GetComponent<Image>().preserveAspect = true;     // �̹��� ��Ⱦ�� �����ϵ��� ����

                itemObject.transform.SetParent(ContentArea.transform);
                itemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { MakePurchase(i.Name, i.Cost); });
                itemObject.transform.localScale = Vector3.one;  // ��ȯ�߰�
                itemObject.transform.SetParent(ContentArea.transform);
            }

        },
        error => { });
    }

    void UpdateInventory()
    {
        // InventoryContent�� ��� �ڽ� ������Ʈ ����
        foreach (Transform child in InventoryContent.transform)
        {
            Destroy(child.gameObject);
        }

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            List<ItemInstance> itemIv = result.Inventory;
            foreach (ItemInstance i in itemIv)
            {
                foreach (ItemToBuy editorI in Items)
                {
                    if (editorI.Name == i.ItemId)
                    {
                        GameObject itemObject = Instantiate(ItemObj, InventoryContent.transform.position, Quaternion.identity);
                        itemObject.transform.GetChild(1).GetComponent<Text>().text = i.ItemId;
                        itemObject.transform.GetChild(2).GetComponent<Text>().text = "[" + editorI.Cost + " Coin]";
                        itemObject.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "EQUIP";
                        itemObject.GetComponent<Image>().sprite = editorI.GetComponent<Image>().sprite;
                        itemObject.GetComponent<Image>().preserveAspect = true;     // �̹��� ��Ⱦ�� �����ϵ��� ����

                        itemObject.transform.SetParent(InventoryContent.transform);
                        itemObject.transform.localScale = Vector3.one;

                        //[MiJeong] 231002: �������� EQUIP �� �����ϱ� ����
                        itemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { PlayerColorChange(i.ItemId); });


                    }

                }
            }
        }, error => { });
    }

    void MakePurchase(string name, int price)
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.CatalogVersion = "Player Items Catalog";
        request.ItemId = name;
        request.Price = price;
        request.VirtualCurrency = "CN";

        PlayFabClientAPI.PurchaseItem(request, result =>
        {
            Debug.Log("���� ����");
            //UpdateInventory();

            foreach (ItemToBuy editorItems in Items)
            {
                if (editorItems.Name == name)
                {
                    GameObject itemObject = Instantiate(ItemObj, InventoryContent.transform.position, Quaternion.identity);
                    itemObject.transform.GetChild(1).GetComponent<Text>().text = name;
                    itemObject.transform.GetChild(2).GetComponent<Text>().text = "[" + price + " Coin]";
                    itemObject.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "EQUIP";
                    itemObject.GetComponent<Image>().sprite = editorItems.GetComponent<Image>().sprite;
                    itemObject.GetComponent<Image>().preserveAspect = true;     // �̹��� ��Ⱦ�� �����ϵ��� ����

                    itemObject.transform.SetParent(InventoryContent.transform);
                    itemObject.transform.localScale = Vector3.one;
                    //[MiJeong] 231002: �������� EQUIP �� �����ϱ� ����
                    itemObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { PlayerColorChange(name); });

                }
            }


            GetVirtualCurrencies();
        },
            error => { Debug.Log(error.ErrorMessage); });
    }

    //[Mijeong] 231002: �÷��̾� ��Ų ������ �� ����
    public void PlayerColorChange(string colorName)
    {

        switch (colorName)
        {
            case "Default":
                localCashItem = 0;
                break;

            case "SWAT":
                localCashItem = 1;
                break;

            case "GreenArmyMan":
                localCashItem = 2;
                break;
            case "GoldMan":
                localCashItem = 3;
                break;
        }

        playerSample.GetComponent<PlayerShop>().EquipMat(localCashItem);
        Debug.Log(colorName);
    }
    #endregion

    #region Lang_Panel
    public void LangBtn()
    {
        Lang_Panel.SetActive(true);
    }
    public void ExitLang_Panel()
    {
        Lang_Panel.SetActive(false);
    }
    #endregion

    #region Lobby
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    // Photon �������� ����ȭ�� �Ϸ� �� CountOfPlayers�� ������Ʈ�ϵ��� �ڸ�ƾ ���
    private int currentPlayerCount = 0;

    private IEnumerator UpdatePlayerCount()
    {
        while (true)
        {
            int newPlayerCount = PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms;

            if (newPlayerCount != currentPlayerCount)
            {
                LobbyInfoText.text = "�κ� : " + newPlayerCount + " / ���� : " + PhotonNetwork.CountOfPlayers;
                currentPlayerCount = newPlayerCount;
            }

            yield return new WaitForSeconds(1f); // 1�ʸ��� ������Ʈ
        }
    }

    public override void OnJoinedLobby()
    {
        // �濡�� �κ�� �� �� �����̾���, �α����ؼ� �κ�� �� �� PlayFabUserList�� ä���� �ð����� ������
        if (isLoaded)
        {
            SetLocalPlayerProfile();
            ShowPanel(Lobby_Panel);
            ShowUserNickName();
        }
        else Invoke("OnJoinedLobbyDelay", 1);
    }

    // ���� �÷��̾� �г��� �Ҵ�
    void OnJoinedLobbyDelay()
    {
        isLoaded = true;

        SetLocalPlayerProfile();
        LobbyScreen(true);      // �κ� ��濵�� �Ҵ�.
        state = State.Lobby;    // ���� �κ�� ����
        PlayerProfile.gameObject.SetActive(true);

        ShowPanel(Lobby_Panel);
        ShowUserNickName();
    }

    public void SetLocalPlayerProfile()
    {
        //PhotonNetwork.LocalPlayer.NickName = MyPlayFabInfo.DisplayName;

        //Debug.Log($"�����Ʈ��ũ �г��� : {PhotonNetwork.LocalPlayer.NickName}");
        //playerInfo[0].nickName.text = string.Format(PhotonNetwork.LocalPlayer.NickName);
        //Debug.Log($"playerInfo[0].nickName: {playerInfo[0].nickName.text}");
        //playerInfo[0].level.text = string.Format(localPlayerLv);
        //playerInfo[0].className.text = localPlayerClass;

        if (playerInfo[0].nickName == null)
        {
            Debug.Log("���� �÷��̾� �����Ͱ� �ʱ�ȭ �Ǿ �ٽ� �ҷ���");
            SetLocalPlayerData();
        }

        SetClassIcon(0, localPlayerClass);
    }
    void ShowPanel(GameObject curPanel)
    {

        Room_Panel.SetActive(false);
        UserRoom_Panel.SetActive(false);
        Lobby_Panel.SetActive(false);
        Login_Panel.SetActive(false);
        Store_Panel.SetActive(false);

        curPanel.SetActive(true);
    }

    void ShowUserNickName()
    {
        UserNickNameText.text = "";
        for (int i = 0; i < PlayFabUserList.Count; i++)
        {
            UserNickNameText.text += PlayFabUserList[i].DisplayName + "\n";
        }
    }

    public void XBtn()
    {
        if (PhotonNetwork.InLobby) PhotonNetwork.Disconnect();
        else if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        LobbyScreen(false);// �κ� ����
        isLoaded = false;
        ShowPanel(Login_Panel);
    }
    #endregion

    //[Mijeong] 230927 ���ʿ��� �ڵ� ����
    #region UserRoom
    public void JoinOrCreateRoom(string roomName)
    {
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 6 }, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("�游������");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("����������");


    //[Mijeong] 230927 ���ʿ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        RoomRenewal();

        // [Mijeong] 230925 : ���� �غ�,���� ��ư Ȱ��ȭ �޼��� �߰�
        PlayerReadyBtn();

        string curName = PhotonNetwork.CurrentRoom.Name;
        RoomNameInfoText.text = curName;
        Debug.Log(curName);

        if (curName == "ROOM1" || curName == "ROOM2" || curName == "ROOM3" || curName == "ROOM4")
        {
            state = State.Room;
            ShowPanel(Room_Panel);
        }
        else { };
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) => RoomRenewal();

    public override void OnPlayerLeftRoom(Player otherPlayer) => RoomRenewal();

    void RoomRenewal()
    {

        UserNickNameText.text = "";
        ResetPlayerUI();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // �� ���� UI ����
            int playerIndex = i + 1; // ���� �÷��̾� �ε��� ����

            playerInfo[playerIndex].gameObject.SetActive(true);
            playerInfo[playerIndex].nickName.text = PhotonNetwork.PlayerList[i].NickName;

            for (int j = 0; j < PlayFabUserList.Count; j++)
            {
                if (PhotonNetwork.PlayerList[i].NickName == PlayFabUserList[j].DisplayName)
                {
                    int currentPlayerIndex = playerIndex; // Ŭ�������� ����� ���� �÷��̾� �ε���

                    PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = PlayFabUserList[j].PlayFabId }, (result) =>
                    {
                        if (result.Data.ContainsKey("HomeLevel"))
                        {
                            string levelValue = result.Data["HomeLevel"].Value;
                            playerInfo[currentPlayerIndex].level.text = levelValue;
                            playerInfo[currentPlayerIndex].className.text = result.Data["Class"].Value;
                            SetClassIcon(currentPlayerIndex, result.Data["Class"].Value);

                            Debug.Log($"Player {currentPlayerIndex}�� ����: {levelValue}");
                        }
                        else
                        {
                            Debug.Log($"Player {currentPlayerIndex}�� ���� ������ �����ϴ�.");
                        }
                    },
                    (error) => Debug.Log("���� �ҷ����� ����"));
                }
            }
        }

        RoomNumInfoText.text = PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ� �ο�";
    }
    void ResetPlayerUI()
    {
        for (int i = 0; i < 6; i++)
        { playerInfo[i + 1].gameObject.SetActive(false); }
    }
    public override void OnLeftRoom()
    {
        SetDataInput.gameObject.SetActive(false);
        SetDataBtnObj.SetActive(false);

        SetDataInput.text = "";
        UserSearchInput.text = "";
        UserRoomDataText.text = "";
    }

    //public void SetDataBtn()
    //{
    //    // �ڱ��ڽ��� �濡���� �� ������ �����ϰ�, �� ���� �� 1�� �ڿ� �� �ҷ�����
    //    SetData(SetDataInput.text);
    //    Invoke("SetDataBtnDelay", 1);
    //}

    //void SetDataBtnDelay() => GetData(PhotonNetwork.CurrentRoom.CustomProperties["PlayFabID"].ToString());
    #endregion

    // [Mijeong] 230925 : ���� �غ�,���� üũ �޼��� �߰�
    #region Player Ready Check
    void PlayerReadyBtn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MasterStartBtn.SetActive(true); // �� �����͸� ��ŸƮ ��ư Ȱ��ȭ
            OtherReadyBtn.SetActive(false);
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                masterStartBtnDisable.gameObject.SetActive(false);
            }
        }
    }

    [PunRPC]
    public void OnStartCheck(int readyCheck, int index)
    {
        if(readyCheck == -1)
        {
            playerInfo[index].ready.gameObject.SetActive(false);
        }
        else if (readyCheck == 1)
        {
            playerInfo[index].ready.gameObject.SetActive(true);
        }

        // �Ʒ��� ������Ŭ���̾�Ʈ�� �ƴϸ� ����
        if(!PhotonNetwork.IsMasterClient)
        { return; }
        photonView.RPC("OnStartCheck", RpcTarget.Others, readyCheck, index);


        readyCount += readyCheck;
        Debug.Log($"readyCount: {readyCount}");
        Debug.Log($"readyCheck: {readyCheck}");

        if (readyCount == PhotonNetwork.CurrentRoom.PlayerCount - 1)
        {
            MasterStartButton.interactable = true;
            masterStartBtnDisable.gameObject.SetActive(false);
            Debug.Log("��� Ŭ���̾�Ʈ �غ� �Ϸ�");
        }
        else
        {
            MasterStartButton.interactable = false;
            masterStartBtnDisable.gameObject.SetActive(true);

            Debug.Log("��� Ŭ���̾�Ʈ�� �غ����� �ʾҽ��ϴ�.");
        }
    }

    public void OnReadyCheck()
    {
        int profileIndex = 0; // ����� ������ �������� �ѹ�

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            int playerIndex = i + 1; // ���� �÷��̾� �ε��� ����

            if (PhotonNetwork.PlayerList[i].NickName == localPlayerName)
            {
                profileIndex = playerIndex;
            }

        }


        switch (readyCheck)
        {
            case -1:
                readyCheck = 1;
                photonView.RPC("OnStartCheck", RpcTarget.MasterClient, readyCheck, profileIndex);
                break;

            case 1:
                readyCheck = -1;
                photonView.RPC("OnStartCheck", RpcTarget.MasterClient, readyCheck, profileIndex);
                OtherReadyBtn.GetComponent<Button>().interactable = false;
                OtherReadyBtn.GetComponent<Button>().interactable = true;
                break;
        }
    }
    #endregion

    #region PlayScene Load
    public void OnPlayerTestScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("PlayerTestScene");
        }
    }
    public void OnGunTestScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GunTestScene");
        }
    }
    public void OnZombieTestScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("ZombieTestScene");
        }
    }
    public void OnMainTestScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main");
        }
    }
    public void OnLevelTestScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("LevelTestScene");
        }
    }
    #endregion

    // ��ȯ�ۼ�
    #region LobbyUI

    // ��ȯ
    void LobbyScreen(bool isLobby)
    {
        Lobby_Screen.SetActive(isLobby);
    }


    public void HomeButton()
    {
        if (state == State.Lobby || state == State.Room)
        {
            return;
        }

        // �κ�� �κ� ���·�, �濡 ������ ������ ���� ����
        if (PhotonNetwork.InLobby) { state = State.Lobby; }
        else if (PhotonNetwork.InRoom)
        {
            state = State.Room;
            RoomRenewal();

        }

        SetButtonColor(0);
        SetPanel();
    }
    // �� ������ ��ư
    public void LeaveRoomButton()
    {
        state = State.Lobby;
        SetPanel();
        PhotonNetwork.LeaveRoom();
    }
    public void ClassButton()
    {
        if (state == State.Class)
        {
            return;
        }
        state = State.Class;

        SetButtonColor(3);
        SetPanel();
    }
    // ���� ��ư
    public void StoreButton()
    {
        if (state == State.Store)
        {
            return;
        }
        state = State.Store;

        SetButtonColor(1);
        SetPanel();
    }
    public void OptionButton()
    {
        if (state == State.Option)
        {
            return;
        }
        state = State.Option;

        SetButtonColor(2);
        SetPanel();
    }
    public void ExitButton()
    {
        state = State.Login;
        LobbyScreen(false);
        PhotonNetwork.Disconnect();

        SetButtonColor(0);
        SetPanel();
    }
    public void RegistButton()
    {
        RegistBox.gameObject.SetActive(!RegistBox.gameObject.activeSelf);
        LoginBox.gameObject.SetActive(!RegistBox.gameObject.activeSelf);
    }


    // ��ư ����� ���� �������ִ� �޼���
    public void SetButtonColor(int num)
    {
        buttonBackGround[0].color = new Color(255, 255, 255, 0);    // Home
        buttonBackGround[1].color = new Color(255, 255, 255, 0);    // Store
        buttonBackGround[2].color = new Color(255, 255, 255, 0);    // Option
        buttonBackGround[3].color = new Color(255, 255, 255, 0);    // Class
                                                                    // ��û�� ��ư�� ���� ���ֱ�
        buttonBackGround[num].color = new Color(255, 255, 255, 0.8f);

    }
    // ���¿� ���� �г� ���� �޼���
    public void SetPanel()
    {
        Login_Panel.SetActive(false);
        Lobby_Panel.SetActive(false);
        Room_Panel.SetActive(false);
        Class_Panel.SetActive(false);
        UserRoom_Panel.SetActive(false);
        Option_Panel.SetActive(false);
        Store_Panel.SetActive(false);
        PlayerProfile.SetActive(false);

        playerSample.SetActive(false);


        switch (state)
        {
            case State.Lobby:
                Lobby_Panel.SetActive(true);
                PlayerProfile.SetActive(true);
                break;
            case State.Room:
                Room_Panel.SetActive(true);
                PlayerProfile.SetActive(false);
                break;
            case State.Store:
                Store_Panel.SetActive(true);
                PlayerProfile.SetActive(false);
                playerSample.SetActive(true);

                break;
            case State.Option:
                Option_Panel.SetActive(true);
                PlayerProfile.SetActive(false);
                break;
            case State.Login:
                Login_Panel.SetActive(true);
                PlayerProfile.SetActive(true);
                break;
            case State.Class:
                Class_Panel.SetActive(true);
                PlayerProfile.SetActive(true);
                break;
        }
    }

    #endregion

    #region SetClass
    public void ClassChangeCommando()
    {
        NetworkManager.net_instance.localPlayerClass = "Commando";
        Debug.Log(NetworkManager.net_instance.localPlayerClass + "Ŭ���� ����");
        // �����Ϳ� ����� ���� ������ �����ϱ� ���ؼ� �� �ʿ�
        NetworkManager.net_instance.SetClass(NetworkManager.net_instance.localPlayerClass);

        playerInfo[0].className.text = NetworkManager.net_instance.localPlayerClass;
        SetClassIcon(0, "Commando");
        SetClass("Commando");
        SetLocalPlayerData();

        photonView.RPC("SyncClass", RpcTarget.All);

    }
    public void ClassChangeDemolitionist()
    {
        NetworkManager.net_instance.localPlayerClass = "Demolitionist";
        Debug.Log(NetworkManager.net_instance.localPlayerClass + "Ŭ���� ����");
        // �����Ϳ� ����� ���� ������ �����ϱ� ���ؼ� �� �ʿ�
        NetworkManager.net_instance.SetClass(NetworkManager.net_instance.localPlayerClass);

        playerInfo[0].className.text = NetworkManager.net_instance.localPlayerClass;
        SetClassIcon(0, "Demolitionist");
        SetClass("Demolitionist");
        SetLocalPlayerData();

        photonView.RPC("SyncClass", RpcTarget.All);

    }

    public void SetClassIcon(int index, string playerClass)
    {
        playerInfo[index].classIcon[0].SetActive(false);    // �ڸ���
        playerInfo[index].classIcon[1].SetActive(false);    // �������ŴϽ�Ʈ

        switch (playerClass)
        {
            case "Commando":
                playerInfo[index].classIcon[0].SetActive(true);
                break;
            case "Demolitionist":
                playerInfo[index].classIcon[1].SetActive(true);
                break;
        }
    }

    [PunRPC]
    public void SyncClass()
    {
        RoomRenewal();
    }
    #endregion

}