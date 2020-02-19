using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

//permet de créer un nouveau item depuis le menu create de unity
[CreateAssetMenu(fileName = "Nouvel item", menuName ="Inventaire/ItemData")]
public class ItemData : ScriptableObject
{
    new public string name = "nouvel item";
    public Sprite icone=null;
    public int maxUsure;
    public float decayTime;
    public bool persistent;
    enum type
    {
        ARMES,
        MEDICAMENTS,
        NOURRITURE,
        EQUIPEMENT,
        OUTILS,
        MATERIAUX
    };
    //public MeshFilter inHandModel = null;
    public Mesh mesh = null;
    public List<Scritable_Items> Material;
    public List<int> Number;
}
