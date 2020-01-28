using UnityEngine;

public class ViewController : MonoBehaviour
{
    public Transform playerCamera;

    public float mouseSensitivity = 150f;
    public float smooth; 

    float yRotation;

    private Vector2 mouseRotation;
    private Vector2 oldMouseRotation;

    private void Start()
    {
        yRotation = 0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        oldMouseRotation.x = mouseRotation.x;
        oldMouseRotation.y = mouseRotation.y;

        mouseRotation.x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseRotation.y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        mouseRotation.x = Mathf.Lerp(mouseRotation.x, oldMouseRotation.x, smooth);
        mouseRotation.y = Mathf.Lerp(mouseRotation.y, oldMouseRotation.y, smooth);

        yRotation = Mathf.Clamp(yRotation - mouseRotation.y, -90f, 90f);

        transform.Rotate(Vector3.up * mouseRotation.x);
        playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }
}
