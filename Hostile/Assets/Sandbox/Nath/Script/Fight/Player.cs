using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Object
{
    public int damage;
    public int stamina;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public bool checkLife()
    {
        if(this.life <= 0)
        {
            isAlive = false;
            return false;
        }
        return true;
    }
}
