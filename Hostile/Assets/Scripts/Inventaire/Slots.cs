using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Slots : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Item item;
    public bool isEmpty;
    public GameObject icone;
    public GameObject draggableItem;

    public Sprite slotSelected;
    public Sprite slotUnselected;
    public bool isSelected;

    
    public void Selected(bool isSelected)
    {
        this.isSelected = isSelected;
        GetComponent<Image>().sprite = (isSelected) ? slotSelected : slotUnselected;
    }

    public void Add(Item itemRecu)
    {
        if (itemRecu == null) return;
        isEmpty = false;
        item = itemRecu;
        icone.GetComponent<Image>().sprite = item.itemData.icone;
        icone.gameObject.SetActive(true);
    }
   

    public void Reset2()
    {
        //Debug.Log("item " + item.name + " droppé");
        isEmpty = true;
        item = null;
        icone.GetComponent<Image>().sprite = null;
        icone.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Inventaire.instance.hoveredSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Inventaire.instance.hoveredSlot = null;
    }

   
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isEmpty)
        { 
            draggableItem.GetComponent<Image>().sprite = icone.GetComponent<Image>().sprite;
            draggableItem.GetComponent<Transform>().position = icone.transform.position;
            draggableItem.SetActive(true);
            icone.SetActive(false);
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isEmpty)
            draggableItem.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!isEmpty && Inventaire.instance.hoveredSlot!=null)
        {
            draggableItem.SetActive(false);
            Item itemInt = item;
            this.Reset2();
            this.Add(Inventaire.instance.hoveredSlot.item);
            Inventaire.instance.hoveredSlot.Reset2();
            Inventaire.instance.hoveredSlot.Add(itemInt);

        }
        if(!isEmpty)
        {
            icone.SetActive(true);
            draggableItem.SetActive(false);
        }  
    }
}