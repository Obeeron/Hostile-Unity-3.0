using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSystem : Player
{
    public Animator animator;

    IEnumerator DisableCollider(Collider box)
    {
        yield return new WaitForSeconds(1);
        box.isTrigger = false;
    }

    void Update()
    {

        if (!isAlive)
        {
            Debug.Log("DE4D");
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.Play("pickaxe_hit");
            Collider box;
            if(GetComponentInChildren<BoxCollider>() != null)
            {
                box = GetComponentInChildren<BoxCollider>();

                Debug.Log(box);
                box.isTrigger = true; // Active le trigger de l'arme 
                // attendre 2sec;
                StartCoroutine(DisableCollider(box)); // Redesactive le trigger de l'arme
                
                
            }
            
        }
        
    }
   
}
