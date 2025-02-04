using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] Transform orientation;

    float rotX;
    float rotY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotX -= mouseY;
        rotY += mouseX;

        rotX = Mathf.Clamp(rotX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotX, rotY, 0f);
        orientation.localRotation = Quaternion.Euler(0f, rotY, 0f);
    }
}
