using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joueur
{
    public class PlayerInterraction : MonoBehaviour
    {
        Camera cam;
        // Start is called before the first frame update
        void Start()
        {
            cam = GetComponentInChildren<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 2))
                {
                    FarmingItem farming = hit.collider.GetComponent<FarmingItem>();

                    if (farming != null)
                    {
                        farming.GetHit(1);
                    }
                }
            }
        }
    }
}