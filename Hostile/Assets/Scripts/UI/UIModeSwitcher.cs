using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModeSwitcher : MonoBehaviour
{
    MonoBehaviour cameraScript;

    void Start()
    {
        cameraScript = GetComponent<Joueur.Camera_Movement>();
    }

    public void UIModeSwitchState(bool state)
    {
        cameraScript.enabled = !state;
    }
}
