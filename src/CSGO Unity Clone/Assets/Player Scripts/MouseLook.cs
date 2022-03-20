using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;
    public Transform playerBody;

    // Start is called before the first frame update
    void Start()
    {
       // Lock cursor to middle of screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        transform.localRotation = Quaternion.Euler(xRotation,0f, 0f);
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
