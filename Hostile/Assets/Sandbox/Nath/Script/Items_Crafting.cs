using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Items_Crafting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Scritable_Items item; // Public pour voir les changements ou pour debug
    public void Update()
    {
        item = GetComponent<Items_in_UI>().item; // On récupère l'item que l'on stock au préalable dans Item_in_UI
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        item.OnEnter(); // On affiche les items needed dans le panel associé
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // On clear la panel items needed
        Transform panel = GameObject.Find("Needed").transform;
        foreach(Transform child in panel)
        {
            Sprite sp = Resources.Load<Sprite>("Sprites/inventorySlot");
            if (sp != null)
            {
                child.gameObject.GetComponent<Image>().sprite = sp;
                
            }
            if(child.childCount == 1)
            {
                child.GetChild(0).gameObject.SetActive(false);
            }

        }

    }
}
