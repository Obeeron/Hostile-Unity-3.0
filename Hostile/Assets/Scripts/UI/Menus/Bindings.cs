using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Bindings : MonoBehaviour
{

    [Tooltip("list of rebindOperation fro reset all. Add every rebindOperation to this.")]
    [SerializeField] private List<RebindOperation> resetList;
    [SerializeField] private InputActionReference action;

    public void ResetAll()
    {
        foreach (RebindOperation rb in resetList)
        {
            rb.reset();
        }
    }
}
