using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public Button Weapons;
    public Button Tools;
   public void OnClickWeapons()
    {
        this.GetComponent<Button>().interactable = false;
       // SlotTools.enabled = true;
       // SlotWeapons.enabled = false;
        Tools.interactable = true;

    }

    public void OnClickTools()
    {
        this.GetComponent<Button>().interactable = false;
        // SlotTools.enabled = false;
        // SlotWeapons.enabled = true;
        Weapons.interactable = true;
    }
}
