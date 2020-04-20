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
    public Animator animatorArms;
    private Camera cam;
    public AudioClip[] sounds;

    public LayerMask layerMask;

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetLayerWeight(1, 0);
        usable = true;
        canTouch = true;
    }

    IEnumerator PlaySound(float t, int i)
    {
        yield return new WaitForSeconds(t);

    }

    IEnumerator FireRaycast()
    {
        yield return new WaitForSeconds(0.6f);
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out hit,5,layerMask))
        {
            if (hit.collider != null)
            {
                //Debug.Log("triggerCollider searched");
                TriggerCollider tc = GetComponentInChildren<TriggerCollider>();
                if (tc != null)
                {

                    //Debug.Log(tc.gameObject.name);
                    tc.Raycast_hit(hit);
                }

            }
        }
    }

    private void Start()
    {
        usable = true;
        canTouch = true;
        PV = PhotonView.Get(this);
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //animatorArms.Play("Pickaxe_hit", 0);
            //var anim = this.GetComponent<Animations_State>();
            //Debug.Log("Changed");
            //anim.ChangeEquiped();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && usable /* && PV.IsMine*/)
        {
            usable = false;
            animator.SetLayerWeight(1, 1);
            animator.Play("Melee", 1);
            animatorArms.Play("Farm_Hit",0);
            PV.RPC("playAnimation", RpcTarget.Others, "Melee");
            StartCoroutine(FireRaycast());
            StartCoroutine(DisableCollider()); // Redesactive le trigger de l'arme
            //



        }
        
    }

    void updateCollider(Collider box)
    {
        if (canTouch)
        {
            Debug.Log(box.name);
            box.isTrigger = true; // Active le trigger de l'arme 
            canTouch = false;
        }
    }

    [PunRPC]
    void playAnimation(string animation)
    {
        //Debug.Log("PLaying " + animation);
        animator.Play(animation);
    }


    /*[PunRPC]
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

    */
}
