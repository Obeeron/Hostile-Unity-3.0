using UnityEngine;
using UnityEngine.EventSystems;

public class Items_Crafting : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Item item; // Public pour voir les changements ou pour debug
    private Craft_Controller cr;
    private int ID;


    private void Start()
    {
        cr = this.gameObject.GetComponent<Craft_Controller>();
    }
    public Item GetItem_ID(int ID)
    {
        return NetworkItemsController.instance.netObjPrefabs[ID].GetComponent<Item>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //On récupère l'item dans l'ui
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        ID = eventData.pointerCurrentRaycast.gameObject.GetComponent<Slots>().ID;
        //
        item = GetItem_ID(ID);
        cr.itemData = item.itemData;
        cr.OnEnter(); // On affiche les items needed dans le panel associé
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // On clear la panel items needed
        cr.OnExit();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        cr.CraftItem(ID);
    }
}
