using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Items_Crafting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Item item; // Public pour voir les changements ou pour debug

    public void Start()
    {
        Button button = transform.GetComponent<Button>();
        button.onClick.AddListener(this.OnClick);
    }
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
        item.OnExit();
    }
    public void OnClick()
    {
        item.OnClick();
    }
}
