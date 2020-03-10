using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmingItem : MonoBehaviour
{
    private bool isAlive = true;
    private GameObject itembody;
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
        switch (type)
        {
            case Type.Tree:
                itemDroped = "Log";
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
            StartCoroutine(Destroyinng());
        }
    }

    private IEnumerator Destroyinng()
    {
        Rigidbody itemRigid = itembody.AddComponent<Rigidbody>();
        itemRigid.mass = 1;
        itemRigid.AddForce(new Vector3(4,-5f,0));
        yield return new WaitForSeconds(4f);
        Vector3 dropPosition = itembody.transform.position;
        Destroy(itembody);
        //drop new items here
        while (DropNmb > 0)
        {
            DropNmb--;
            Debug.Log("Droping Itmes at position : " + dropPosition);
            PhotonNetwork.Instantiate(itemDroped, dropPosition, Quaternion.identity);
        }
    }

    //permet de voir la range d'interaction
    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }*/
}
