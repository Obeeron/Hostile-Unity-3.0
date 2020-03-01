using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class Item : Interactable
{
    public ItemData itemData;
    public int usure;
    public bool inInventory;
    public bool isSelected;

    float decayEnd;
    float decayStart;

    private GameObject gm;
    private Button slot;
    private TextMeshProUGUI txt;



    private MeshFilter meshFilter;


    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = itemData.mesh;
        if (!inInventory)
            CalculTemps();
    }

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }


    void PickUp()
    {
        Debug.Log("item " + itemData.name + " récupéré!");

        if (Inventaire.instance.Add(this))
        {
            inInventory = true;
            this.gameObject.SetActive(false);
        }
    }
    void CalculTemps()
    {
        decayStart = Time.time;
        decayEnd = decayStart + itemData.decayTime;
    }

    public void Drop(Transform pos)
    {
        inInventory = false;
        CalculTemps();
        gameObject.transform.position = pos.position;
        gameObject.SetActive(true);
    }
    //Méthode Craft
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

    private void loadSprite(Transform child,int i)
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

    public void OnClick()
    {
        //Debug.Log("Clicked");
        List<int> count = CheckMaterials_In_Inventory();
        foreach (var num in count)
        {
            //Debug.Log(num);
        }
        if (IsCraftable(count)) // on ajoute dans l'inventaire
        {
            //Debug.Log("Crafted !");
            Inventaire.instance.Add(this);
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
        for (int i = 0; i < itemData.Material.Count; i++)
        {
            int currentCount = 0;
            count.Add(currentCount);
            if (itemData.Number[i] > 0) // au cas où le scriptable object soit mal crée // Devrait toujours rentrer dans le if
            {
                foreach (Item item in Inventaire.instance.items)
                {
                    if (item.itemData.name == itemData.Material[i].name)
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
        if (count.Count == itemData.Material.Count) // Devrait toujours être true sinon c'est qu'il y a un problème dans les matériaux du scriptable_Object
        {
            bool isCraftable = true;
            int i = 0;
            while (i < count.Count && isCraftable)
            {
                if (itemData.Number[i] <= count[i])
                {
                    for (int j = 0; j < Inventaire.instance.items.Count; j++) // Tant qu'il reste des items 
                    {
                        int index = Inventaire.instance.items.FindIndex(a => a.name == itemData.Material[i].name); // je récupère le premier qui match le nom que je recherche
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


    // Update is called once per frame
    void Update()
    {
        if (!itemData.persistent && !inInventory)
            if (Time.time >= decayEnd)
                Destroy(this.gameObject);
    }
}