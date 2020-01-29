using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera_Movement : MonoBehaviour
{
    protected float mouseSensi = 100f;

    public Camera playerCamera;
    public CharacterController playerBody;
    
    
    private PlayerControls controls;
    private float xRotation = 0f;

    private void Awake()
    {
        controls = new PlayerControls();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = controls.InGame.MouseMovement.ReadValue<Vector2>();

        float xMouse = mouse.x * mouseSensi * Time.deltaTime;
        xRotation -= mouse.y * mouseSensi * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.transform.Rotate(Vector3.up * xMouse);
    }

    //enables controls input
    private void OnEnable() => controls.InGame.Enable(); 
    private void OnDisable() => controls.InGame.Disable();
}
