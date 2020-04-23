using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class PlayerNetworkController : MonoBehaviour
{
    public GameObject Root;
    public GameObject Arms;
    public GameObject Armature;
    public GameObject Camera;
    PhotonView phView;

    void Start()
    {
        phView = GetComponent<PhotonView>();
        if (phView.IsMine)
        {
            Root.SetActive(false);
            Arms.SetActive(true);
            MonoBehaviour[] allScripts = GetComponents<MonoBehaviour>();
            foreach (var script in allScripts){
                Debug.Log(script);
                script.enabled = true;
            }
        }
        else
            Camera.SetActive(false);
    }
}
