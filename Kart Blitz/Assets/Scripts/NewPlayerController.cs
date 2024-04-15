using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    // Movement Stuff
    [SerializeField] private CharacterController characterController;
    public Vector3 offset;
    public Quaternion offsetRotation;
    [SerializeField] private float speed;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject driver;

    // Rotation Stuff
    [SerializeField] private float rotS;

    // Moving Objects
    [SerializeField] private GameObject GasOff;
    [SerializeField] private GameObject GasOn;
    [SerializeField] private GameObject BrakeOff;
    [SerializeField] private GameObject BrakeOn;
    [SerializeField] private GameObject SteeringWheel;
    private float steeringWheelRotation;

    private float moveF;
    private float RotAS = 90;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        float horizontalInput = Input.GetAxis("Horizontal");
    }

    private void Update()
    {
        Move();
        Rotate();
        ReverseCameraRotation();
    }

    private void LateUpdate()
    {
        CameraStuff();
    }

    private void Rotate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (moveInput > 0) // Check if there's any vertical input
        {
            RotAS += horizontalInput * rotS;
            transform.rotation = Quaternion.Euler(0, RotAS, 0);
            
            // Adjust the rotation of the steering wheel based on the direction of input
            steeringWheelRotation += horizontalInput * rotS;
            steeringWheelRotation = Mathf.Clamp(steeringWheelRotation, -45, 45);
            SteeringWheel.transform.localRotation = Quaternion.Euler(0, steeringWheelRotation, 0);

            // wheel childed to parent.
            // turn parent's local rotation when you steer.
            // wheel rotates w/ local rotation. doesn't care what way parent is facing.
        }
    }


    private void ReverseCameraRotation()
    {
        float moveInput = Input.GetAxis("Vertical");

        if (moveInput < 0)
        {
            RotAS += Input.GetAxis("Horizontal") * rotS;
            transform.rotation = Quaternion.Euler(0, RotAS, 0);
        }
    }

    private void CameraStuff()
    {
        float moveInput = Input.GetAxis("Vertical");
        if (moveInput < 0) // Player is moving backward
        {
            offsetRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            offsetRotation = Quaternion.Euler(0, 0, 0); // Reset rotation when not moving backward
        }

        float driverYRotation = driver.transform.eulerAngles.y;

        playerCamera.transform.rotation = Quaternion.Euler(0, driverYRotation + offsetRotation.eulerAngles.y, 0);
        playerCamera.transform.position = driver.transform.position + offset;
    }

    private void Move()
    {
        float y = playerCamera.transform.eulerAngles.y;
        float z = Input.GetAxis("Vertical");

        if (Input.GetAxis("Vertical") < 0) // Player is moving backward
        {
            z *= -1; // Invert z axis
        }

        Vector3 inputDirection = new Vector3(0, 0, z);
        Vector3 playerMove = Quaternion.AngleAxis(y, Vector3.up) * inputDirection;

        if (z > 0)
        {
            GasOn.SetActive(true);
            GasOff.SetActive(false);
        }
        else
        {
            GasOn.SetActive(false);
            GasOff.SetActive(true);
        }

        characterController.Move(speed * Time.deltaTime * playerMove);
    }

}