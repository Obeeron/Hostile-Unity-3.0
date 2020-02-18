using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace UI
{
    public class UI_QuitScreen : UI_Screen
    {
        #region Variables
        [Header("Targeted Menu Properties")]
        public int mainMenuSceneIndex = 0;
        #endregion


        public void OnQuitConfirm()
        {
            Debug.Log("Leaving " + PhotonNetwork.CurrentRoom.Name + "..");
            PhotonNetwork.LeaveRoom();
            Debug.Log("Room left");

            SceneManager.LoadScene(mainMenuSceneIndex);
        }
    }
}
