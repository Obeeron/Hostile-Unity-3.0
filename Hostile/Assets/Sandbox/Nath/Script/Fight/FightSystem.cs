using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSystem : MonoBehaviour
{
    public Animator animator;
    private bool usable;
    private bool canTouch;

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
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && usable)
        {
            usable = false;
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
