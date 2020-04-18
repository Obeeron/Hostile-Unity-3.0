using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
#pragma warning disable 649
    [SerializeField] private Text nbPlayerTxt;
    [SerializeField] private int minPlayerToStartGame;

    [SerializeField] private int multiSceneIndex;

    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private Text countdownTxt;
    [SerializeField] private int fullTimer;
    [SerializeField] private int shortTimer;

    [SerializeField] private PhotonView phView;
    private bool startingGame = false;
#pragma warning restore 649

    bool countingDown;
    float currentTimer;

    public void Start()
    {
        Debug.Log("Waiting room joined");
        phView = GetComponent<PhotonView>();
        countingDown = false;
        currentTimer = fullTimer;
        UpdatePlayerCount();
    }

    public void Update()
    {
        if (countingDown)
            UpdateTimer();
    }


    void StartGame()
    {
        countingDown = false;
        startingGame = true;
        Debug.Log("Starting game..");
        this.GetComponent<Joueur.Player_Choices>().Choosing();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(multiSceneIndex);
        }
    }

    private void UpdateTimer()
    {
        currentTimer -= Time.deltaTime;
        countdownTxt.text = string.Format("{0:00}", currentTimer);
        if (currentTimer <= 0 && !startingGame)
            StartGame();   
    }

    private void UpdatePlayerCount()
    {
        nbPlayerTxt.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        
        if(PhotonNetwork.CurrentRoom.PlayerCount >= minPlayerToStartGame)
        {
            countingDown = true;
            countdownPanel.SetActive(true);
        }
        else
        {
            countdownPanel.SetActive(false);
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        countingDown = false;
        currentTimer = fullTimer;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerCount();

        if (PhotonNetwork.IsMasterClient)
            phView.RPC("RPC_SynchTimer", RpcTarget.AllViaServer, currentTimer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerCount();
    }
    
    [PunRPC]
    private void RPC_SynchTimer(float timerValue)
    {
        currentTimer = timerValue;
    }
}
