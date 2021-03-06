﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmingItem : NetworkObject
{
    private bool isAlive = true;
    private GameObject groundcheck;
    public enum Type
    {
        Tree,
        Stone
    };
#pragma warning disable 649
    public Type type;
    [SerializeField] private int DropNmb;
    [SerializeField] public float life;
    public AudioClip[] sounds;

    public AudioClip destroyed;
#pragma warning restore
    private int itemDroped;

    public void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "GroundCheck")
                groundcheck = transform.GetChild(i).gameObject;
        }
        switch (type)
        {
            case Type.Tree:
                itemDroped = (int)NetworkItemsController.prefabID.LOG;
                break;
            case Type.Stone:
                itemDroped = (int)NetworkItemsController.prefabID.STONE;
                break;
            default:
                itemDroped = -1;
                break;
        }
    }

    public void GetHit(float dmg)
    {
        if (isAlive)
        {
            if (type == Type.Tree)
                SoundHitTree(ID);
            else if (type == Type.Stone)
                SoundHitRock(ID);
            life -= dmg;
            AliveUpdate();

        }
    }

    private void SoundHitTree(int ID)
    {
        TreeNetworkController.instance.SoundTree(ID);
    }

    private void SoundHitRock(int ID)
    {
        RockNetworkController.instance.SoundRock(ID);
    }


    private void AliveUpdate()
    {
        if (life <= 0f)
        {
            isAlive = false;
            switch(type){
            case Type.Tree:
                TreeNetworkController.instance.DestroyFarmingItem(ID);
                break;
            case Type.Stone:
                RockNetworkController.instance.DestroyFarmingItem(ID);
                break;
            }
        }
    }

    public void DestroyFarmingItem(bool spawn = false){
        switch(type)
        {
            case Type.Tree:
                StartCoroutine(Falling(spawn));
                break;
            case Type.Stone:
                StartCoroutine(WaitToDestroy(spawn));
                break;
        }
    }

    private IEnumerator Falling(bool spawn = false)
    {
        transform.position += new Vector3(0, 2f, 0);
        gameObject.layer = 10;
        Rigidbody itemRigid = gameObject.AddComponent<Rigidbody>();
        itemRigid.mass = 1;
        //itemRigid.constraints = RigidbodyConstraints.FreezeRotation;
        //yield return new WaitForSeconds(0.2f);
        itemRigid.constraints = RigidbodyConstraints.FreezeRotationY;
        StartCoroutine(Timed(spawn));
        do
        {
            yield return null;
        }while (!isonground());
        StopCoroutine(Timed());
        Destroying(spawn);
    }

    private IEnumerator Timed(bool spawn = false)
    {
        yield return new WaitForSeconds(6f);
        StopCoroutine(Falling());
        Destroying(spawn);
    }

    private IEnumerator WaitToDestroy(bool spawn = false)
    {
        yield return new WaitForSeconds(2);
        Destroying(spawn);
    }

    private void Destroying(bool spawn = false)
    {
        Vector3 dropPosition = transform.position;
        dropPosition.y += 1f;
        Vector3 topPosition = dropPosition;
        float angle = 0f;
        float radius = 1.8f;
        Vector3 rotation = Vector3.zero;
        if (type == Type.Tree)
        {
            topPosition = groundcheck.transform.position;
            rotation =  new Vector3(90f, 0, 0);
        }
        Destroy(gameObject);
        //drop new items here
        if (spawn)
        {
            while (DropNmb-- > 0)
            {
                NetworkItemsController.instance.InstantiateNetworkObject(itemDroped, dropPosition, rotation);
                if (type == Type.Tree)
                {
                    dropPosition = Vector3.Lerp(dropPosition, topPosition, 0.1f);
                }
                else
                {
                    dropPosition.x = topPosition.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
                    dropPosition.z = topPosition.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                    angle += 60f;
                    if (angle >= 380f)
                    {
                        angle = 0f;
                        radius += 0.5f;
                    }
                }
            }
        }
    }

    private bool isonground()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundcheck.transform.position, new Vector3(0, -1, 0), out hit, 1f))
        {
            if (hit.transform.name == "Terrain")
                return true;
        }
        return false;
    }

    //permet de voir la range d'interaction
    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }*/
}