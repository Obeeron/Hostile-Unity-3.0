using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Nouvel_item_Materials", menuName = "Inventaire/Item_Materials")]
public class Scritable_Materials : ScriptableObject
{
    new public string name = "nouvel item";
    public Sprite icone = null;
}

