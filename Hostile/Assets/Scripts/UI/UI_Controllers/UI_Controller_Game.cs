using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class UI_Controller_Game : UI_Controller
    {
        #region Variables
        [Header("Screens")]
        public UI_Screen inventoryBagScreen;
        public UI_Screen hotbarScreen;

        [Header("UIMode Events")]
        public UnityEvent OnUIModeEnable = new UnityEvent();
        public UnityEvent OnUIModeDisable = new UnityEvent();

        private bool inUIMode = false;
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
                
        }
        #endregion

        #region Helper Methods
        void HandleInventoryKeyPressed()
        {
            if (inUIMode)
            {
                SetUIModeActive(false);
                DisableScreen(inventoryBagScreen);
            }
            else
            {
                SetUIModeActive(true);
                EnableScreen(inventoryBagScreen);
            };
        }

        public void SetUIModeActive(bool state)
        {
            if (inUIMode == state) return;

            if (state)
            {
                inUIMode = true;
                Cursor.visible = inUIMode;
                Cursor.lockState = CursorLockMode.None;
                if (OnUIModeEnable != null)
                {
                    Debug.Log(OnUIModeEnable.ToString());
                    Debug.Log("Event has been filled");
                }
                else
                {
                    Debug.Log("Event is empty");
                }
                OnUIModeEnable?.Invoke();
            }
            else
            {
                inUIMode = false;
                Cursor.visible = inUIMode;
                Cursor.lockState = CursorLockMode.Locked;
                OnUIModeDisable?.Invoke();
            }
        }
        #endregion

    }
}