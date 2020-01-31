using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSystem : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.Play("pickaxe_hit");
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched");
    }
}
