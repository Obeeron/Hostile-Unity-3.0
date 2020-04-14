using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDrop : MonoBehaviour
{
    public void DropAll(GameObject player)
    {
        List<Item> inventory = Inventaire.instance.items;
        int i = inventory.Count-1;
        while (i >= 0)
        {
            inventory[i].Drop(player.transform.position);
            inventory.RemoveAt(i);
            i--;
        }
    }
}
