using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    [PunRPC]
    public void SynchronizeItem_RPC(int ID, bool state, Vector3 position, bool refreshPos)
    {
        Item item = (Item)GetNetworkObject(ID);
        item.inInventory = !state;
        if (state && refreshPos)
            item.gameObject.transform.position = position;
        item.gameObject.SetActive(state);
    }
}