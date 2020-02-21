using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.EventSystems;

public class TriggerCollider : MonoBehaviour
{
    //public UnityEvent onHit;
    [SerializeField]
    private PlayerData playerData;
    private void OnTriggerEnter(Collider other)
    {
        CharacterController main = this.GetComponentInParent<CharacterController>();
        if (other != main) // On vérifie qu'on ne se tappe pas soi-même
        {
            //PointerEventData pointerData = data as PointerEventData;
            PhotonView PV = PhotonView.Get(this);
            PV.RPC("GetHit", RpcTarget.MasterClient, playerData.Strength,other);
            Debug.Log("Touched");
            /*if (other.GetComponent<Entity>() != null)
            {
                Entity player = GetComponent<Entity>();
                Entity ennemy = other.gameObject.GetComponent<Entity>();
                ennemy.getHit(player.Damage); //getHit() -->
            }*/
        }
    }

    [PunRPC]
    void GetHit(int dmg,Collider other)
    {
        Debug.Log("ur getting hit");
    }
}
