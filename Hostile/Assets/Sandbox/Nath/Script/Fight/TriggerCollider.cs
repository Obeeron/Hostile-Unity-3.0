using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class TriggerCollider : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    private Vector3 impactForce;
    private Collider ennemy;
    private void OnTriggerEnter(Collider other)
    {
        CharacterController main = this.GetComponentInParent<CharacterController>();
        PhotonView PV = GetComponentInParent<PhotonView>();
        if (other != main) // On vérifie qu'on ne se tappe pas soi-même
        {
            ennemy = other;
            if(ennemy.GetComponentInParent<CharacterController>() != null)
            {
                Debug.Log(ennemy.name);
                Debug.Log(playerData.Strength);
                int pv = other.GetComponentInParent<PhotonView>().ViewID;
                Debug.Log("MY PV :" + PV + " other pv : " + pv);
                //byte codeHit = 2;
                //PhotonNetwork.RaiseEvent(codeHit, new object[] { playerData.Strength, other, pv }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
                float strenght = playerData.Strength;
                PV.RPC("GetHit", RpcTarget.All, strenght, pv);
                //Hit((int)playerData.Strength, other);
            }
            else
            {
                Debug.Log("not a player");
                if(ennemy.GetComponent<FarmingItem>().GetComponent<CapsuleCollider>() != null)
                {
                    PV.RPC("GetHitFarm", RpcTarget.AllBuffered, playerData.Strength, other);
                }
            }
            
            Debug.Log("Touched");
            main.GetComponentInChildren<BoxCollider>().isTrigger = false;
        }
    }

    private void Update()
    {
        if (ennemy)
        {
            if(impactForce.magnitude > 0.2)
            {
                ennemy.GetComponentInParent<CharacterController>().Move(impactForce * Time.deltaTime);
                impactForce = Vector3.Lerp(impactForce, Vector3.zero, 5 * Time.deltaTime);
            }

        }
    }

    //Local function for test-purposes
    void Hit(int dmg,Collider other)
    {
        Vector3 vect = other.transform.position - this.transform.position;
        vect.y = 0.5f;
        int ImpactStrength = 50;
        impactForce = AddForceOnHit(vect, ImpactStrength);

        //si il y a un rigidbody
        //other.GetComponent<Rigidbody>().AddForce(vect * Strength,ForceMode.Impulse);
    }

    Vector3 AddForceOnHit(Vector3 vect, float strenght)
    {
        vect.Normalize();
        return vect.normalized * strenght / 1;
    }
    Vector3 AddExplosionForceOnHit(Vector3 vect, float strenght)
    {
        vect.Normalize();
        return vect.normalized * strenght / 1;
    }

    

  

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        if(eventcode == 2)
        {
            Debug.Log("test");
        }
    }
}
