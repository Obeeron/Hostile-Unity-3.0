using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CraftInventory : MonoBehaviour
{
    public GameObject inventory;
    // Update is called once per frame
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (inventory.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;

            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            inventory.SetActive(!inventory.activeSelf);
        }
            
            
    }
}
