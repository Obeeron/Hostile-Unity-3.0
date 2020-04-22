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
    #pragma warning disable 649
    [SerializeField] private List<RebindOperation> resetList;
    [SerializeField] private InputActionReference action;
    [SerializeField] private GameObject currentPannel;
    [SerializeField] private GameObject otherPannel;
    #pragma warning restore 649

    public void ResetAll()
    {
        foreach (RebindOperation rb in resetList)
        {
            rb.reset();
        }
    }

    public void change()
    {
        currentPannel.SetActive(false);
        otherPannel.SetActive(true);
    }
}
