using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RockNetworkController : NetworkObjectManager
{
    #region Singleton 
    //on crée une unique instance accessible de partout
    public static RockNetworkController instance;

    //appelé avant le start
    protected override void Awake ()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogWarning("erreur, il y a deja une instance RockNetworkController");
            return;
        }
        instance = this;
    }
    #endregion

    public void DestroyFarmingItem(int ID)
    {
        pv.RPC("DestroyFarmingItem_RPC", RpcTarget.AllViaServer, ID);
    }

    [PunRPC]
    public void DestroyFarmingItem_RPC(int ID)
    {
        FarmingItem rock = (FarmingItem)GetNetworkObject(ID);
        rock?.DestroyFarmingItem();
    }
}
