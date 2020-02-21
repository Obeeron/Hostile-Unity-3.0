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
            PhotonView PV = PhotonView.Get(this);
            PV.RPC("GetHit", RpcTarget.All, playerData.Strength,other);
            Debug.Log("Touched");
        }
    }

    [PunRPC]
    void GetHit(int dmg,Collider other)
    {
        other.GetComponentInParent<Joueur.StatsController>().getHit(dmg);
        Debug.Log("ur getting hit");
    }
}
