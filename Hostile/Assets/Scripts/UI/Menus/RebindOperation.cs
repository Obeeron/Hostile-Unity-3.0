using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class RebindOperation : MonoBehaviour
{
    public TextMeshProUGUI bindText;
    [SerializeField] public InputActionReference refaction;
    [SerializeField] private PlayerData Data;
    public Button btn;
    private PlayerControls controls;
    private bool running = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!(bindText is null))
            bindText.text = "" + InputControlPath.ToHumanReadableString(refaction.action.bindings[0].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
    }

    public void rebind()
    {
        if (!running)
            StartCoroutine(RemapButtonClicked());
    }
    
    public void reset()
    {
        Data.controls = new PlayerControls();
    }

    private IEnumerator RemapButtonClicked()
    {
        running = true;
        var rebindOperation = refaction.action.PerformInteractiveRebinding()
            .WithCancelingThrough("<Keyboard>/escape")
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding(refaction.action.bindings[0].effectivePath)
            .WithTimeout(2f);

        rebindOperation.Start();
        Debug.Log("started");
        while (!rebindOperation.canceled && !rebindOperation.completed)
        {
            yield return null;
        }
        Debug.Log("Stopped");
        bindText.text = "" + InputControlPath.ToHumanReadableString(refaction.action.bindings[0].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
        running = false;
    }
}
