using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSystem : Player
{
    public Animator animator;
    private bool enabled;
    private bool canTouch;

    IEnumerator DisableCollider(Collider box)
    {
        yield return new WaitForSeconds(1.2f);
        box.isTrigger = false;
        animator.SetLayerWeight(1, 0);
        enabled = true;
        canTouch = true;
    }

    private void Start()
    {
        enabled = true;
        canTouch = true;
    }

    void Update()
    {

        if (!isAlive)
        {
            Debug.Log("DE4D");
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) && enabled)
        {
            enabled = false;
            animator.SetLayerWeight(1, 1);
            animator.Play("Sword_Right",1);
            Collider box;
            if(GetComponentInChildren<BoxCollider>() != null)
            {
                box = GetComponentInChildren<BoxCollider>();
                if (canTouch)
                {
                    box.isTrigger = true; // Active le trigger de l'arme 
                    canTouch = false;
                }
                
                // attendre 2sec;
                StartCoroutine(DisableCollider(box)); // Redesactive le trigger de l'arme
       
            }

        }
        
    }
   
}
