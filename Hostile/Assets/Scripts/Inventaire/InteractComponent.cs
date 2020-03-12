using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    Transform cam;
    void Start()
    {
        cam = GetComponentInChildren<Camera>().transform;   
    }
    void Interaction()
    {
        RaycastHit hit;
        int mask = ~(1 << 12);
        //si on touche qlqhch vers la où la caméra regarde (jusqu'a 2 m)
        if (Physics.Raycast(cam.position, cam.forward, out hit, 10f, mask))
        {
            //on crée une interactable qui va contenir le composant Interacatable de ce qui a été touché
            Interactable interactable = hit.collider.GetComponent<Interactable>();  
            //si ca a touché qlqch d'interactable
            if (interactable != null && hit.distance <= interactable.distance)
                interactable.Interact();
            else
                Debug.Log("oops, object pas interactable / distance : " + hit.distance);
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
