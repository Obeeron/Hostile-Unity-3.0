using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Obee : MonoBehaviour
{
    Transform playerCamera;

    public float mouseSensitivity = 150f;
    public float smoothness;

    float yRotation;

    private Vector2 mouseRotation;

    private void Start()
    {
        yRotation = 0f;
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 newMouseRotation = new Vector2( Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime,
                                                Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime);

        mouseRotation.x = Mathf.Lerp(newMouseRotation.x, mouseRotation.x, smoothness);
        mouseRotation.y = Mathf.Lerp(newMouseRotation.y, mouseRotation.y, smoothness);

        yRotation = Mathf.Clamp(yRotation - mouseRotation.y, -90f, 90f);

        transform.Rotate(Vector3.up * mouseRotation.x);
        playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }
}
