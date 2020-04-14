using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Craft_Controller : MonoBehaviour
{
    #region Singleton
    public static Craft_Controller instance;

    protected void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("erreur, il y a deja une instance NetworkItemsController");
            return;
        }
        instance = this;
    }
    #endregion

    public ItemData itemData;

    public void OnEnter()
    {
        Transform panel = GameObject.Find("Needed").transform;
        int i = 0;
        foreach (Transform child in panel)
        {
            loadSprite(child, i);
            loadNumber(child, i);
            if (i < itemData.Material.Count)
                i++;
        }
    }

    private void loadSprite(Transform child, int i)
    {
        Sprite sp = Resources.Load<Sprite>("Sprites/inventorySlot"); ;
        if (i < itemData.Material.Count)
            sp = Resources.Load<Sprite>("Sprites/" + itemData.Material[i].name);

        if (sp != null)
            child.gameObject.GetComponent<Image>().sprite = sp;
    }

    private void loadNumber(Transform child, int i)
    {
        if (child.childCount == 2)
        {
            GameObject gm = child.GetChild(0).gameObject;
            TextMeshProUGUI txt = gm.GetComponent<TextMeshProUGUI>();
            if (i < itemData.Material.Count && txt != null)
            {
                txt.text = itemData.Number[i].ToString();
                txt.fontSize = 105;
                gm.SetActive(true);
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
            if (child.childCount == 2)
            {
                child.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void CraftItem(int ID, bool forceCraft = false)
    {
        if (forceCraft)
        {
            System.Random rnd = new System.Random();
            int reference = rnd.Next(0, 1000000);
            NetworkItemsController.instance.InstantiateNetworkObject(ID, this.transform.position, new Vector3(0, 0, 0), reference);

            // On l'ajoute dans l'inventaire
            StartCoroutine(Click(reference));
        }
        else
        {
            List<int> count = CheckMaterials_In_Inventory();
            foreach (var num in count)
            {
                //Debug.Log(num);
            }
            if (IsCraftable(count)) // on ajoute dans l'inventaire
            {
                Debug.Log("Item Crafted !");
                //On Instantiate l'item en Réseau
                System.Random rnd = new System.Random();
                int reference = rnd.Next(0, 1000000);
                NetworkItemsController.instance.InstantiateNetworkObject(ID, this.transform.position, new Vector3(0, 0, 0), reference);

                // On l'ajoute dans l'inventaire
                StartCoroutine(Click(reference));


            }
            else
            {
                Debug.Log("Unable to craft item : need more relevant materials");
            }
        }

    }

    IEnumerator Click(int reference)
    {
        int r = NetworkItemsController.instance.NetObjList.Count - 1;
        Debug.Log("LEN : " + r);
        Debug.Log("Reference : " + reference);
        Debug.Log(NetworkItemsController.instance.referenceCode);
        while (NetworkItemsController.instance.referenceCode != reference)
            yield return null;
        int len = NetworkItemsController.instance.NetObjList.Count - 1;
        Debug.Log("LEN OUT : " + len);
        Item item = (Item)NetworkItemsController.instance.NetObjList[len];
        Debug.Log(item.name);
        if (Inventaire.instance.Add(item))
        {
            Debug.Log("Item added to inventory with len " + len);
            NetworkItemsController.instance.SynchronizeItem(len, false, this.transform.position);
            Inventaire.instance.Equip();
        }
    }

    public List<int> CheckMaterials_In_Inventory()
    {
        // On rentre le nombre d'item de l'inventaire correspondant aux items nécessaires
        List<int> count = new List<int>(); // On construit une liste qui contient le nombre de chaque matériaux nécessaire que le joueur possède dans son inventaire
        for (int i = 0; i < itemData.Material.Count; i++)
        {
            int currentCount = 0;
            if (itemData.Number[i] > 0) // au cas où le scriptable object soit mal crée // Devrait toujours rentrer dans le if
            {
                foreach (Item item in Inventaire.instance.items)
                {
                    if (item.itemData.name == itemData.Material[i].name)
                    {
                        currentCount += 1;
                    }
                }

                if(currentCount > 0)
                {
                    count.Add(currentCount);
                }
            }
        }
        return count;
    }

    public bool IsCraftable(List<int> count)
    {
        if (count.Count == itemData.Material.Count) // Devrait toujours être true sinon c'est qu'il y a un problème dans les matériaux du scriptable_Object
        {
            bool isCraftable = true;
            int i = 0;
            while (i < count.Count && isCraftable)
            {
                if (itemData.Number[i] <= count[i])
                {
                    count[i] = itemData.Number[i];
                    int ind = count[i];
                    for (int j = 0; j < Inventaire.instance.items.Count; j++) // Tant qu'il reste des items 
                    {
                        //Debug.Log(Inventaire.instance.items[j].GetComponent<Item>().itemData.name + " De l'inventaire et ///// " + itemData.Material[i].name + "de l'item à craft");
                        if (itemData.Material[i].name == Inventaire.instance.items[j].GetComponent<Item>().itemData.name && ind > 0) // si il a trouvé quelquechose
                        {
                            Debug.Log(Inventaire.instance.items[j].GetComponent<Item>().itemData.name + " is going to be destroyed if craft is succesful");
                            Inventaire.instance.RemoveofCraft(Inventaire.instance.items[j],j);
                            ind--;
                        }
                    }
                }
                else
                {
                    isCraftable = false;
                }
                i++;
            }


            return isCraftable;
        }
        Debug.Log("Error in Scriptable_Items : OnClick -> IsCraftable");
        return false;
    }
}
