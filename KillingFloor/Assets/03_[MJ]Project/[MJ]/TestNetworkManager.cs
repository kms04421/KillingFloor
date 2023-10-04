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

public class TestNetworkManager : MonoBehaviourPunCallbacks
{
    public static TestNetworkManager instance;

    public GameObject Lang_Panel, Room_Panel, UserRoom_Panel, Lobby_Panel, Login_Panel, Store_Panel, Lobby_Screen, Option_Panel;

    public string localPlayerName = default;
    public string localPlayerLv = default;

    [Header("Login")]
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();
    public InputField EmailInput, PasswordInput, UsernameInput;

    [Header("Lobby")]
    public InputField UserSearchInput;
    public Text LobbyInfoText, UserNickNameText, UserNameText;

    [Header("Room")]
    public InputField SetDataInput;
    public GameObject SetDataBtnObj, MasterStartBtn, OtherReadyBtn;
    public Text UserRoomDataText, RoomNameInfoText, RoomNumInfoText;
    public Button MasterStartButton, OtherReadyButton;

    [Header("Store")]
    public Text CoinsValueText;
    public Text StarsValueText;

    public int coins = default;
    public int stars = default;

    bool isLoaded;
    int readyCheck = -1;
    int readyCount = 0;

    public enum State { Login, Lobby, Room, Store, Option };
    [Header("Lobby UI")]
    public State state;
    public Image[] buttonBackGround;    // �κ� ��ư ���ÿ���
    public PlayerProfile[] playerInfo;

