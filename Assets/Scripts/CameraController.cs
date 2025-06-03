using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 90f;

    void Start()
    {
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void Update()
    {
        float mouseXInput = Input.GetAxis("Mouse X");
        transform.Rotate(mouseXInput * mouseSensitivity * Time.deltaTime * Vector3.up);

        //float mouseYInput = Input.GetAxis("Mouse Y");
        //var mouseMovement = new Vector2(mouseXInput, mouseYInput).normalized;

        //transform.Rotate(mouseYInput * mouseSensitivity * Time.deltaTime * Vector3.left);
        //transform.Rotate(mouseMovement.x * mouseSensitivity * Time.deltaTime, mouseMovement.y * mouseSensitivity * Time.deltaTime, 0f);
    }
}
