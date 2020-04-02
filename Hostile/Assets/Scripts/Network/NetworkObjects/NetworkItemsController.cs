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
        STONE
    };

    public void SynchronizeItem(int ID, bool state, Vector3 position)
    {
        pv.RPC("SynchronizeItem_RPC", RpcTarget.AllViaServer, ID, state, position);
    }

    [PunRPC]
    public void SynchronizeItem_RPC(int ID, bool state, Vector3 position)
    {
        Item item = (Item)GetNetworkObject(ID);
        item.inInventory = !state;
        if (state)
            item.gameObject.transform.position = position;
        item.gameObject.SetActive(state);
    }
}