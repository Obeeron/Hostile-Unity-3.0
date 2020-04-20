using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkItemsController : NetworkObjectManager
{
    #region Singleton 
    //on crée une unique instance accessible de partout
    public static NetworkItemsController instance;
    //appelé avant le start
    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogWarning("erreur, il y a deja une instance NetworkItemsController");
            return;
        }
        instance = this;
    }
    #endregion

    public GameSetupManager gameSetupManager;

    public enum prefabID
    {
        LOG,
        PLANK,
        STONE,
        CHEST,
        PICKAXE,
        SPEAR,
        AXE
    };

    public void SynchronizeItem(int ID, bool state, Vector3 position, bool refreshPos = true)
    {
        pv.RPC("SynchronizeItem_RPC", RpcTarget.AllViaServer, ID, state, position, refreshPos);
    }

    public void SynchronizeItemEquip(int ID, bool state, Vector3 position, Quaternion rotation, int id)
    {
        pv.RPC("SynchronizeItemEquip_RPC", RpcTarget.AllViaServer, ID, state, position, rotation, id);
    }

    [PunRPC]
    public void SynchronizeItem_RPC(int ID, bool state, Vector3 position, bool refreshPos = true)
    {
        Item item = (Item)GetNetworkObject(ID);
        item.inInventory = !state;
        if (state && refreshPos)
            item.gameObject.transform.position = position;
        item.gameObject.SetActive(state);
    }


    [PunRPC]
    public void SynchronizeItemEquip_RPC(int ID, bool state, Vector3 position, Quaternion rotation, int parentID)
    {
        Item item = (Item)GetNetworkObject(ID);
        item.inInventory = !state;
        if (state)
        {

            foreach (GameObject player in gameSetupManager.players)
            {
                if(player.GetComponent<PhotonView>().ViewID == parentID)
                {
                    item.transform.parent = player.GetComponentInChildren<Arms_Transform>().Arms_Network;
                    item.gameObject.transform.localPosition = position;
                    item.gameObject.transform.localRotation = rotation;
                }

            }
        }
            
        item.gameObject.SetActive(state);
    }
}