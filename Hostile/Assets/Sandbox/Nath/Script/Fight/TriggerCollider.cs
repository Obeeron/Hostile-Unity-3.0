using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class TriggerCollider : MonoBehaviour, IOnEventCallback
{
    [SerializeField]
    private PlayerData playerData;

    private Vector3 impactForce;
    private Collider ennemy;

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterController main = this.GetComponentInParent<CharacterController>();
        PhotonView PV = GetComponentInParent<PhotonView>();
        if (other != main) // On vérifie qu'on ne se tappe pas soi-même
        {
            ennemy = other;
            if(ennemy.GetComponentInParent<CharacterController>() != null)
            {
                int pv = other.GetComponentInParent<PhotonView>().ViewID;
                float strenght = playerData.Strength;
                PV.RPC("GetHit", RpcTarget.All, strenght, pv);
                Hit((int)playerData.Strength, other);
            }
            else
            {
                
                if (ennemy.GetComponent<FarmingItem>() != null)   
                {
                    if(ennemy.GetComponent<CapsuleCollider>() != null)
                    {
                        //Debug.Log("not a player");
                        // On envoit un Event
                        byte eventCode = 2;
                        object[] content = new object[] { playerData.Strength};
                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                        SendOptions send = new SendOptions { Reliability = true };
                        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, send);
                    }
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

    

  

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        Debug.Log("ON EVENT");
        if(eventCode == 2)
        {
            object[] data = (object[])photonEvent.CustomData;
            float dmg = (float)data[0];
            ennemy.GetComponent<FarmingItem>().GetHit(dmg);
        }
    }
}
