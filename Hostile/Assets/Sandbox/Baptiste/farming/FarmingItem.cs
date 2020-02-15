using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmingItem : MonoBehaviourPunCallbacks
{
    public float life = 10f;
    private bool isAlive = true;
    private PhotonView item;
    private GameObject itembody;
    //public GameObject dropedItems;

    public void Start()
    {
        item = this.GetComponent<PhotonView>();
        itembody = this.gameObject;
    }

    public void Update()
    {
        AliveUpdate();
    }

    public void Interact(float strength, float weapon)
    {
        float mult = strength*weapon;
        LifeDown(mult);
    }

    private void LifeDown(float mult)
    {
        if (isAlive)
        {
            life -= 1*mult;
        }   
    }

    private void AliveUpdate()
    {
        if (life < 0f && isAlive)
        {
            isAlive = false;
            StartCoroutine(Destroying());
        }
    }

    private IEnumerator Destroying()
    {
        Rigidbody itemRigid = itembody.AddComponent<Rigidbody>();
        itemRigid.mass = 1;
        itemRigid.AddForce(new Vector3(4,-5f,0));
        yield return new WaitForSeconds(4f);
        Vector3 dropPosition = itembody.transform.position;
        PhotonNetwork.Destroy(item);
        //drop new items hear
        Debug.Log("Droping Itmes at position : " + dropPosition);
        //PhotonNetwork.Instantiate("Robot Kyl", dropPosition, Quaternion.identity);
    }

    //permet de voir la range d'interaction
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
