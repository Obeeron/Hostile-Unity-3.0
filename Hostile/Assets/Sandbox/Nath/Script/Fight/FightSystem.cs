using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FightSystem : MonoBehaviour
{
    public Animator animator;
    private bool usable;
    private bool canTouch;
    private PhotonView PV;

    IEnumerator DisableCollider(Collider box)
    {
        yield return new WaitForSeconds(1.2f);
        box.isTrigger = false;
        animator.SetLayerWeight(1, 0);
        usable = true;
        canTouch = true;
    }

    private void Start()
    {
        usable = true;
        canTouch = true;
        PV = PhotonView.Get(this);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && usable && PV.IsMine)
        {
            usable = false;
            animator.SetLayerWeight(1, 1);
            animator.Play("Sword_Right",1);
            PV.RPC("playAnimation", RpcTarget.Others, "Sword_Right");
            Collider box;
            if(GetComponentInChildren<BoxCollider>() != null)
            {
                box = GetComponentInChildren<BoxCollider>();
                updateCollider(box);
                
                // attendre 2sec;
                StartCoroutine(DisableCollider(box)); // Redesactive le trigger de l'arme
            }

        }
        
    }

    void updateCollider(Collider box)
    {
        if (canTouch)
        {
            box.isTrigger = true; // Active le trigger de l'arme 
            canTouch = false;
        }
    }

    [PunRPC]
    void playAnimation(string animation)
    {
        Debug.Log("PLaying " + animation);
        animator.Play(animation);
    }


    [PunRPC]
    void GetHit(float dmg, int pv)
    {
        int PVs = PV.ViewID;
        Debug.Log("ViewID " + PV + " touché " + pv);
        if (PVs == pv)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                Debug.Log(player.GetComponent<PhotonView>().ViewID + " /// Celui touché " + pv);
                if (player.GetComponent<PhotonView>().ViewID == pv) // si c'est celui que j'ai tappé.
                {

                    GameObject[] gm = GameObject.FindGameObjectsWithTag("GameController");
                    foreach (GameObject g in gm)
                    {
                        Debug.Log(g.name);
                        if (g.name == "StatsController")
                        {
                            g.GetComponent<Joueur.StatsController>().getHit(dmg);
                            Debug.Log("ur getting hit");
                        }
                    }
                }
            }
        }
        
        
    }


}
