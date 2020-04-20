using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class RebindOperation : MonoBehaviour
{
    [Header("Bindings")]
    [Tooltip("The Action to rebind.")]
    [SerializeField] private InputActionReference m_Action;
    [Tooltip("The index of the binding to rebind.\n(0 by default, if binding is composite, then start counting at 1 for the first composite.)")]
    [SerializeField] private int m_BindingIndex = 0;
    [Tooltip("If you want to show more text. (let 'DontIncludeInteractions' by default.)")]
    [SerializeField] private InputBinding.DisplayStringOptions m_DisplayStringOptions = InputBinding.DisplayStringOptions.DontIncludeInteractions;
    [Header("UI")]
    [Tooltip("The textmeshpro component with the binding print.")]
    [SerializeField] private TextMeshProUGUI bindText;
    [Tooltip("The pannel to show while waiting the input from the player.")]
    [SerializeField] private GameObject waitingPannel;
    private InputAction action;
    private Button btn;
    [Header("Events")]
    [Tooltip("Event invoked when the rebind starts.")]
    [SerializeField] public UnityEvent onStarted;
    [Tooltip("Event invoked when the rebind is cancelled.")]
    [SerializeField] public UnityEvent onCancelled;
    [Tooltip("Event invoked when the rebind has been completed.")]
    [SerializeField] public UnityEvent onCompleted;

    // Start is called before the first frame update
    void Start()
    {
        action = m_Action?.action;
        if (action == null || action.bindings.Count <= m_BindingIndex)
            this.enabled = false;
        if (!(bindText is null))
            bindText.text = action.GetBindingDisplayString(m_BindingIndex, m_DisplayStringOptions);
            // bindText.text = "" + InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath).Split(new string[] {"[Keyb"}, StringSplitOptions.None)[0];
        btn = this.GetComponentInParent<Button>();
        onCancelled.AddListener(delegate {
            UIUpdate(true);
            Debug.Log("canceled");
            });
        onCompleted.AddListener(delegate {
            UIUpdate(true);
            Debug.Log("completed");
            });
        onStarted.AddListener(delegate {
            UIUpdate(true);
            Debug.Log("started");
            });
    }

    public void rebind()
    {
        StartCoroutine(RemapButtonClicked());
    }

    private IEnumerator RemapButtonClicked()
    {
        var rebindOperation = action.PerformInteractiveRebinding(m_BindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding(action.bindings[m_BindingIndex].effectivePath)
            .OnCancel(
                operation =>
                {
                    onCancelled?.Invoke();
                }
            )
            .OnComplete(
                operation =>
                {
                    onCompleted?.Invoke();
                }
            );
        onStarted?.Invoke();
        rebindOperation.Start();
        Debug.Log("started");
        while (!rebindOperation.canceled && !rebindOperation.completed)
        {
            yield return null;
        }
        Debug.Log("Stopped");
    }    
    
    public void reset()
    {
        if (action.bindings[m_BindingIndex].isComposite)
        {
            for (var i = m_BindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                action.RemoveBindingOverride(i);//remove override for each composite
        }
        else
        {
            action.RemoveBindingOverride(m_BindingIndex);
        }
        UIUpdate();
    }

    private void UIUpdate(bool switchPannel = false)
    {
        bindText.text = action.GetBindingDisplayString(m_BindingIndex, m_DisplayStringOptions);
        btn.interactable = false;
        btn.interactable = true;
        if (switchPannel)
            waitingPannel.SetActive(!waitingPannel.activeSelf);
    }
}
