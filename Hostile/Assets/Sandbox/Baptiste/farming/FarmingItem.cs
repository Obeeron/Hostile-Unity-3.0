using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmingItem : Interraction
{
    public float life = 10f;
    private bool isAlive = true;
    private PhotonView item;
    private Rigidbody itembody;
    [SerializeField] float distance;
    //public GameObject dropedItems;

    public void Start()
    {
        item = this.GetComponent<PhotonView>();
        itembody = this.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        AliveUpdate();
    }

    public override void Interact()
    {
        LifeDown();
    }

    private void LifeDown()
    {
        life--;
    }

    private void AliveUpdate()
    {
        if (life < 0f && isAlive)
        {
            isAlive = false;
            StartCoroutine(Destroying());
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distance);

    }

    private IEnumerator Destroying()
    {
        itembody.AddForce(new Vector3(1,-1,0));
        yield return new WaitForSeconds(1.5f);
        Vector3 dropPosition = itembody.transform.position;
        PhotonNetwork.Destroy(item);
        //drop new items hear
        PhotonNetwork.Instantiate("Robot Kyl", dropPosition, Quaternion.identity);
    }
}
