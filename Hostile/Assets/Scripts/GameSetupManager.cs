using UnityEngine;
using Photon.Pun;
using System;
using Joueur;

public class GameSetupManager : MonoBehaviourPunCallbacks
{
    public UI.UI_Controller_Game uiController;
    public StatsController statsController;
    public Vector3 spawnPoint;

    void Start()
    {
        Debug.Log("Room Joined");
        SetupCamera();
        CreatePlayer();
    }

    private void SetupCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CreatePlayer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject player = PhotonNetwork.Instantiate("NetworkSimplePlayer", spawnPoint, Quaternion.identity);
        Debug.Log("Avatar created");
        
        //UIModeSwitch Events
        UIModeSwitcher uiSwitcher = player.GetComponent<UIModeSwitcher>();
        uiController.OnUIModeEnable.AddListener(delegate { uiSwitcher.UIModeSwitchState(true); });
        uiController.OnUIModeDisable.AddListener(delegate { uiSwitcher.UIModeSwitchState(false); });

        //UI's player reference
        Inventaire.instance.player = player;
            
        //player movement event setup
        player.GetComponent<Player_Movement>().onJump.AddListener(delegate { statsController.looseStamina(10f); });
    }
}
