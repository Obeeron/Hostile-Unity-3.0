using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations_State : MonoBehaviour
{
    private bool equiped;
    public Animator animator; 
    // Start is called before the first frame update
    void Start()
    {
        equiped = false;
    }

    public bool GetState()
    {
        return equiped;
    }

    public void ChangeEquiped()
    {
        equiped = !equiped;
        Debug.Log(equiped);
        Debug.Log(animator.GetLayerName(2));
        Debug.Log(animator.name);
        if (equiped)
        {
            animator.SetBool("Equiped", true);
            //animator.SetLayerWeight(animator.GetLayerIndex("Equiped"), 1);
            //animator.runtimeAnimatorController = (RuntimeAnimatorController) Resources.Load("MainAnimator_Equiped");
        }
        else
        {
            animator.SetBool("Equiped", false);
            //animator.SetLayerWeight(2, 0);
            //animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("MainAnimator");
        }
    }
}
