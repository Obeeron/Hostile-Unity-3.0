using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmInterraction : MonoBehaviourPunCallbacks
{
    [SerializeField] private float rad;

    public virtual void Interact()
    {
        
    }
    
    //permet de voir la range d'interaction
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rad);
    }
}
