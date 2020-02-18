using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UI_Screen : MonoBehaviour
    {
        #region Variables
        [Header("Main Properties")]
        public Selectable m_StartSelectable;

        [Header("Screen Events")]
        public UnityEvent onScreenStart = new UnityEvent();
        public UnityEvent onScreenClose = new UnityEvent();

        private Animator animator;
        private bool active = false;
        public bool Active { get { return active; } }
        #endregion


        #region Main Methods
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();

            if (m_StartSelectable)
            {
                EventSystem.current.SetSelectedGameObject(m_StartSelectable.gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Helper Methods
        public virtual void StartScreen()
        {
            active = true;
            onScreenStart?.Invoke();
            HandleAnimator("show");
        }

        public virtual void CloseScreen()
        {
            active = false;
            onScreenClose?.Invoke();
            HandleAnimator("hide");
            //if(animator.runtimeAnimatorController == null)
            //    gameObject.SetActive(false);
        }

        void HandleAnimator(string trigger)
        {
            if(animator.runtimeAnimatorController != null)
                animator.SetTrigger(trigger);                
        }
        #endregion
    }
}
