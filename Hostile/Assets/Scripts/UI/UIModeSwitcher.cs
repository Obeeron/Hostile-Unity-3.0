using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModeSwitcher : MonoBehaviour
{
    MonoBehaviour movementScript;
    MonoBehaviour cameraScript;

    void Start()
    {
        movementScript = GetComponent<Joueur.Player_Movement>();
        cameraScript = GetComponent<Joueur.Camera_Movement>();
    }

    public void UIModeSwitchState(bool state)
    {
        if (movementScript != null)
        {
            Debug.Log("movementScript not empty : "+state);
        }
        else
        {
            Debug.Log("movementScript is empty : "+state);
        }
        movementScript.enabled = !state;
        cameraScript.enabled = !state;
    }
}
