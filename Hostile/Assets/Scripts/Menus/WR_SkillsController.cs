using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WR_SkillsController : MonoBehaviour
{
    public GameObject player;
    public GameObject cam;
    public GameObject skillPanel;

    public float transitionLerpSpeed;

    public Transform playerView;
    public Transform UIView;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !skillPanel.activeSelf)
            SwitchSkillPanelState(true);
    }

    public void OnFinishPressed()
    {
        SwitchSkillPanelState(false);
    }

    public void SwitchSkillPanelState(bool state)
    {
        //Panel
        skillPanel.SetActive(state);

        //Cameras
        Transform target = (state) ? UIView: playerView;
        StopCoroutine(SmoothCameraChange(target, state));
        StartCoroutine(SmoothCameraChange(target, state));

        //Player scripts
        player.GetComponent<PlayerMovement_Obee>().enabled = !state;
        player.GetComponent<Camera_Obee>().enabled = !state;
    }

    IEnumerator SmoothCameraChange(Transform target, bool state)
    {
        while (state && Vector3.Distance(cam.transform.position, target.position) > 0.01f)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, target.position, transitionLerpSpeed);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, target.rotation, transitionLerpSpeed);
            Debug.Log("Transitionning Camera");
            yield return new WaitForEndOfFrame();
        }

        cam.transform.position = target.position;
        cam.transform.rotation = target.rotation;

        //Cursor
        Cursor.visible = state;
        Cursor.lockState = (state) ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
