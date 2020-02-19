using UnityEngine;
using Photon.Pun;
using System;

public class GameSetupManager : MonoBehaviourPunCallbacks
{
    public UI.UI_Controller_Game uiController;
    public Vector3 spawnPoint;

    void Start()
    {
        Debug.Log("Room Joined");
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        GameObject player = PhotonNetwork.Instantiate("NetworkSimplePlayer", spawnPoint, Quaternion.identity);
        Debug.Log("Avatar created");
        if (player.GetPhotonView().IsMine)
        {
            //UIModeSwitch Events
            UIModeSwitcher uiSwitcher = player.GetComponent<UIModeSwitcher>();
            uiController.OnUIModeEnable.AddListener(delegate { uiSwitcher.UIModeSwitchState(true); });
            uiController.OnUIModeDisable.AddListener(delegate { uiSwitcher.UIModeSwitchState(false); });

            //UI's player reference
            Inventaire.instance.player = player;
        }
    }
}
