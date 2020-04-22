using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    
    public float movementSpeed = 3;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    Animator anim;
    Rigidbody rb;

    public int life = 6;
    public GameObject joueur;
    public float distanceEnnemiMin = 11.0f;
    private NavMeshAgent agent;
    
    void Start()
    {
       // anim = GetComponent<Animator>();
        //:rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distanceEnnemi = Vector3.Distance(transform.position, joueur.transform.position);
        Debug.Log("Distance :" + distanceEnnemi);
        if(distanceEnnemi<distanceEnnemiMin)
        {

                Vector3 newDir = transform.position - joueur.transform.position;
                Vector3 newPos = transform.position + newDir;
               agent.SetDestination(newPos);
           // ControllPlayer();
        }

    }
   
    void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            anim.SetInteger("Walk", 1);
        }
        else {
            anim.SetInteger("Walk", 0);
        }

        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
                rb.AddForce(0, jumpForce, 0);
                canJump = Time.time + timeBeforeNextJump;
                anim.SetTrigger("jump");
        }
    }
}