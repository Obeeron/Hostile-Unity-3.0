using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkItemsController : NetworkObjectManager
{
    #region Singleton 
    //on crée une unique instance accessible de partout
    public static NetworkItemsController instance;

    //appelé avant le start
    protected override void Awake ()
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
    
    public enum prefabID{
        LOG,
        PLANK,
        STONE
    };
}
