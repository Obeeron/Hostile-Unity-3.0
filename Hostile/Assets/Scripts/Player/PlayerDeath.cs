using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject LeavePannel;
    // Start is called before the first frame update

    private IEnumerator ShowPannel()
    {
        yield return new WaitForSeconds(2f);
        LeavePannel.SetActive(true);
    }
    
    public void CameraDeath(Camera current, GameObject pl)
    {
        // Debug.Log("CameraDeath");
        Camera campl = pl.GetComponentInChildren<Camera>();
        campl.GetComponent<AudioListener>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Debug.Log("creating new camera...");
        Camera deathcam = Instantiate(current, pl.transform.position  + new Vector3(-3f, 5f, 0),
            Quaternion.Euler(30f, 0, 0));
        // Debug.Log("...camera created");
        Camera.SetupCurrent(deathcam);
        // Debug.Log("Destroying player...");
        PhotonNetwork.Destroy(pl.GetComponent<PhotonView>());
        // Debug.Log("...Player Destroyed");
        StartCoroutine(ShowPannel());
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
