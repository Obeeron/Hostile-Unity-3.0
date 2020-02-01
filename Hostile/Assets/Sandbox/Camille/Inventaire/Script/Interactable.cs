using UnityEngine;

//classe qui permet d'interagir avec les items/coffres/ennemis
public class Interactable : MonoBehaviour
{
    //distance a laquelle il sera possible d'intéragir
    public float distance = 5f;

    
    //methode qui doit etre overwritten car différente si c'est un item, ennemi, coffre
    public virtual void Interact()
    {
        Debug.Log("en train d'intéragir avec " + transform.name);
    }
    
    //fonction de unity qui permet de "dessiner", ici on veut dessiner un cercle de distance distance autour de l'objet interactable
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distance);

    }
}
