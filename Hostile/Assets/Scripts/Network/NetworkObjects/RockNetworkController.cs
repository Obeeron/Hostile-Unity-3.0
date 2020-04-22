using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RockNetworkController : NetworkObjectManager
{
    #region Singleton 
    //on crée une unique instance accessible de partout
    public static RockNetworkController instance;
    private int localID;
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
        localID = ID;
        pv.RPC("DestroyFarmingItem_RPC", RpcTarget.AllViaServer, ID);
    }

    [PunRPC]
    public void DestroyFarmingItem_RPC(int ID)
    {
        bool spawn = localID == ID;
        FarmingItem rock = (FarmingItem)GetNetworkObject(ID);
        rock?.DestroyFarmingItem(spawn);
    }

    public void SoundRock(int ID)
    {
        pv.RPC("SoundRock_Local", RpcTarget.AllViaServer, ID);
    }

    [PunRPC]
    public void SoundRock_Local(int ID)
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
