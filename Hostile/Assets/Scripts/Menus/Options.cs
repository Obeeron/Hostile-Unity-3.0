using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject controllsPannel;
#pragma warning restore 649

    public void OnBackBtnClick()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OnControllsBtnClick()
    {
        controllsPannel.SetActive(true);
        mainPanel.SetActive(false);
    }
}
