using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Bindings : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject controllsPannel;
#pragma warning restore 649

    #pragma warning disable 649
    [SerializeField] private PlayerControls binds;
    #pragma warning restore 649
    private TextMeshProUGUI Uptext;
    private TextMeshProUGUI DownText;
    private TextMeshProUGUI LefttText;
    private TextMeshProUGUI RightText;
    private TextMeshProUGUI JumpText;
    private TextMeshProUGUI RunSText;
    private TextMeshProUGUI CrouchSText;

    public void Start()
    {
        if (binds == null)
            ResestControls();
        if (controllsPannel != null)
        {
            Uptext = controllsPannel.transform.Find("UpBtn").Find("Text").GetComponent<TextMeshProUGUI>();
            DownText = controllsPannel.transform.Find("DownBtn").Find("Text").GetComponent<TextMeshProUGUI>();
            LefttText = controllsPannel.transform.Find("LeftBtn").Find("Text").GetComponent<TextMeshProUGUI>();
            RightText = controllsPannel.transform.Find("RightBtn").Find("Text").GetComponent<TextMeshProUGUI>();
            JumpText = controllsPannel.transform.Find("JumpBtn").Find("Text").GetComponent<TextMeshProUGUI>();
            RunSText = controllsPannel.transform.Find("RunSBtn").Find("Text").GetComponent<TextMeshProUGUI>();
            CrouchSText = controllsPannel.transform.Find("CrouchSBtn").Find("Text").GetComponent<TextMeshProUGUI>();
            //Printings Bindings//
            Uptext.text = "" + InputControlPath.ToHumanReadableString(binds.InGame.Movement.bindings[1].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
            DownText.text = "" + InputControlPath.ToHumanReadableString(binds.InGame.Movement.bindings[2].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
            LefttText.text = "" + InputControlPath.ToHumanReadableString(binds.InGame.Movement.bindings[3].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
            RightText.text = "" + InputControlPath.ToHumanReadableString(binds.InGame.Movement.bindings[4].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
            JumpText.text = "" + InputControlPath.ToHumanReadableString(binds.InGame.Jump.bindings[0].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
            RunSText.text = "" + InputControlPath.ToHumanReadableString(binds.InGame.SpeedSwap.bindings[0].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
            CrouchSText.text = "" + InputControlPath.ToHumanReadableString(binds.InGame.Crouch.bindings[0].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
        }
        else 
        {
            this.enabled = false;
        }
    }

    public void ResestControls()
    {
        binds = new PlayerControls();
    }

    public void RebindJump()
    {
        RemapButtonClicked(binds.InGame.Jump);
    }
    
    public void RemapButtonClicked(InputAction actionToRebind)
    {
        var rebindOperation = actionToRebind
            .PerformInteractiveRebinding()
            .WithCancelingThrough("<Keyboard>/escape")
            .WithControlsExcluding("Mouse")
            .WithTimeout(2f);

        rebindOperation.Start();
        JumpText.text = "" + InputControlPath.ToHumanReadableString(binds.InGame.Jump.bindings[0].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
    
    }

    public void OnBackBtnClick()
    {
        optionsPanel.SetActive(true);
        controllsPannel.SetActive(false);
    }
}
