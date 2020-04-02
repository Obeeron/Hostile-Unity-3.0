using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class UI_Controller_WaitingRoom : UI_Controller
    {
        #region Variables
        [Header("Screens")]
        public UI_Screen skillScreen;
        public UI_Screen quitScreen;
        #endregion

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandleEscapeKeyPress();
            }
        }

        void HandleEscapeKeyPress()
        {
            Debug.Log("quitscreen:"+quitScreen.Active);
            if (quitScreen.Active)
            {
                quitScreen.CloseScreen();
            }
            else {
                if (skillScreen.Active)
                    quitScreen.StartScreen();
                else
                    skillScreen.StartScreen();
                
            }
        }

    }
}
