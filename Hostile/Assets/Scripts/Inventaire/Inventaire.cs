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
    void Awake ()
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
    public int selectedSlotIndex=0;
    public Slots hoveredSlot = null;
    Item itemNetwork;

    public GameObject player;

    private void Start()
    {
        InitializeInventory();
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !slots[selectedSlotIndex].isEmpty)
            Drop();
        else if (Input.mouseScrollDelta.y != 0)
            changeSelection();
    }

    void InitializeInventory()
    {
        selectedSlotIndex = 0;
        slots = GetComponentsInChildren<Slots>(true);
    }

    public void changeSelection()
    {
        slots[selectedSlotIndex].Selected(false);

        if (Input.mouseScrollDelta.y > 0) selectedSlotIndex--;
        else selectedSlotIndex++;

        selectedSlotIndex %= nbSlotsHotbar;

        if (selectedSlotIndex < 0)
            selectedSlotIndex = nbSlotsHotbar - 1;

        slots[selectedSlotIndex].Selected(true);
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
    }

    public void RemoveofList(Item item)
    {
        items.Remove(item);
        
        slots[selectedSlotIndex].Reset2();
    }

    public void RemoveofCraft(Item item)
    {
        items.Remove(item);
        GameObject gm = GameObject.Find("SlotHotBarHolder");
        GameObject gm2 = GameObject.Find("SlotHolder");
        Slots[] Slots = gm.GetComponentsInChildren<Slots>();
        Slots[] Slots2 = gm2.GetComponentsInChildren<Slots>();
        bool end = false;
        foreach (Slots slot in Slots)
        {
            if (slot.item == item && !end)
            {
                Debug.Log(slot.name + "is getting wiped");
                slot.isEmpty = true;
                slot.item = null;
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
                slot.icone.GetComponent<Image>().sprite = null;
                slot.icone.SetActive(false);
                end = true;
            }
        }
    }
    
}


