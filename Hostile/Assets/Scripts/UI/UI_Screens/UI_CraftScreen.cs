using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UI_CraftScreen : UI_Screen, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Item item; // Public pour voir les changements ou pour debug

        public void OnPointerEnter(PointerEventData eventData)
        {
            //On récupère l'item dans l'ui
            item = eventData.pointerCurrentRaycast.gameObject.GetComponent<Slots>().item;
            item.OnEnter(); // On affiche les items needed dans le panel associé
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            // On clear la panel items needed
            item.OnExit();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            item.OnClick();
        }
    }
}
