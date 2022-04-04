using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.Serialization;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_InputField joinRoomNameInputField;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Transform roomListContent;
    [SerializeField] private GameObject roomListItemPrefab;
    [SerializeField] private Transform playerListContent;
    [SerializeField] private GameObject playerListItemPrefab;
    [SerializeField] private GameObject startGameButton;
    public string playerName;
    public string nicknamee => PhotonNetwork.NickName;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        print("connecting to master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("connected to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    public override void OnJoinedLobby()
    {
        if (playerName == "")
            MenuManager.instance.OpenMenu("set name");
        else
        {
            OpenTitleMenu();
        }

        PhotonNetwork.NickName = playerName;

        print("Joined lobby");
        // PhotonNetwork.NickName = "Player" + Random.Range(0, 10).ToString("00");
        // if (playerName == "")
        //     PhotonNetwork.NickName = "Player" + Random.Range(0, 10).ToString("00");
        // else
        // PhotonNetwork.NickName = playerName;
    }

    public void OpenTitleMenu()
    {
        MenuManager.instance.OpenMenu("title");
    }

    public void SetPlayerName()
    {
        playerName = playerNameInputField.text;
        PhotonNetwork.NickName = playerName;
        OpenTitleMenu();
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        { return; }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("room");

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);

        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "failed creating room" + message;
        MenuManager.instance.OpenMenu("error");
    }

    public void OnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");
    }

    public void OnJoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        MenuManager.instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // foreach (Transform transform in roomListContent)
        // {
        //     Destroy(transform.gameObject);
        // }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;

            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);

            print(roomList[i].Name);
        }

        print("Room Updated");

    }

    public void JoinRoom()
    {

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }
}
