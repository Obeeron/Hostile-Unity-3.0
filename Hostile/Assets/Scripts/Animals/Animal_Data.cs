using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//permet de créer un nouveau item depuis le menu create de unity
[CreateAssetMenu(fileName = "Nouvel animal", menuName = "Inventaire/AnimalData")]
public class Animal_Data : ScriptableObject

{
   
    public float movementSpeed ;
    public float jumpForce ;
    public float timeBeforeNextJump ;
    private float canJump ;
    Animator anim;
    Rigidbody rb;

    public int life;
    public int degatsInfligesParAttaque;
    public float distanceEnnemiMin;
    public float distanceEnnemiMax;
    public ItemData itemData;
    public string name;
}
