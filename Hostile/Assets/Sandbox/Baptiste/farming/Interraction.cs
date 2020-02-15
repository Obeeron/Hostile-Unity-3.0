using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Interraction : MonoBehaviourPunCallbacks
{
    public float rad = 5f;

    
    //methode qui doit etre overwritten car différente si c'est un item, ennemi, coffre
    public virtual void Interact()
    {
        
    }
    
    //fonction de unity qui permet de "dessiner", ici on veut dessiner un cercle de distance distance autour de l'objet interactable
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rad);
    }
}
