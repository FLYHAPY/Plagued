using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float senX;
    public float senY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public bool back;

    public float targetZAngle = 90.0f;
    public float rotationSpeed = 2.0f;

    [Header("Refrences")]
    public PlayerMovement pm;

    [Header("Side")]
    public bool right;
    public bool left;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (!pm.wallRunning && back == false)
        {
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        if(back == true)
        {
            float currentZAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, 0, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, currentZAngle);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

       

    }
    public void RotateLeft()
    {
        float currentZAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetZAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, currentZAngle);
    }

    public void RotateRight()
    {
        float currentZAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, -targetZAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, currentZAngle);
    }

    public void GoBack()
    {
        back = true;
    }
}
