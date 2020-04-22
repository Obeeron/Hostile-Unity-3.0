using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.AI;


    public class AnimalController : MonoBehaviour
{
    
   
    private float canJump = 0f;
    Animator anim;
    public int compt=0;
    Rigidbody rb;

   
    public GameObject joueur;
   
    private NavMeshAgent agent;
    public Animal_Data aniData;
    
    void Start()
    {
       anim = GetComponent<Animator>();
       rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
   
    void meurs()
    {
        Inventaire.instance.InitializeItems();
        int taille=Inventaire.instance.items.Count - 1;
        Inventaire.instance.items[taille].itemData = aniData.itemData;
        Inventaire.instance.Add(Inventaire.instance.items[taille]);
        Destroy(this);
        Debug.Log("mourru");
    }
    void perdVie(int degat)
    {
        if (degat == 0)
            return;
       
        if (aniData.life - degat <= 0)
            meurs();

    }
    
    public Vector3 newPosRand( float radius)
    {
        Vector3 newPos = Random.insideUnitSphere * radius ;
        newPos += transform.position; 
        return newPos;
      
    }

    void Update()
    {
        float distanceEnnemi = Vector3.Distance(transform.position, joueur.transform.position);
        Debug.Log("Distance :" + distanceEnnemi);
        compt +=1; 
        Vector3 newPOS;
        if (distanceEnnemi < aniData.distanceEnnemiMin)
        {

            Vector3 newDir = transform.position - joueur.transform.position;
            Vector3 newPos = transform.position + newDir;
            agent.SetDestination(newPos);
            // ControllPlayer();
        }
        if (distanceEnnemi > aniData.distanceEnnemiMax)
        {

            Vector3 newDir = transform.position - joueur.transform.position;
            Vector3 newPos = transform.position - newDir;
            agent.SetDestination(newPos);
            // ControllPlayer();
            perdVie(7);
            Debug.Log("lolilol");
        }
        
        //quand ils sont pas obligé de fuir ou de se rapprocher
        else if (distanceEnnemi > aniData.distanceEnnemiMin && distanceEnnemi < aniData.distanceEnnemiMax)
        {
            
                agent.SetDestination(newPosRand(40f));
            
             
        }
           

    }
   /*
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
    }*/
}