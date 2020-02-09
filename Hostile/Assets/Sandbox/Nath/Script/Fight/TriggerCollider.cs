using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterController main = this.GetComponentInParent<CharacterController>();
        if (other != main)
        {
            Debug.Log("Touched");
            if (other.GetComponent<Player>() != null)
            {
                Player player = this.GetComponentInParent<FightSystem>();
                Player ennemy = other.gameObject.GetComponent<Player>();
                ennemy.life -= player.damage;
                Debug.Log(ennemy.life);
                ennemy.checkLife();
            }
        }
        

    }
}
