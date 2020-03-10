using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

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


    public int espaceDisponible = 20;
    public List<Item> items = new List<Item>();
    public int nbSlots = 20;
    public int nbSlotsHotbar = 5;
    public Slots[] slots;
    public int selectedSlotIndex=0;
    public Slots hoveredSlot = null;

    public GameObject player;

    private void Start()
    {
        InitializeInventory();
        //Slots[] unorderedSlots = GetComponentsInChildren<Slots>(true);
        //

        //for (int i = 0; i < nbSlotsHotbar; i++)
        //{
        //    (slots[i]) = (unorderedSlots[i + nbSlots - nbSlotsHotbar]);
        //    Debug.Log(string.Format("i={0}   (i+nbSlots-nbSlotsHotbar)={1}", i, i + nbSlots - nbSlotsHotbar));
        //}
            
        //for (int i = nbSlotsHotbar; i < nbSlots; i++)
        //    (slots[i]) = (unorderedSlots[i-nbSlotsHotbar]);
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

    private void Drop()
    {
        slots[selectedSlotIndex].item.Drop(player.transform);
        RemoveofList(slots[selectedSlotIndex].item);
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

    public void RemoveofList(Item item)
    {
        items.Remove(item);
        slots[selectedSlotIndex].Reset2();
    }
        
}


