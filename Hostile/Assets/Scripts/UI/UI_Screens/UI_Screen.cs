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

        protected Animator animator;
        private bool hasAnimation = false;
        private bool active = false;
        public bool Active { get { return active; } }
        #endregion


        #region Main Methods
        void Awake()
        {
            animator = GetComponent<Animator>();
            hasAnimation = animator.runtimeAnimatorController != null;
        }

        void Start()
        {
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
            if(!hasAnimation)
               gameObject.SetActive(true);
        }

        public virtual void CloseScreen()
        {
            active = false;
            onScreenClose?.Invoke();
            HandleAnimator("hide");
            if(!hasAnimation){
                gameObject.SetActive(false);
                Debug.Log("blblblblb");
            }
                
        }

        void HandleAnimator(string trigger)
        {
            if(animator != null && animator.runtimeAnimatorController != null)
                animator.SetTrigger(trigger);                
        }
        #endregion
    }
}
