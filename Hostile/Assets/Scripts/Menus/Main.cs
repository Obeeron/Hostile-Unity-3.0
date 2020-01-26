using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Main : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playBtn;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to master server..");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Now connected to " + PhotonNetwork.CloudRegion + " server.");
        playBtn.SetActive(true);
    }
    public void OnPlayBtnClick()
    {
        
    }

    public void OnOptionsBtnClick()
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
}
