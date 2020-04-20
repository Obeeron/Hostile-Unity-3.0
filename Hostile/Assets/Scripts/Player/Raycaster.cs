using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Raycaster : MonoBehaviour
{  
    private RaycastHit hit;
    private RaycastHit Hit => hit;
    private GameObject target;
    public GameObject Target => target;

    private Ray ray;

    private Camera cam;
    public float maxDistance = 2.5f;
    public LayerMask layerMask;

    private GameObject crossHair;
    private TextMeshProUGUI text_InteractMain;
    private TextMeshProUGUI text_InteractSub;

    private Interactable interactable;
    private TriggerCollider triggerCollider;

    // Start is called before the first frame update
    void Start()
    {    
        cam = GetComponentInChildren<Camera>();
        crossHair = GameObject.Find("crossHair");
        text_InteractMain = GameObject.Find("text_InteractMain").GetComponent<TextMeshProUGUI>();
        text_InteractSub = GameObject.Find("text_InteractSub").GetComponent<TextMeshProUGUI>();
        
        target = null;
        text_InteractMain.gameObject.SetActive(false);
        crossHair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out hit,maxDistance,layerMask))
        {
            interactable = hit.collider.GetComponent<Interactable>();
            if(interactable){
                if(target != interactable.gameObject){
                    target = interactable.gameObject;

                    text_InteractMain.gameObject.SetActive(true);
                    crossHair.SetActive(true);
                
                    text_InteractMain.text = interactable.GetMainInteractMessage();
                    text_InteractSub.text = interactable.GetSubInteractMessage();
                }
            }
        }
        else{
            interactable = null;
        }

        if(interactable == null){
            if(target != null){
                target = null;
                text_InteractMain.gameObject.SetActive(false);
                crossHair.SetActive(false);
            }
        }
    }
}
