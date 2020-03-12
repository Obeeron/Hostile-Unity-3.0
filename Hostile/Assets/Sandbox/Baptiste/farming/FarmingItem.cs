using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmingItem : MonoBehaviour
{
    private bool isAlive = true;
    private GameObject itembody;
    private GameObject groundcheck;
    enum Type
    {
        Tree,
        Stone
    };
#pragma warning disable 649
    [SerializeField] private Type type;
    [SerializeField] private int DropNmb;
    [SerializeField] private float life;
#pragma warning restore
    private string itemDroped;

    public void Start()
    {
        itembody = this.gameObject;
        for(int i = 0; i < itembody.transform.childCount; i++)
        {
            if (itembody.transform.GetChild(i).name == "GroundCheck")
                groundcheck = itembody.transform.GetChild(i).gameObject;
        }
        switch (type)
        {
            case Type.Tree:
                itemDroped = "Logs";
                break;
            case Type.Stone:
                itemDroped = "Stone";
                break;
            default:
                itemDroped = " ";
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
        if (life < 0f)
        {
            isAlive = false;
            StartCoroutine(Falling());
        }
    }

    private IEnumerator Falling()
    {
        Rigidbody itemRigid = itembody.AddComponent<Rigidbody>();
        itemRigid.mass = 1;
        itemRigid.AddForce(new Vector3(1f,-10f,0));
        StartCoroutine(Timed());
        do
        {
            yield return null;
        }while (!isonground());
        Destroying();
    }

    private IEnumerator Timed()
    {
        yield return new WaitForSeconds(4f);
        StopCoroutine(Falling());
        Destroying();
    }

    private void Destroying()
    {
        Vector3 dropPosition = itembody.transform.position;
        Destroy(itembody);
        //drop new items here
        while (DropNmb > 0)
        {
            DropNmb--;
            Debug.Log("Droping Items at position : " + dropPosition);
            GameObject drop = PhotonNetwork.Instantiate(itemDroped, dropPosition, Quaternion.identity);
            //Debug.Log(drop.name);
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
