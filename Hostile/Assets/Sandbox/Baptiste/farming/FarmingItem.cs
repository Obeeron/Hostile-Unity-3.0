using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmingItem : MonoBehaviourPunCallbacks, IPunObservable
{
    private bool isAlive = true;
    private PhotonView item;
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
        item = this.GetComponent<PhotonView>();
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

    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(life);
        }
        else
        {
            this.life = (float)stream.ReceiveNext();
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
        //drop new items here
        while (DropNmb > 0)
        {
            DropNmb--;
            Debug.Log("Droping Itmes at position : " + dropPosition);
            PhotonNetwork.Instantiate(itemDroped, dropPosition, Quaternion.identity);
        }
    }

    //permet de voir la range d'interaction
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
