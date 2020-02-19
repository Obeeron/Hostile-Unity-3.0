using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Joueur
{
    public class Camera_Movement : MonoBehaviour
    {
        #region Variables
        [Header("Camera Properties")]
        public float mouseSensi = 8f;
        public float smoothness = 0.74f;

        private Transform playerCamera;

        private Vector2 mouseRotate;

        private float yRotation;
        
        private PlayerControls controls;
        #endregion

        private void Awake()
        {
            controls = new PlayerControls();
            playerCamera = GetComponentInChildren<Camera>().transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector2 mouse = controls.InGame.MouseMovement.ReadValue<Vector2>();
            
            mouseRotate.x = Mathf.Lerp(mouse.x * mouseSensi * Time.fixedDeltaTime, mouseRotate.x, smoothness);
            mouseRotate.y = Mathf.Lerp(mouse.y * mouseSensi * Time.fixedDeltaTime,mouseRotate.y, smoothness);

            yRotation -= mouseRotate.y;
            yRotation = Mathf.Clamp(yRotation, -90f, 90f);

            playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseRotate.x);
        }

        //enables controls input
        private void OnEnable()
        {
            controls.InGame.Enable();
        }
        private void OnDisable()
        {
            controls.InGame.Disable();
        }
    }
}