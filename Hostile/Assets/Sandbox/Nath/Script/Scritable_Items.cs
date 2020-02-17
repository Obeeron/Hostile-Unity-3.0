using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Nouvel_item_Craft", menuName = "Inventaire/Item_Craft")]
public class Scritable_Items : ScriptableObject
{
    new public string name = "nouvel item";
    public Sprite icone = null;
    public List<Scritable_Items> Material;
    public List<int> Number;

    private Button slot;
    private TextMeshProUGUI txt;
    private GameObject gm;

    //Méthode
    public void OnEnter()
    {
        
        for (int i = 0; i < Material.Count; i++) // Je changerais ça par un foreach plus tard pour l'instant ça marche
        {
            if(Number[i] > 0)
            {
                switch (i)
                {
                    case 0:
                        gm = GameObject.Find("Needed_1");

                        slot = gm.GetComponent<Button>();
                        gm.transform.Find("Needed_Number_1").gameObject.SetActive(true);
                        txt = gm.transform.Find("Needed_Number_1").gameObject.GetComponent<TextMeshProUGUI>();
                        txt.text = Number[i].ToString();
                        txt.fontSize = 105;
                        break;
                    case 1:
                        gm = GameObject.Find("Needed_2");

                        slot = gm.GetComponent<Button>();
                        gm.transform.Find("Needed_Number_2").gameObject.SetActive(true);
                        txt = GameObject.Find("Needed_2").GetComponentInChildren<TextMeshProUGUI>();
                        txt.text = Number[i].ToString();
                        txt.fontSize = 105;
                        break;
                    case 2:
                        gm = GameObject.Find("Needed_3");

                        slot = gm.GetComponent<Button>();
                        gm.transform.Find("Needed_Number_3").gameObject.SetActive(true);
                        txt = GameObject.Find("Needed_3").GetComponentInChildren<TextMeshProUGUI>();
                        txt.text = Number[i].ToString();
                        txt.fontSize = 105;
                        break;
                    default:
                        gm = GameObject.Find("Needed_1");

                        slot = gm.GetComponent<Button>();
                        gm.transform.Find("Needed_Number_1").gameObject.SetActive(true);
                        txt = GameObject.Find("Needed_1").GetComponentInChildren<TextMeshProUGUI>();
                        txt.text = Number[i].ToString();
                        txt.fontSize = 105;
                        break;
                }

                slot.image.sprite = Material[i].icone;

            }
        }
    }

    public void OnExit()
    {

        Transform panel = GameObject.Find("Needed").transform;
        foreach (Transform child in panel)
        {
            Sprite sp = Resources.Load<Sprite>("Sprites/inventorySlot");
            if (sp != null)
            {
                child.gameObject.GetComponent<Image>().sprite = sp;

            }
            if (child.childCount == 1)
            {
                child.GetChild(0).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < Material.Count; i++)
        {
            if (Number[i] > 0)
            {
                switch (i)
                {
                    case 0:
                        gm = GameObject.Find("Needed_1");
                        gm.transform.Find("Needed_Number_1").gameObject.SetActive(false);
                        break;
                    case 1:
                        gm = GameObject.Find("Needed_2");
                        gm.transform.Find("Needed_Number_2").gameObject.SetActive(false);
                        break;
                    case 2:
                        gm = GameObject.Find("Needed_3");
                        gm.transform.Find("Needed_Number_3").gameObject.SetActive(false);
                        break;
                    default:
                        gm = GameObject.Find("Needed_1");
                        gm.transform.Find("Needed_Number_1").gameObject.SetActive(false);
                        break;
                }
            }
        }
    }

    public void OnClick()
    {
        List<int> count = CheckMaterials_In_Inventory();
        if (IsCraftable(count)) // on ajoute dans l'inventaire
        {
            Debug.Log("Crafted !");
            //Inventaire.instance.Add(this); Il faut merge Scriptable_Items et Item
        }
        else
        {
            Debug.Log("Unable to craft item : need more relevant materials");
        }
        
        
    }

    public List<int> CheckMaterials_In_Inventory()
    {
        // On rentre le nombre d'item de l'inventaire correspondant aux items nécessaires
        List<int> count = new List<int>(); // On construit une liste qui contient le nombre de chaque matériaux nécessaire que le joueur possède dans son inventaire
        for (int i = 0; i < Material.Count; i++)
        {
            int currentCount = 0;
            count.Add(currentCount);
            if (Number[i] > 0) // au cas où le scriptable object soit mal crée // Devrait toujours rentrer dans le if
            {
                foreach (Item item in Inventaire.instance.items)
                {
                    Debug.Log(item.itemData.name);
                    if (item.itemData.name == Material[i].name)
                    {
                        count[i] += 1;
                    }
                }
            }
        }
        return count;
    }

    public bool IsCraftable(List<int> count)
    {
        List<int> indexToRemove = new List<int>();
        if (count.Count == Material.Count) // Devrait toujours être false sinon c'est qu'il y a un problème dans les matériaux du scriptable_Object
        {
            bool isCraftable = true;
            int i = 0;
            while (i < count.Count && isCraftable)
            {
                if (Number[i] <= count[i])
                {
                    for (int j = 0; j < Inventaire.instance.items.Count; j++) // Tant qu'il reste des items 
                    {
                        int index = Inventaire.instance.items.FindIndex(a => a.name == Material[i].name); // je récupère le premier qui match le nom que je recherche
                        if (index >= 0) // si il a trouvé quelquechose
                        {
                            indexToRemove.Add(index);
                        }
                    }
                }
                else
                {
                    isCraftable = false;
                }
                i++;
            }

            if (isCraftable)
            {
                foreach (int index in indexToRemove)
                {
                    Inventaire.instance.items.RemoveAt(index);
                }
            }
            return isCraftable;
        }
        Debug.Log("Error in Scriptable_Items : OnClick -> IsCraftable");
        return false;
    }
}
