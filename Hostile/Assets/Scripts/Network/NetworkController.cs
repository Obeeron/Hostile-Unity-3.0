using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Events;

namespace Network
{
    public class NetworkController : MonoBehaviourPunCallbacks
    {
        #region Variables
        public UnityEvent onConnectionToMaster = new UnityEvent();
        #endregion

        #region Main Methods
        void Start()
        {
            Debug.Log("Connecting to master server..");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
        #endregion

        #region Helper Methods
        public override void OnConnectedToMaster()
        {
            Debug.Log("Now connected to " + PhotonNetwork.CloudRegion + " server.");
            onConnectionToMaster?.Invoke();
        }

        public void CloseApplication()
        {
            Debug.Log("Closing application");
            PhotonNetwork.Disconnect();
            Application.Quit();
        }
        #endregion
    }
}
