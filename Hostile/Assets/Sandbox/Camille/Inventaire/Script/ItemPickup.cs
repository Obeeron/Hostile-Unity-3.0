using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Items item;
    public override void Interact()
    {
        base.Interact();
        PickUp();

    }
    void PickUp()
    {
       
        Debug.Log("item " + item.name +" récupéré!");
        if(Inventaire.instance.Add(item))
            Destroy(gameObject);
    }
    
}