    void Awake()
    {
        instance = this;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //// ���� ��Ʈ��ũ �ӵ� ����ȭ ����
        //PhotonNetwork.SendRate = 60;
        //PhotonNetwork.SerializationRate = 30;

        //��ȯ : �÷��̾���� �� ��ũ ���߱�
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region �÷�����
    // �̸��� ���� ���� : '@', '.' �� �־����
    // ��й�ȣ ���� ���� : 6~100 ���� ����
    // �̸� ���� ���� : 3~20 ���� ����
    public void Login()
    {
        // �̸��ϰ� ��й�ȣ ����ؼ� �α��� ��û
        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // �α��� ������ ����
            GetLeaderboard(result.PlayFabId);       // PlayFab �������� ������
            GetVirtualCurrencies();                 // ���� Currency ������

            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����
        },
            (error) => Debug.Log("�α��� ����"));
    }
    public void Register()
    {
        // �̸���, ��й�ȣ, ���� �̸����� ��� ��û ����
        var request = new RegisterPlayFabUserRequest
        { Email = EmailInput.text, Password = PasswordInput.text, Username = UsernameInput.text, DisplayName = UsernameInput.text };

        PlayFabClientAPI.RegisterPlayFabUser(request, (result) =>
        {
            Debug.Log("ȸ������ ����");
            SetStat();          // ��� �ʱ�ȭ
            SetData("0");    // ���� ������ �ʱ�ȭ
        },
            (error) => Debug.Log("ȸ������ ����"));

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
            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����

            GetVirtualCurrencies();                 // ���� Currency ������

            SetLocalPlayerData();
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
            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����

            GetVirtualCurrencies();                 // ���� Currency ������

            SetLocalPlayerData();
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
            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����

            GetVirtualCurrencies();                 // ���� Currency ������

            SetLocalPlayerData();
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
            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����

            GetVirtualCurrencies();                 // ���� Currency ������

            SetLocalPlayerData();
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
            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����

            GetVirtualCurrencies();                 // ���� Currency ������

            SetLocalPlayerData();
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
            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����

            GetVirtualCurrencies();                 // ���� Currency ������

            SetLocalPlayerData();
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
            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����

            GetVirtualCurrencies();                 // ���� Currency ������

            SetLocalPlayerData();
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
            PhotonNetwork.ConnectUsingSettings();   // Photon ���� ����

            GetVirtualCurrencies();                 // ���� Currency ������

            SetLocalPlayerData();
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

    // ���� ������ �������� �޼���
    void GetData(string curID)
    {
        // ����� �����͸� ������ ��û ����
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = curID }, (result) =>
        {
            UserRoomDataText.text = "����ID" + curID + "\n" + result.Data["HomeLevel"].Value;
            playerInfo[0].level.text = result.Data["HomeLevel"].Value;
        },  // ��ȯ ���� �������� ���� �߰�

        (error) => Debug.Log("������ �ҷ����� ����"));

    }

    //[MiJeong] 230925 �ּ� ����
    void SetLocalPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = MyPlayFabInfo.PlayFabId }, (result) =>
        {
            Debug.Log("result: " + result);
            Debug.Log(MyPlayFabInfo.DisplayName + result.Data["HomeLevel"].Value);

            localPlayerName = MyPlayFabInfo.DisplayName;
            localPlayerLv = result.Data["HomeLevel"].Value;

            UserNameText.text = "Name: " + localPlayerName + "\nLevel: " + localPlayerLv;
        },
            (error) => Debug.Log("������ �ҷ����� ����"));
    }
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
        stars = result.VirtualCurrency["ST"];

        CoinsValueText.text = "Coins: " + coins.ToString();
        StarsValueText.text = "Stars: " + stars.ToString();

        Debug.Log(result);
        Debug.Log(CoinsValueText.text);
    }
    // ���� �߰� �޼���
    public void OnAddVirtualCurrency()
    {
        var request = new AddUserVirtualCurrencyRequest { VirtualCurrency = "CN", Amount = 50 };
        PlayFabClientAPI.AddUserVirtualCurrency(request, GrantVirtualCurrencySuccess, OnError);
    }
    void GrantVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Add 50 Coins Granted !");

        coins += 50;
        CoinsValueText.text = "Coins: " + coins.ToString();
        StarsValueText.text = "Stars: " + stars.ToString();
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error: " + error.ErrorMessage);
    }
    #endregion

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

    //LEGACY:
    //void Update()
    //{
    //    LobbyInfoText.text =
    //    "�κ� : " + (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms)
    //    + " / ���� : " + PhotonNetwork.CountOfPlayers;
    //}

    // Photon �������� ����ȭ�� �Ϸ� �� CountOfPlayers�� ������Ʈ�ϵ��� �ڸ�ƾ ���
    private int currentPlayerCount = 0;

    void Start()
    {
        StartCoroutine(UpdatePlayerCount());
    }

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
            ShowPanel(Lobby_Panel);
            ShowUserNickName();
        }
        else Invoke("OnJoinedLobbyDelay", 3);
    }

    // ���� �÷��̾� �г��� �Ҵ�
    void OnJoinedLobbyDelay()
    {
        isLoaded = true;
        PhotonNetwork.LocalPlayer.NickName = MyPlayFabInfo.DisplayName;

        LobbyScreen(true);      // �κ� ��濵�� �Ҵ�.
        state = State.Lobby;    // ���� �κ�� ����
        playerInfo[0].nickName.text = string.Format(PhotonNetwork.LocalPlayer.NickName);
        playerInfo[0].level.text = string.Format(localPlayerLv);  // ToDo : ���� �־����

        ShowPanel(Lobby_Panel);
        ShowUserNickName();
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

    #region Store_Panel
    public void StoreBtn()
    {
        Store_Panel.SetActive(true);
    }
    public void ExitStore_Panel()
    {
        Store_Panel.SetActive(false);
    }
    #endregion

    #region UserRoom
    public void JoinOrCreateRoom(string roomName)
    {
        if (roomName == "������")
        {
            //PlayFabUserList�� ǥ���̸��� �Է¹��� �г����� ���ٸ� PlayFabID�� Ŀ���� ������Ƽ�� �ְ� ���� �����
            for (int i = 0; i < PlayFabUserList.Count; i++)
            {
                if (PlayFabUserList[i].DisplayName == UserSearchInput.text)
                {
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.MaxPlayers = 6;
                    roomOptions.CustomRoomProperties = new Hashtable() { { "PlayFabID", PlayFabUserList[i].PlayFabId } };
                    PhotonNetwork.JoinOrCreateRoom(UserSearchInput.text + "���� ����â", roomOptions, null);

                    return;
                }
            }
            print("�г����� ��ġ���� �ʽ��ϴ�");
        }
        else PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 6 }, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("�游������");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("����������");



    public override void OnJoinedRoom()
    {
        RoomRenewal();

        // [Mijeong] 230925 : ���� �غ�,���� ��ư Ȱ��ȭ �޼��� �߰�
        PlayerReadyBtn();

        string curName = PhotonNetwork.CurrentRoom.Name;
        RoomNameInfoText.text = curName;
        Debug.Log(curName);

        if (curName == "ROOM1" || curName == "ROOM2" || curName == "ROOM3" || curName == "ROOM4") ShowPanel(Room_Panel);

        //�������̸� ������ ��������
        else
        {
            ShowPanel(UserRoom_Panel);

            string curID = PhotonNetwork.CurrentRoom.CustomProperties["PlayFabID"].ToString();
            GetData(curID);

            // ���� �� PlatyFabID Ŀ���� ������Ƽ�� ���� PlayFabID�� ���ٸ� ���� ������ �� ����
            if (curID == MyPlayFabInfo.PlayFabId)
            {
                RoomNameInfoText.text += " (���� ����â)";

                SetDataInput.gameObject.SetActive(true);
                SetDataBtnObj.SetActive(true);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) => RoomRenewal();

    public override void OnPlayerLeftRoom(Player otherPlayer) => RoomRenewal();

    //// REGACY:
    //void RoomRenewal()
    //{
    //    UserNickNameText.text = "";
    //    ResetPlayerUI();
    //    PlayFabClientAPI.GetUserData(new GetUserDataRequest(), (result) =>
    //    {
    //        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //        {
    //            UserNickNameText.text += PhotonNetwork.PlayerList[i].NickName + " : " + result.Data["HomeLevel"].Value + "\n";

    //            // �� ���� UI ����
    //            playerInfo[i + 1].gameObject.SetActive(true);
    //            playerInfo[i + 1].nickName.text = PhotonNetwork.PlayerList[i].NickName;
    //            playerInfo[i + 1].level.text = result.Data["HomeLevel"].Value;
    //        }
    //    },
    //    (error) => { Debug.Log("���� �ҷ����� ����"); }
    //    );
    //// REGACY2:
    //    RoomNumInfoText.text = PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ� �ο�";
    ////}
    //void RoomRenewal()
    //{
    //    UserNickNameText.text = "";
    //    ResetPlayerUI();

    //    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //    {

    //        // �� ���� UI ����
    //        playerInfo[i + 1].gameObject.SetActive(true);
    //        playerInfo[i + 1].nickName.text = PhotonNetwork.PlayerList[i].NickName;

    //        for (int j = 0; j < PlayFabUserList.Count; j++)
    //        {
    //            if (PhotonNetwork.PlayerList[i].NickName == PlayFabUserList[j].DisplayName)
    //            {
    //                PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = PlayFabUserList[j].PlayFabId }, (result) =>
    //                {
    //                    Debug.Log(result.Data["HomeLevel"].Value);
    //                    playerInfo[i+1].level.text = result.Data["HomeLevel"].Value;

    //                    //Debug.Log(PhotonNetwork.PlayerList[i].NickName);
    //                    Debug.Log($"playerInfo[i + 1].level: {playerInfo[i + 1].level.text}");
    //                    //UserNickNameText.text += PhotonNetwork.PlayerList[i].NickName + " : " + result.Data["HomeLevel"].Value + "\n";
    //                },

    //                (error) => Debug.Log("���� �ҷ����� ����"));
    //            }
    //        }
    //    }

    //    RoomNumInfoText.text = PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ� �ο�";
    //}
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

    public void SetDataBtn()
    {
        // �ڱ��ڽ��� �濡���� �� ������ �����ϰ�, �� ���� �� 1�� �ڿ� �� �ҷ�����
        SetData(SetDataInput.text);
        Invoke("SetDataBtnDelay", 1);
    }

    void SetDataBtnDelay() => GetData(PhotonNetwork.CurrentRoom.CustomProperties["PlayFabID"].ToString());
    #endregion

    // [Mijeong] 230925 : ���� �غ�,���� üũ �޼��� �߰�
    #region Player Ready Check
    void PlayerReadyBtn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MasterStartBtn.SetActive(true);
            OtherReadyBtn.SetActive(false);
        }
    }

    [PunRPC]
    public void OnStartCheck(int readyCheck)
    {
        readyCount += readyCheck;
        Debug.Log($"readyCount: {readyCount}");
        Debug.Log($"readyCheck: {readyCheck}");

        if (readyCount == PhotonNetwork.CurrentRoom.PlayerCount - 1)
        {
            MasterStartButton.interactable = true;
            Debug.Log("��� Ŭ���̾�Ʈ �غ� �Ϸ�");
        }
        else
        {
            MasterStartButton.interactable = false;
            Debug.Log("��� Ŭ���̾�Ʈ�� �غ����� �ʾҽ��ϴ�.");
        }
    }

    public void OnReadyCheck()
    {
        switch (readyCheck)
        {
            case -1:
                readyCheck = 1;
                photonView.RPC("OnStartCheck", RpcTarget.MasterClient, readyCheck);
                break;

            case 1:
                readyCheck = -1;
                photonView.RPC("OnStartCheck", RpcTarget.MasterClient, readyCheck);
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
            PhotonNetwork.LoadLevel("MainLoad");
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
        if (PhotonNetwork.InLobby) state = State.Lobby;
        else if (PhotonNetwork.InRoom) state = State.Room;

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


    // ��ư ����� ���� �������ִ� �޼���
    public void SetButtonColor(int num)
    {
        buttonBackGround[0].color = new Color(255, 255, 255, 0);    // Home
        buttonBackGround[1].color = new Color(255, 255, 255, 0);    // Store
        buttonBackGround[2].color = new Color(255, 255, 255, 0);    // Option
                                                                    // ��û�� ��ư�� ���� ���ֱ�
        buttonBackGround[num].color = new Color(255, 255, 255, 0.8f);

    }
    // ���¿� ���� �г� ���� �޼���
    public void SetPanel()
    {
        Login_Panel.SetActive(false);
        Lobby_Panel.SetActive(false);
        Room_Panel.SetActive(false);
        UserRoom_Panel.SetActive(false);
        Option_Panel.SetActive(false);
        Store_Panel.SetActive(false);

        switch (state)
        {
            case State.Lobby:
                Lobby_Panel.SetActive(true);
                break;
            case State.Room:
                Room_Panel.SetActive(true);
                break;
            case State.Store:
                Store_Panel.SetActive(true);
                break;
            case State.Option:
                Option_Panel.SetActive(true);
                break;
            case State.Login:
                Login_Panel.SetActive(true);
                break;
        }
    }

    #endregion
}