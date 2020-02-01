using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    #region Singleton 
    // design pattern pour restreindre l'instanciation d'une classe à un seul objet


    //on crée une unique instance accessible de partout
    public static Inventaire instance;

    //appelé avant le start
    void Awake ()
    {

        if (instance != null)
        {
            Debug.LogWarning("erreur, il y a deja une instance Inventory");
            return;
        }
        instance = this;
    }
    #endregion


    public int espaceDisponible = 20;
    public List<Items> items = new List<Items>();
    public GameObject inventory;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventory.SetActive(!inventory.activeSelf);
    }

    public bool Add(Items item)
    {
        
        if (!item.isDefaultItem)
        {
            if (items.Count >= espaceDisponible)
            {
                Debug.Log("Pas assez d'espace dans l'inventaire");
                return false;
            }
            items.Add(item);
            return true;
 
        }
        return false;
    }

    public void RemoveofList(Items item)
    {
        items.Remove(item);
    }
}


