using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyController : MonoBehaviourPunCallbacks
{
#pragma warning disable 649
    [SerializeField] private int nbPlayerMax;
#pragma warning restore 649

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to master server..");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinGame()
    {
        Debug.Log("Joining random room..");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room.");
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Creating room.");

        int rdm = Random.Range(0, 999);
        string roomName = "room#" + rdm;
        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)nbPlayerMax };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log(roomName + " created.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create new room. Trying again..");
        CreateRoom();
    }
}
