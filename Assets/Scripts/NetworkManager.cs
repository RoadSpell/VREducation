using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField joinInputField;
    [SerializeField] private TMP_InputField createInputField;

    private void Start()
    {
        ConnectToMaster();
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
}