using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerLevel : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinGame()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Joined game");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room...");
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Creating room.");

        string roomName = "room#" + Random.Range(0, 999);
        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 4 };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log(roomName + " has been created.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create new room. Trying again...");
        CreateRoom();
    }
}
