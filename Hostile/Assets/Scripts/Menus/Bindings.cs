using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bindings : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject controllsPannel;
#pragma warning restore 649

    void RemapButtonClicked(InputAction actionToRebind)
    {
        var rebindOperation = actionToRebind
            .PerformInteractiveRebinding().Start();
    }

    public void OnBackBtnClick()
    {
        optionsPanel.SetActive(true);
        controllsPannel.SetActive(false);
    }
}
