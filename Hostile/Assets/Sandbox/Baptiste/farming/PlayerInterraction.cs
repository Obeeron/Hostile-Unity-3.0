using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joueur
{
    public class PlayerInterraction : Player_Movement
    {

        Player_Movement player;
        Camera cam;
        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Player_Movement>();
            cam = GetComponentInChildren<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    FarmingItem farming = hit.collider.GetComponent<FarmingItem>();

                    if (farming != null)
                    {
                        farming.Interact();
                    }
                }
            }
        }
    }
}