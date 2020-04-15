using UnityEngine;
using Photon.Pun;

public class PlayerNetworkController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] MonoBehaviour[] ignoredScripts;
#pragma warning restore 649
    public GameObject Arms;
    public GameObject Root;
    public GameObject Armature;
    PhotonView phView;

    void Start()
    {
        phView = GetComponent<PhotonView>();
        if (!phView.IsMine)
        {
            foreach (var script in ignoredScripts)
                script.enabled = false;
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            Arms.SetActive(false);
        }
        else
            Root.SetActive(false);
    }
}
