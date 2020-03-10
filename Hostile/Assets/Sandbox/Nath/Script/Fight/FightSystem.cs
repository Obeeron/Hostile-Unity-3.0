using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
   
}
