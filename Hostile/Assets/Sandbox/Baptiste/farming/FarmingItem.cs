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
                StartCoroutine(WaitToDestroy());
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

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroying();
    }

    private void Destroying()
    {
        Vector3 dropPosition = transform.position;
        dropPosition.y += 1f;
        Vector3 topPosition = dropPosition;
        float angle = 0f;
        float radius = 1.8f;
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