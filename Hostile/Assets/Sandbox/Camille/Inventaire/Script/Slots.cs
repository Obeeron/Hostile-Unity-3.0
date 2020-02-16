using UnityEngine.UI;
using UnityEngine;

public class Slots : MonoBehaviour
{
    public Item item;
    public bool isEmpty;
    public GameObject icone;

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
}