using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmingItem : NetworkObject
{
    private bool isAlive = true;
    private GameObject groundcheck;
    enum Type
    {
        Tree,
        Stone
    };
#pragma warning disable 649
    [SerializeField] private Type type;
    [SerializeField] private int DropNmb;
    [SerializeField] public float life;
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
            life -= dmg;
            AliveUpdate();
        }
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

    public void DestroyFarmingItem(){
        switch(type){
            case Type.Tree:
                StartCoroutine(Falling());
                break;
            case Type.Stone:
                Destroying();
                break;
        }
    }

    private IEnumerator Falling()
    {
        transform.position += new Vector3(0, 2f, 0);
        gameObject.layer = 10;
        Rigidbody itemRigid = gameObject.AddComponent<Rigidbody>();
        itemRigid.mass = 1;
        //itemRigid.constraints = RigidbodyConstraints.FreezeRotation;
        //yield return new WaitForSeconds(0.2f);
        itemRigid.constraints = RigidbodyConstraints.FreezeRotationY;
        StartCoroutine(Timed());
        do
        {
            yield return null;
        }while (!isonground());
        StopCoroutine(Timed());
        Destroying();
    }

    private IEnumerator Timed()
    {
        yield return new WaitForSeconds(6f);
        StopCoroutine(Falling());
        Destroying();
    }

    private void Destroying()
    {
        Vector3 dropPosition = transform.position;
        dropPosition.y += 1f;
        Vector3 topPosition = dropPosition + new Vector3(5f, 0f, 0f);
        Vector3 stonePosition = dropPosition - new Vector3(5f, 0f, 0f);
        float stoneAngle = 0.1f;
        if (type == Type.Tree)
        {
            topPosition = groundcheck.transform.position;
        }
        Destroy(gameObject);
        //drop new items here
        while (DropNmb-- > 0)
        {
            NetworkItemsController.instance.InstantiateNetworkObject(itemDroped, dropPosition, Vector3.zero);
            if (type == Type.Tree)
            {
                dropPosition = Vector3.Lerp(dropPosition, topPosition, 0.1f);
            }
            else
            {
                dropPosition = Vector3.Slerp(stonePosition, topPosition, stoneAngle);
                dropPosition += Vector3.forward;
                stoneAngle += 0.1f;
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