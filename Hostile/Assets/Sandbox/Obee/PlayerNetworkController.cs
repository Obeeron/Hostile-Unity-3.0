using UnityEngine;
using Photon.Pun;

public class PlayerNetworkController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] MonoBehaviour[] ignoredScripts;
#pragma warning restore 649
    PhotonView phView;

    void Start()
    {
        phView = GetComponent<PhotonView>();
        if (!phView.IsMine)
        {
            foreach (var script in ignoredScripts)
                script.enabled = false;
            GetComponentInChildren<Camera>().enabled = false;
        }
    }
}
