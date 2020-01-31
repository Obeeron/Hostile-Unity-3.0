using UnityEngine;
using Photon.Pun;
using System;

public class GameSetupManager : MonoBehaviourPunCallbacks
{
#pragma warning disable 649
    [SerializeField] Vector3 spawnPoint;
#pragma warning restore 649
    private PhotonView phView;

    public void Start()
    {
        phView = GetComponent<PhotonView>();
        Debug.Log("Room Joined");
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate("NetworkSimplePlayer", spawnPoint, Quaternion.identity);
        Debug.Log("Avatar created.");
    }
}
