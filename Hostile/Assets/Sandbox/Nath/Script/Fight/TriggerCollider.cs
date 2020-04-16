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
#pragma warning disable 649
    [SerializeField]
    private PlayerData playerData;
#pragma warning restore 649


    private Vector3 impactForce;
    private PhotonView PV;
    private GameObject ennemy;
    private System.Random rd = new System.Random();

    private void Start()
    {
        PV = GetComponentInParent<PhotonView>();
    }
    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void Raycast_hit(RaycastHit hit)
    {
        //Debug.Log("Fired !");
        GameObject other = hit.collider.gameObject;
        GameObject main = GetComponentInParent<CharacterController>().gameObject;
        Debug.Log(other.name);
        if (other != main) // On vérifie qu'on ne se tappe pas soi-même
        {
            ennemy = other;
            if(ennemy.GetComponentInParent<CharacterController>() != null)
            {
                int pv = other.GetComponentInParent<PhotonView>().ViewID; // on récupère l'id de ce que l'on a touché
                float strenght = playerData.Strength;

                //On prépare l'event

                byte eventCode = 3;
                object[] content = new object[] {playerData.Damage * playerData.Strength , pv};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                SendOptions send = new SendOptions { Reliability = true };

                PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, send);
                //PV.RPC("GetHit", RpcTarget.All, strenght, pv);
                //Hit((int)playerData.Strength, other);
            }
            else
            {  
                if (ennemy.GetComponent<FarmingItem>() != null)   
                {       
                    if(ennemy.GetComponent<Collider>() != null)
                    {
                        //Debug.Log("not a player");
                        //Debug.Log("hit a farmable item");
                        FarmingItem fItem = ennemy.GetComponent<FarmingItem>();
                        int j = fItem.sounds.Length;
                        float strength = 0;
                        if (fItem.type == FarmingItem.Type.Tree)
                        {
                            strength = playerData.ChoppingStrength;
                        }
                        else if (fItem.type == FarmingItem.Type.Stone)
                            strength = playerData.MiningStrength;
                        else
                        {
                            Debug.Log("Hit a farming item with a wrong type");
                        }
                        int index = rd.Next(0, j);
                        // On envoit un Event
                        byte eventCode = 2;
                        object[] content = new object[] {strength, index};
                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                        SendOptions send = new SendOptions { Reliability = true };
                        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, send);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (ennemy)
        {
            if(impactForce.magnitude > 0.2)
            {
                //Fonctionne en local seulement
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
        if(eventCode == 2)
        {
            //Debug.Log("hit a tree or a rocks");
            object[] data = (object[])photonEvent.CustomData;
            float dmg = (float)data[0];
            int i = (int)data[1];
            if(ennemy != null)
            {
                FarmingItem farm = ennemy.GetComponent<FarmingItem>();
                if (farm != null)
                {
                    AudioSource listener = ennemy.GetComponent<AudioSource>();
                    if (farm.life == 1)
                    {
                        listener.PlayOneShot(farm.sounds[i]);
                        listener.PlayOneShot(farm.destroyed);
                    }
                    else
                    {
                        listener.PlayOneShot(farm.sounds[i]);
                    }

                    //Debug.Log("Farming item");
                    farm.GetHit(dmg);

                    Debug.Log(ennemy.GetComponent<FarmingItem>().life);
                }
                   
            }
            
        }

        if(eventCode == 3)
        {
            object[] data = (object[])photonEvent.CustomData;
            float dmg = (float)data[0];
            int pv = (int)data[1];
            //Debug.Log("My pv " + PV.ViewID + "Ennemy pv is " + pv);
            if(PV.ViewID == pv)
            {
                GameObject g = GameObject.Find("StatsController");
                g.GetComponent<Joueur.StatsController>().getHit(dmg);
                //Debug.Log("ur getting hit");
            }
        }
    }
}
