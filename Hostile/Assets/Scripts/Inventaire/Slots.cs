using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Slots : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    public List<Item> item = new List<Item>();
    public int nbItem;
    public int ID;
    public bool isEmpty;
    public GameObject icone;
    public GameObject draggableItem;

    public Sprite slotSelected;
    public Sprite slotUnselected;
    public bool isSelected;
    public GameObject txt;


    public void Selected(bool isSelected)
    {
        this.isSelected = isSelected;
        GetComponent<Image>().sprite = (isSelected) ? slotSelected : slotUnselected;
    }
    public bool isSameItem(Item itemRecu)
    {
        return itemRecu.itemData.name == item[0].itemData.name;
    }
    public bool isRestePlace(Item itemRecu)
    {
        return itemRecu.itemData.maxSizeStack > nbItem;
    }
    public bool isAddable(Item itemRecu)
    {
        if (isEmpty)
            return true;
        if (isSameItem(itemRecu) && isRestePlace(itemRecu))
            return true;
        return false;
    }
    public void Add(Item itemRecu)
    {
        if (itemRecu == null) return;
        isEmpty = false;
        item.Add(itemRecu);
        if (nbItem == 0)
        {
            icone.GetComponent<Image>().sprite = itemRecu.itemData.icone;
            icone.gameObject.SetActive(true);
        }
        nbItem++;
        txt.GetComponentInChildren<Text>().text = "" + nbItem;
        txt.SetActive(true);
    }
   

    public void Reset2()
    {
        //Debug.Log("item " + item.name + " droppé");
        nbItem = 0;
        isEmpty = true;
        item = new List<Item>();
        icone.GetComponent<Image>().sprite = null;
        icone.SetActive(false);
        txt.GetComponentInChildren<Text>().text = "" + nbItem; ;
        txt.SetActive(false);
    }
    public void Suppr()
    {
        if (nbItem == 1){
            Console.WriteLine("lol");
            Reset2();
        }
        else{
            nbItem--;
            item.RemoveAt(nbItem - 1);
            txt.GetComponentInChildren<Text>().text = "" + nbItem; ;
        }
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
        if (!isEmpty && Inventaire.instance.hoveredSlot != null)
        {
            draggableItem.SetActive(false);
            List<Item> itemInt = item;
            this.Reset2();
            foreach (Item a in Inventaire.instance.hoveredSlot.item)
                this.Add(a);
            Inventaire.instance.hoveredSlot.Reset2();
            foreach (Item b in itemInt)
                Inventaire.instance.hoveredSlot.Add(b);

        }
        if (!isEmpty)
        {
            icone.SetActive(true);
            draggableItem.SetActive(false);
        }
    }
}
