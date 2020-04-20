using UnityEngine;
using Photon.Pun;
using System;
using Joueur;
using System.Collections.Generic;

public class GameSetupManager : MonoBehaviourPunCallbacks
{
    public UI.UI_Controller_Game uiController;
    public GameObject loadingScreen;
    public StatsController statsController;
    public PlayerDeath pldeath;
    public DeathDrop deathdrop;
    public Procedural.WorldGenerator worldGenerator;
    public Terrain terrain;
    public List<GameObject> players;

    void Start()
    {
        int seed;
        if (!Int32.TryParse(PhotonNetwork.CurrentRoom.Name.Substring(5), out seed))
            seed = 0;
        worldGenerator.StartCoroutine("GenerateWorld",seed);
    }

    public void AfterWorldGeneration(){ // à la rechercher d'un bon nom de fonction 
        SetupCamera();
        CreatePlayer();
        loadingScreen.SetActive(false);
        EnablePlayerScripts();
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

        GameObject player = PhotonNetwork.Instantiate("NetworkPlayer", GetSpawnPoint(), Quaternion.identity);
        players.Add(player);
        Debug.Log("Avatar created");
        
        //UIModeSwitch Events
        UIModeSwitcher uiSwitcher = player.GetComponent<UIModeSwitcher>();
        uiController.OnUIModeEnable.AddListener(delegate { uiSwitcher.UIModeSwitchState(true); });
        uiController.OnUIModeDisable.AddListener(delegate { uiSwitcher.UIModeSwitchState(false); });

        //UI's player reference
        Inventaire.instance.player = player;
            
        //player death event setup
        statsController.OnDeath.AddListener(delegate { deathdrop.DropAll(player); });
        statsController.OnDeath.AddListener(delegate { pldeath.CameraDeath(player); });

        //Build camera ref
        BuildingSystem.BuildCore.instance.SetCamera(player.GetComponentInChildren<Camera>());
    }

    private void EnablePlayerScripts()
    {
        statsController.enabled = true;
    }

    private Vector3 GetSpawnPoint(){
        int midSize = terrain.terrainData.heightmapResolution/2;
        Vector3 spawnPoint = new Vector3(midSize,terrain.terrainData.GetHeight(midSize,midSize)+10,midSize);
        RaycastHit hit;
        if(Physics.Raycast(spawnPoint,Vector3.down,out hit)){
            return hit.point;
        }
        return spawnPoint;
    }
}
