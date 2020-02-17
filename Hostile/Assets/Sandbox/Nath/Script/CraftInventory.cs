using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CraftInventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject player;
    // Update is called once per frame
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (inventory.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                GetComponent<Joueur.Player_Movement>().enabled = true;
                GetComponent<Joueur.Camera_Movement>().enabled = true;
                GetComponent<FightSystem>().enabled = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                GetComponent<Joueur.Player_Movement>().enabled = false;
                GetComponent<Joueur.Camera_Movement>().enabled = false;
                GetComponent<FightSystem>().enabled = false;
            }
            inventory.SetActive(!inventory.activeSelf);
        }
            
            
    }
}
