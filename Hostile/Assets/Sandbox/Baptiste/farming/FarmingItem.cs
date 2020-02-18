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

    public void Update()
    {
        AliveUpdate();
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

    public void Interact(float strength = 1f, float weapon = 1f)
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
        //drop new items here
        while (DropNmb > 0)
        {
            DropNmb--;
            Debug.Log("Droping Itmes at position : " + dropPosition);
            PhotonNetwork.Instantiate(itemDroped, dropPosition, Quaternion.identity);
            dropPosition.y += 0.1f;
        }
    }

    //permet de voir la range d'interaction
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
