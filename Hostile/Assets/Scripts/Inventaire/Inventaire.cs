using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;
using ExitGames.Client.Photon;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Inventaire : MonoBehaviour//, IBeginDragHandler, IEndDragHandler, IDropHandler

{
    #region Singleton 
    // design pattern pour restreindre l'instanciation d'une classe à un seul objet


    //on crée une unique instance accessible de partout
    public static Inventaire instance;

    //appelé avant le start
    void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("erreur, il y a deja une instance Inventory");
            return;
        }
        instance = this;
    }
    #endregion

    PhotonView photonView;
    const byte DROP_ITEM_EVENT = (byte)4;
    const byte PICKUP_ITEM_EVENT = (byte)5;

    public int espaceDisponible = 20;
    public List<Item> items = new List<Item>();
    public int nbSlots = 20;
    public int nbSlotsHotbar = 5;
    public Slots[] slots;
    public int selectedSlotIndex = 0;
    public Slots hoveredSlot = null;

    private Item currentEquippedNetwork;
    private bool equipped;
    private Item itemSelected;
    private Arms_Transform arms;
    public GameObject hand;
    public GameObject handNet;

    private GameObject[] items_Equipable;
    private GameObject[] items_Equipable_Network;
    private int indexCurrentlyEquipped;
    private bool isItemInitialized = false;

    public GameObject player;

    private void Start()
    {
        InitializeInventory();
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!isItemInitialized)
            InitializeItems();
        if (Input.GetKeyDown(KeyCode.W) && !slots[selectedSlotIndex].isEmpty)
            Drop();
        else if (Input.mouseScrollDelta.y != 0)
            changeSelection();
    }

    void InitializeItems()
    {
        if (player != null)
        {
            initializeItemsLocal();
            initializeItemsNetwork();
            itemSelected = slots[selectedSlotIndex].item;
            Equip();
        }
    }
    void initializeItemsNetwork()
    {
        items_Equipable_Network = new GameObject[arms.items_to_Equip_Network.Length];
        if(arms.Arms_Network != null)
        {
            for (int i = 0; i < arms.items_to_Equip_Network.Length; i++)
            {
                items_Equipable_Network[i] = arms.items_to_Equip_Network[i];
            }
        }
    }

    void initializeItemsLocal()
    {

        arms = player.GetComponentInChildren<Arms_Transform>();
        items_Equipable = new GameObject[arms.items_to_Equip.Length];
        if (arms.Arms != null)
        {
            for (int i = 0; i < arms.items_to_Equip.Length; i++)
            {
                items_Equipable[i] = arms.items_to_Equip[i];
                items_Equipable[i].SetActive(false);
                //  items_Instantiated[i] = Instantiate(items_Equipable[i], new Vector3(0,0,0), Quaternion.identity,arms.Arms);
                //  items_Instantiated[i].SetActive(false);
            }


            int ID = (int)items_Equipable[0].GetComponent<Item>().itemData.prefabID;
            Debug.Log("To add in inventory id : " + ID);
            Craft_Controller.instance.CraftItem(ID, true);

            

            isItemInitialized = true;
        }



    }

    void InitializeInventory()
    {
        selectedSlotIndex = 1;
        slots = GetComponentsInChildren<Slots>(true);
    }

    public void Equip()
    {

        if (itemSelected != null)
        {
            if (itemSelected.canBeEquipped)
            {
                hand = arms.Arms.gameObject;
                handNet = arms.Arms_Network.gameObject;
                //Debug.Log("Trying to equip");

                //Réseau

                int ID = itemSelected.ID;
                Item item = (Item)NetworkItemsController.instance.GetNetworkObject(ID);
                NetworkItemsController.instance.SynchronizeItem(ID, true, handNet.transform.position, false);
                item.transform.parent = handNet.transform;
                //Debug.Log(item.name.Substring(0, item.name.IndexOf("(")) + "_Network");
                foreach (var current in items_Equipable_Network)
                {
                    if (current.name == (item.name.Substring(0,item.name.IndexOf("("))+"_Network"))
                    {
                        //Debug.Log("Founded");
                        item.transform.localPosition = current.transform.position;
                        item.transform.localRotation = current.transform.rotation;
                    }
                        
                }

                //
                arms.ItemEquippedNetwork = item.transform;
                currentEquippedNetwork = item;

                //Local
                GameObject itemLocal = Switch(item.equippedIndex); // pour le local
                arms.ItemEquipped = itemLocal.transform;

                //
                equipped = true;
                ChangeStats(equipped);
            }
        }
        
    }

    public GameObject Switch(int index, bool clean = false)
    {
        if (clean)
        {
            items_Equipable[index].SetActive(false);
            return items_Equipable[index];
        }
        else
        {
            items_Equipable[indexCurrentlyEquipped].SetActive(false);
            items_Equipable[index].gameObject.SetActive(true);
            indexCurrentlyEquipped = index;
            return items_Equipable[index];
        }   
    }

    public void DesEquip()
    {
        if (itemSelected.canBeEquipped)
        {
            GameObject Equipped_Network = arms.ItemEquippedNetwork.gameObject;
            GameObject Equipped = arms.ItemEquipped.gameObject;
            if(Equipped_Network != null)
            {
                int ID = itemSelected.ID;
                NetworkItemsController.instance.SynchronizeItem(ID, false, new Vector3(0,0,0),false);
            }
            if (Equipped != null)
                Switch(indexCurrentlyEquipped, true);
            equipped = false;
            ChangeStats(equipped);
        }
    }

    public void ChangeStats(bool equipped)
    {
        
        if (equipped)
        {
            Joueur.StatsController.instance.UpdateChoppingStrength(itemSelected.ChoppingStrength);
            Joueur.StatsController.instance.UpdateMiningStrength(itemSelected.MiningStrength);
            Joueur.StatsController.instance.UpdateStrength(itemSelected.Strength);
        }
        else
        {
            Joueur.StatsController.instance.UpdateChoppingStrength(0);
            Joueur.StatsController.instance.UpdateMiningStrength(0);
            Joueur.StatsController.instance.UpdateStrength(1);
        }
    }

    public void changeSelection()
    {
        if(!isItemInitialized)
            InitializeItems();

        slots[selectedSlotIndex].Selected(false);

        //desequip if neded
        itemSelected = slots[selectedSlotIndex].item;
        if(itemSelected != null && equipped)
        {
            if (itemSelected.canBeEquipped)
            {
                DesEquip();
            }
        }
        

        if (Input.mouseScrollDelta.y > 0) selectedSlotIndex--;
        else selectedSlotIndex++;

        selectedSlotIndex %= nbSlotsHotbar;

        if (selectedSlotIndex < 0)
            selectedSlotIndex = nbSlotsHotbar - 1;

        slots[selectedSlotIndex].Selected(true);
        //equip if needed
        itemSelected = slots[selectedSlotIndex].item;
        if(itemSelected != null)
        {
            if (itemSelected.canBeEquipped)
            {
                Equip();
            }
        }

    }
    
    public bool Add(Item item)
    {
        if (items.Count >= espaceDisponible)
        {
            Debug.Log("Pas assez d'espace dans l'inventaire");
            return false;
        }

        items.Add(item);

        for (int i=0; i<nbSlots; i++)
        {  
            if (slots[i].isEmpty)
            {
                slots[i].Add(item);
                Debug.Log("Item add in " + i + " slots");
                return true;
            }
        }  

        throw new Exception("Inventaire.Add : incohérence : espace dispo dans l'inventaire mais slot tous remplis");  
    }

    private void Drop()
    {
        Item droppedItem = slots[selectedSlotIndex].item;
        droppedItem.Drop(player.transform.position);
        RemoveofList(slots[selectedSlotIndex].item);
        DesEquip();
    }

    public void RemoveofList(Item item)
    {
        items.Remove(item);
        
        slots[selectedSlotIndex].Reset2();
    }

    public void RemoveofCraft(Item item,int j)
    {
        items.RemoveAt(j);
        bool end = false;
        foreach(Slots slot in slots)
        {
            if (slot.item == item && !end)
            {
                slot.isEmpty = true;
                slot.item = null;
                //slot.GetComponentInChildren<Image>().sprite = null;
                slot.icone.GetComponent<Image>().sprite = null;
                slot.icone.SetActive(false);
                end = true;
            }
        }
        /*
        GameObject gm = GameObject.Find("SlotHotBarHolder");
        GameObject gm2 = GameObject.Find("SlotHolder");
        Slots[] Slots = gm.GetComponentsInChildren<Slots>();
        Slots[] Slots2 = gm2.GetComponentsInChildren<Slots>();
        foreach (Slots slot in Slots)
        {
            if (slot.item == item && !end)
            {
                Debug.Log(slot.name + "is getting wiped");
                slot.isEmpty = true;
                slot.item = null;
                slot.GetComponentInChildren<Image>().sprite = null;
                slot.icone.GetComponent<Image>().sprite = null;
                slot.icone.SetActive(false);
                end = true;
            }
        }
        foreach(Slots slot in Slots2)
        {
            if (slot.item == item && !end)
            {
                Debug.Log(slot.name + "is getting wiped in slotHolder");
                slot.isEmpty = true;
                slot.item = null;
                slot.GetComponentInChildren<Image>().sprite = null;
                slot.icone.GetComponent<Image>().sprite = null;
                slot.icone.SetActive(false);
                end = true;
            }
        }*/
    }

}


