using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UI_CraftScreen : UI_Screen, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Item item; // Public pour voir les changements ou pour debug
        private Craft_Controller cr;
        private int ID;


        private void Start()
        {
            cr = GameObject.Find("CraftController").GetComponent<Craft_Controller>();
        }
        public Item GetItem_ID(int ID)
        {
            return NetworkItemsController.instance.netObjPrefabs[ID].GetComponent<Item>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log(eventData);
            Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
            //On récupère l'item dans l'ui
            if(eventData.pointerCurrentRaycast.gameObject.GetComponent<Slots>() != null)
            {
                Debug.Log(eventData.pointerCurrentRaycast.gameObject.GetComponent<Slots>().ID);
                ID = eventData.pointerCurrentRaycast.gameObject.GetComponent<Slots>().ID;
                item = GetItem_ID(ID);
                cr.itemData = item.itemData;
                cr.OnEnter(); // On affiche les items needed dans le panel associé
            }
                
            //
            
            
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            // On clear la panel items needed
            cr.itemData = null;
            item = null;
            cr.OnExit();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            cr.CraftItem(ID);
        }
    }
}
