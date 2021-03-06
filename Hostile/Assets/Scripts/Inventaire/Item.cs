﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Item : Interactable
{
    public ItemData itemData;
    public int usure;
    public bool inInventory;
    public bool isSelected;
    public bool canBeEquipped;
    public float Strength;
    public float ChoppingStrength;
    public float MiningStrength;

    public int equippedIndex;

    float decayEnd;
    float decayStart;

    private GameObject gm;
    private Button slot;
    private TextMeshProUGUI txt;

    
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = itemData.mesh;
        gameObject.AddComponent(typeof(BoxCollider));
        if(itemData.material != null)
            meshRenderer.material = itemData.material;
        if (!inInventory)
            CalculTemps();
    }

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    public override string GetMainInteractMessage(){
        return "PICK UP";
    }   

    public override string GetSubInteractMessage(){
        return itemData.name;
    }

    
    void PickUp()
    {
        if (Inventaire.instance.Add(this))
        {
            NetworkItemsController.instance.SynchronizeItem(ID,false,transform.position);
        }
    }

    void CalculTemps()
    {
        decayStart = Time.time;
        decayEnd = decayStart + itemData.decayTime;
    }

    public void Drop(Vector3 position)
    {
        CalculTemps();
        position.y+=1;
        if (canBeEquipped)
        {
            NetworkItemsController.instance.SynchronizeItemEquip(ID, true, position, Quaternion.identity, 0, true);
        }
        NetworkItemsController.instance.SynchronizeItem(ID,true,position);
    }


    // Update is called once per frame
    void Update()
    {
        if (!itemData.persistent && !inInventory)
            if (Time.time >= decayEnd)
                NetworkItemsController.instance.DeleteNetworkObject(ID);
    }
}