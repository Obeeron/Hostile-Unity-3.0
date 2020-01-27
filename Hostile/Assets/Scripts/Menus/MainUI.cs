using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainUI : MonoBehaviourPunCallbacks
{
#pragma warning disable 649
    [SerializeField] private Button playBtn;
    [SerializeField] private Button cancelBtn;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject joiningGamePanel;
#pragma warning restore 649

    private LobbyController lobbyController;

    // Start is called before the first frame update
    void Start()
    {
        lobbyController = GetComponent<LobbyController>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Now connected to " + PhotonNetwork.CloudRegion + " server.");
        playBtn.interactable = true;
    }
    public void OnPlayBtnClick()
    {
        joiningGamePanel.SetActive(true);
        mainPanel.SetActive(false);

        lobbyController.JoinGame();
    }

    public void OnSettingsBtnClick()
    {
        optionsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void OnQuitBtnClick()
    {
        Debug.Log("Closing application");
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    public void OnCancelBtnClick()
    {
        PhotonNetwork.LeaveRoom();
        mainPanel.SetActive(true);
        joiningGamePanel.SetActive(false);
    }
}
