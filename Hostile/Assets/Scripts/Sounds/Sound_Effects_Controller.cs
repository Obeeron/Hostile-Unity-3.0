using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Effects_Controller : MonoBehaviour
{
    #region instance
    public static Sound_Effects_Controller instance;
    protected void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("erreur, il y a deja une instance Sound_Effects_Controller");
            return;
        }
        instance = this;
    }

    #endregion
    public AudioClip[] Farm_Tree;
    public AudioClip Tree_Destroyed;
    public AudioClip[] Farm_Rock;
    public AudioClip Rock_Destroyed;
    public AudioClip[] Footsteps;
}
