using UnityEngine;
using Photon.Pun;
using System;

public class GameSetupManager : MonoBehaviourPunCallbacks
{
#pragma warning disable 649
    [SerializeField] Vector3 spawnPoint;
#pragma warning restore 649

    void Start()
    {
        Debug.Log("Room Joined");
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        GameObject player = PhotonNetwork.Instantiate("NetworkSimplePlayer", spawnPoint, Quaternion.identity);
        Debug.Log("Avatar created by "+PhotonNetwork.LocalPlayer.NickName);
    }
}
