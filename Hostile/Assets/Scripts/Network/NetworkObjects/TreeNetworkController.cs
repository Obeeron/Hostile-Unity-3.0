using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TreeNetworkController : NetworkObjectManager
{
    #region Singleton 
    //on crée une unique instance accessible de partout
    public static TreeNetworkController instance;

    //appelé avant le start
    protected override void Awake ()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogWarning("erreur, il y a deja une instance TreeNetworkController");
            return;
        }
        instance = this;
    }
    #endregion

    public enum prefabID{
        TREE1,
        TREE2,
        TREE3
    };

    public void Fall(int ID){
        pv.RPC("Fall_RPC",RpcTarget.AllViaServer,ID);
    }

    [PunRPC]
    public void Fall_RPC(int ID){
        FarmingItem tree = (FarmingItem)GetNetworkObject(ID);
        tree.Fall();
    }
}
