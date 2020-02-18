using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyController : MonoBehaviourPunCallbacks
{
#pragma warning disable 649
    [SerializeField] private int waitingSceneIndex;
    [SerializeField] private int nbPlayerMax;
#pragma warning restore 649

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

    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(waitingSceneIndex);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
