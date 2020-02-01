using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKmy : MonoBehaviour
{
    Camera cam;
    void Start()
    {
        cam = Camera.main;   
    }
    void Interaction()
    {
        RaycastHit hit;
        //si on touche qlqhch vers la où la caméra regarde (jusqu'a 2 m)
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2))
        {
            //on crée une interactable qui va contenir le composant Interacatable de ce qui a été touché
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            //si ca a touché qlqch d'interactabel
            if (interactable != null)
                interactable.Interact();
            else
                Debug.Log("oops, object pas interactable");
            
        }

    }
  
    void Update()
    {
        //si le joueur veut intéragir avec qqlch
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction();
        }
    }
}
