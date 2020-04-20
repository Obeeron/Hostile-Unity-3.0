using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace UI
{
    public class UI_Controller_Game : UI_Controller
    {
        #region Variables
        [Header("Screens")]
        public UI_Screen inventoryBagScreen;
        public UI_Screen hotbarScreen;
        public UI_Screen craftScreen; 

        [Header("UIMode Events")]
        public UnityEvent OnUIModeEnable = new UnityEvent();
        public UnityEvent OnUIModeDisable = new UnityEvent();

        private bool inUIMode = false;

        public GameObject crossHair;
        public TextMeshProUGUI text_InteractMain;
        public TextMeshProUGUI text_InteractSub;
        #endregion

        #region Main Methods
        // Update is called once per frame
        void Update()
        {
            if (playerControls.InGame.Inventory.triggered)
            {
                Debug.Log("Inventory key pressed!");
                HandleInventoryKeyPressed();
            }
            if(playerControls.InGame.Craft.triggered){
                HandleCraftKeyPressed();
            }
        }
        #endregion

        #region Helper Methods
        void HandleInventoryKeyPressed()
        {
            if (inventoryBagScreen.Active)
            {
                if(craftScreen.Active)
                    DisableScreen(craftScreen);
                SetUIModeActive(false);
                DisableScreen(inventoryBagScreen);
            }
            else
            {
                SetUIModeActive(true);
                EnableScreen(inventoryBagScreen);
            };
        }

        void HandleCraftKeyPressed()
        {
            if(!inventoryBagScreen.Active)
                HandleInventoryKeyPressed();

            if(craftScreen.Active)
                DisableScreen(craftScreen);
            else
                EnableScreen(craftScreen);
        }

        public void SetUIModeActive(bool state)
        {
            if (inUIMode == state) return;

            inUIMode = state;
            Cursor.visible = inUIMode;
            if (state)
            { 
                Cursor.lockState = CursorLockMode.None;
                OnUIModeEnable?.Invoke();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                OnUIModeDisable?.Invoke();
            }
        }
        #endregion

    }
}