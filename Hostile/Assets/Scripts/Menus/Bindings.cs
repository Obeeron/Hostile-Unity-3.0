using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Bindings : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject controllsPannel;
#pragma warning restore 649

    #pragma warning disable 649
    [SerializeField] private PlayerControls binds;
    #pragma warning restore 649
    private Text Uptext;
    private Text DownText;
    private Text LefttText;
    private Text RightText;
    private Text JumpText;
    private Text RunSText;
    private Text CrouchSText;

    public void Start()
    {
        binds = new PlayerControls();
        if (controllsPannel != null)
        {
            Uptext = controllsPannel.transform.Find("UpBtn").Find("Uptxt").GetComponent<Text>();
            DownText = controllsPannel.transform.Find("DownBtn").Find("Downtxt").GetComponent<Text>();
            LefttText = controllsPannel.transform.Find("LeftBtn").Find("Lefttxt").GetComponent<Text>();
            RightText = controllsPannel.transform.Find("RightBtn").Find("Righttxt").GetComponent<Text>();
            JumpText = controllsPannel.transform.Find("JumpBtn").Find("Jumptxt").GetComponent<Text>();
            RunSText = controllsPannel.transform.Find("RunSBtn").Find("RunStxt").GetComponent<Text>();
            CrouchSText = controllsPannel.transform.Find("CrouchSBtn").Find("CrouchStxt").GetComponent<Text>();
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

    public void RebindJump()
    {
        RemapButtonClicked(binds.InGame.Jump);
        JumpText.text = "" + InputControlPath.ToHumanReadableString(binds.InGame.Jump.bindings[0].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
    }
    
    public void RemapButtonClicked(InputAction actionToRebind)
    {
        var rebindOperation = actionToRebind
            .PerformInteractiveRebinding()
            .WithCancelingThrough("<Keyboard>/#(escape)")
            .WithControlsExcluding("Mouse")
            .Start();
    }

    public void OnBackBtnClick()
    {
        optionsPanel.SetActive(true);
        controllsPannel.SetActive(false);
    }
}
