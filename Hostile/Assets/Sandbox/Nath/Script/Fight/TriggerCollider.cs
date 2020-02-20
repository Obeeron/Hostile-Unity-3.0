using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterController main = this.GetComponentInParent<CharacterController>();
        if (other != main) // On vérifie qu'on ne se tappe pas soi-même
        {
            Debug.Log("Touched");
            /*if (other.GetComponent<Entity>() != null)
            {
                Entity player = GetComponent<Entity>();
                Entity ennemy = other.gameObject.GetComponent<Entity>();
                ennemy.getHit(player.Damage); //getHit() -->
            }*/
        }
    }
}
