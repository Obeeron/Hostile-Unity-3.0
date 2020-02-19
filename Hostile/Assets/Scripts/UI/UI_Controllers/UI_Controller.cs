using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UI_Controller : MonoBehaviour
    {
        #region Variables
        [Header("Main Properties")]
        public UI_Screen m_startScreen;
        protected PlayerControls playerControls;

        [Header("System Events")]
        public UnityEvent onSwitchScreen = new UnityEvent();

        [Header("Fader Properties")]
        public Image m_Fader;
        public float m_FadeInDuration = 1f;
        public float m_FadeOutDuration = 1f;

        private Component[] screens = new Component[0];

        private UI_Screen currentScreen;
        public  UI_Screen CurrentScreen { get { return currentScreen; } }
        #endregion


        #region Main Methods
        // Start is called before the first frame update
        void Start()
        {
            playerControls = new PlayerControls();
            playerControls.InGame.Enable();
            screens = GetComponentsInChildren<UI_Screen>(true);
            InitializeScreens();

            if(m_startScreen)
            {
                SwitchScreens(m_startScreen);
            }

            m_Fader?.gameObject.SetActive(true);
            FadeIn();
        }
        #endregion


        #region Helper Methods
        public void SwitchScreens(UI_Screen screen)
        {
            currentScreen?.CloseScreen();

            currentScreen = screen;
            currentScreen.gameObject.SetActive(true);
            currentScreen.StartScreen();

            onSwitchScreen?.Invoke();
        }

        private void SwitchScreenState(UI_Screen screen, bool state)
        {
            if (state) screen.StartScreen();
            else screen.CloseScreen();

            //screen.gameObject.SetActive(state);
            onSwitchScreen?.Invoke();
        }

        public void EnableScreen(UI_Screen screen)
        {
            SwitchScreenState(screen, true);
        }

        public void DisableScreen(UI_Screen screen)
        {
            SwitchScreenState(screen, false);
        }

        public void FadeIn()
        {
            m_Fader?.CrossFadeAlpha(0f, m_FadeInDuration, false);
        }

        public void FadeOut()
        {
            m_Fader?.CrossFadeAlpha(1f, m_FadeOutDuration, false);
        }

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        void InitializeScreens()
        {
            foreach(var screen in screens)
            {
                if(screen.GetComponent<Animator>().runtimeAnimatorController)
                    screen.gameObject.SetActive(true);
            }
        }
        #endregion
    }
}