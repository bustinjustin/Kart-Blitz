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
//    [SerializeField] private GameObject GasOff;
  //  [SerializeField] private GameObject GasOn;
 //   [SerializeField] private GameObject BrakeOff;
 //   [SerializeField] private GameObject BrakeOn;
    [SerializeField] private GameObject SteeringWheel;
    private float steeringWheelRotation;
    [SerializeField] private GameObject[] wheels; // Array to hold all wheels

    private float moveF;
    private float RotAS = 90;
    private float targetVelocity;
    private Vector3 gravity;

    private Vector3 carTilt;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Rotate();
        // ReverseCameraRotation();
        RotateWheels();
        // ResetSW();
        // TiltCar();
    }

    private void LateUpdate()
    {
        CameraStuff();
    }

    private void Rotate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        RotAS += horizontalInput * rotS;
        transform.rotation = Quaternion.Euler(0, RotAS, 0);
        
        float steeringWheelTargetRotation = horizontalInput * 45;
        steeringWheelRotation = Mathf.Lerp(steeringWheelRotation, steeringWheelTargetRotation, 3 * Time.deltaTime);
            
        SteeringWheel.transform.rotation = Quaternion.Euler(30, transform.eulerAngles.y, -steeringWheelRotation);

        // wheel childed to parent.
        // turn parent's local rotation when you steer.
        // wheel rotates w/ local rotation. doesn't care what way parent is facing.
        
    }

    private void CameraStuff()
    {
        float moveInput = Input.GetAxis("Vertical");
        if (moveInput < 0) // Player is moving backward
        {
            offsetRotation = Quaternion.Euler(0, 180, 0);
            offset = new Vector3(0.45f,3,1.5f);
        }
        else
        {
            offsetRotation = Quaternion.Euler(0, 0, 0); // Reset rotation when not moving backward
            offset = new Vector3(0.45f,3,1.5f);

        }

        Vector3 cameraPosition = driver.transform.position + driver.transform.rotation * offset;
        Quaternion cameraRotation = driver.transform.rotation * offsetRotation;

        playerCamera.transform.SetPositionAndRotation(cameraPosition, cameraRotation);
    }

    private void Move()
    {
        float y = transform.eulerAngles.y;
        float z = Input.GetAxis("Vertical");

        targetVelocity = Mathf.Lerp(targetVelocity, z, 0.5f * Time.deltaTime);

        Vector3 inputDirection = new Vector3(0, 0, targetVelocity);
        Vector3 playerMove = Quaternion.AngleAxis(y, Vector3.up) * inputDirection * speed;

        float gravityY = characterController.isGrounded ? -0.5f : gravity.y - 5;
        gravity = new(0, gravityY, 0);

        characterController.Move(Time.deltaTime * (playerMove + gravity));
    }

    private void HandBrake()
    {
        //check if player is holding down space, make speed lerp to 0
        //drifting?
    }

    private void RotateWheels()
        {
            float moveInput = Input.GetAxis("Vertical");

            // Calculate rotation amount based on player's input
            float rotationAmount = Input.GetAxis("Vertical") * Time.deltaTime * 500;

            if (moveInput > 0) // Player is moving forward

            // Rotate each wheel in the array
            foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(Vector3.left * -rotationAmount);
            }
            else if (moveInput < 0)
             foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(Vector3.right * rotationAmount);
            }
    }
}
