using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using Photon.Realtime;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
#pragma warning disable 649
    [SerializeField] private int minPlayerToStartGame;

    [SerializeField] private int multiSceneIndex;
    [SerializeField] private Text nbPlayerTxt;
#pragma warning restore 649

    public void Start()
    {
        UpdatePlayerCount();
        Debug.Log("Waiting room joined");
    }

    public void Update()
    {
        
    }

    private void UpdatePlayerCount()
    {
        nbPlayerTxt.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        
        if(PhotonNetwork.CurrentRoom.PlayerCount >= minPlayerToStartGame)
        {
            StartGame();
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerCount();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerCount();
    }

    void StartGame()
    {
        Debug.Log("Starting game..");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(multiSceneIndex);
        }
    }
}
