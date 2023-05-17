using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField joinInputField;
    [SerializeField] private TMP_InputField createInputField;
    [SerializeField] private GameObject roomElementPrefab;
    [SerializeField] private Transform roomListContainer;

    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();

    private void Start()
    {
        ConnectToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    public void ConnectToMaster()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.NickName = "Tester";
        PhotonNetwork.AutomaticallySyncScene = true; //Required for PhotonNetwork.LoadLevel()
        PhotonNetwork.GameVersion = "v1"; // Only change if required;
        PhotonNetwork.UseRpcMonoBehaviourCache = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Not connected!");
            return;
        }

        if (string.IsNullOrEmpty(createInputField.text))
        {
            Debug.LogError("No course name is given!");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)10;


        PhotonNetwork.CreateRoom(createInputField.text, roomOptions);
    }

    public void ConnectToRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Not connected!");
            return;
        }

        PhotonNetwork.JoinRoom(joinInputField.text);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Could not connect: " + cause);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log(PhotonNetwork.LocalPlayer.NickName + PhotonNetwork.CurrentRoom.PlayerCount + " has joined!");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError("Could not join room!");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRooms();
        foreach (var room in roomList)
        {
            if (room.RemovedFromList || !room.IsVisible)
            {
                Debug.Log("Removing room, room removed: " + room.RemovedFromList + ", room is visible: " +
                          room.IsVisible);
                _cachedRoomList.Remove(room.Name);
                continue;
            }

            Debug.Log(room.Name);
            _cachedRoomList[room.Name] = room;
        }

        foreach (var room in _cachedRoomList.Values)
        {
            GameObject roomElement = Instantiate(roomElementPrefab, roomListContainer) as GameObject;
            roomElement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            roomElement.GetComponentInChildren<Button>().onClick.AddListener(() => JoinTargetRoom(room.Name));
        }
    }

    public override void OnLeftLobby()
    {
        ClearRooms();
    }

    public void JoinTargetRoom(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            LeaveLobby();
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinLobby()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public void LeaveLobby()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
    }

    public void StartCourse(string sceneName)
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(sceneName);
    }

    private void ClearRooms()
    {
        _cachedRoomList = new Dictionary<string, RoomInfo>();
    }
}