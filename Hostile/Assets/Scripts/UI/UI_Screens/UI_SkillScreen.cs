using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UI_SkillScreen : UI_Screen
    {
        #region Variables
        [Header("World Space Canvas Position:")]
        public Transform skillScreenPosition;
        #endregion

        #region Main Methods
        private void Update()
        {
            if (skillScreenPosition) MoveToPlayer();
        }
        #endregion

        #region HelperMethods
        public override void StartScreen()
        {
            base.StartScreen();
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }

        void MoveToPlayer()
        {
            transform.parent.position = skillScreenPosition.position;
            transform.parent.rotation = skillScreenPosition.rotation;
        }
        #endregion
    }
}
