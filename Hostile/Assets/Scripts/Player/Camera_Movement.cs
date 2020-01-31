using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera_Movement : MonoBehaviour
{
    protected float mouseSensi = 8f;
    public float smoothness;

    public Camera playerCamera;
    public CharacterController playerBody;
    
    
    private PlayerControls controls;
    private float xRotation;
    /*private Vector2 mouseRotate;*/

    private void Awake()
    {
        controls = new PlayerControls();
    }
    // Start is called before the first frame update
    void Start()
    {
        xRotation = 0f;
        smoothness = 0.8f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = controls.InGame.MouseMovement.ReadValue<Vector2>();
        
        /*mouseRotate.x = Mathf.Lerp(mouse.x, mouseRotate.x, smoothness);
        mouseRotate.y = Mathf.Lerp(mouse.y,mouseRotate.y, smoothness);*/

        float xMouse = mouse.x * mouseSensi * Time.deltaTime;
        xRotation -= mouse.y * mouseSensi * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -85f, 70f);


        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.transform.Rotate(Vector3.up * xMouse);
    }

    //enables controls input
    private void OnEnable() => controls.InGame.Enable(); 
    private void OnDisable() => controls.InGame.Disable();
}
