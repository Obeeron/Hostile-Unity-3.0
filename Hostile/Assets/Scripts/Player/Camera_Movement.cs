using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Joueur
{
    public class Camera_Movement : MonoBehaviour
    {
        protected float mouseSensi = 8f;
        public float smoothness;

        public Camera playerCamera;
        public CharacterController playerBody;
        
        
        private PlayerControls controls;
        private float yRotation;
        private Vector2 mouseRotate;

        private void Awake()
        {
            controls = new PlayerControls();
        }
        // Start is called before the first frame update
        void Start()
        {
            yRotation = 0f;
            smoothness = 0.8f;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 mouse = controls.InGame.MouseMovement.ReadValue<Vector2>();
            
            mouseRotate.x = Mathf.Lerp(mouse.x * mouseSensi * Time.deltaTime, mouseRotate.x, smoothness);
            mouseRotate.y = Mathf.Lerp(mouse.y * mouseSensi * Time.deltaTime,mouseRotate.y, smoothness);

            float xRotation = mouseRotate.x ;
            yRotation -= mouseRotate.y;
            yRotation = Mathf.Clamp(yRotation, -85f, 70f);


            playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
            playerBody.transform.Rotate(Vector3.up * xRotation);
        }

        //enables controls input
        private void OnEnable() => controls.InGame.Enable(); 
        private void OnDisable() => controls.InGame.Disable();
    }
}