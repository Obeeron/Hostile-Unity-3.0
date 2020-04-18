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
    protected override void Awake()
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

    public void DestroyFarmingItem(int ID)
    {
        pv.RPC("DestroyFarmingItem_RPC", RpcTarget.AllViaServer, ID);
    }

    [PunRPC]
    public void DestroyFarmingItem_RPC(int ID)
    {
        FarmingItem tree = (FarmingItem)GetNetworkObject(ID);
        tree?.DestroyFarmingItem();
    }

    public void SoundTree(int ID)
    {
        pv.RPC("SoundTree_Local", RpcTarget.AllViaServer, ID);
    }

    [PunRPC]
    public void SoundTree_Local(int ID)
    {
        NetworkObject localObj = GetNetworkObject(ID);
        FarmingItem farm = localObj.GetComponent<FarmingItem>();
        AudioSource listener = localObj.GetComponent<AudioSource>();
        int i = Random.Range(0, farm.sounds.Length);
        if (farm.life == 0)
        {
            listener.PlayOneShot(farm.sounds[i]);
            listener.PlayOneShot(farm.destroyed);
        }
        else
        {
            listener.PlayOneShot(farm.sounds[i]);
        }
    }
}