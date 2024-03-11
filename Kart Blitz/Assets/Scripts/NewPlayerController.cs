using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    //Movement Stuff
    [SerializeField] private CharacterController characterController;
    public GameObject player;
    public Transform playerTran; 
    public Vector3 offset;
    public Quaternion offsetRotation;
    [SerializeField] private float speed;
    [SerializeField] private GameObject playerCamera;

    //Rotation Stuff
    [SerializeField] private float rotS;

    //Gas/Brake Stuff
    [SerializeField] private GameObject GasOff;
    [SerializeField] private GameObject GasOn;
    [SerializeField] private GameObject BrakeOff;
    [SerializeField] private GameObject BrakeOn;


    private float moveF;
    private float mouseMove;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Rotate();
        ReverseCameraRotation();
    }

     void LateUpdate()
    { 
       CameraStuff();
      
    }

    private void Rotate()
{
    float horizontalInput = Input.GetAxis("Horizontal");
    if (horizontalInput != 0) // fix rotating while still
    {
        mouseMove += horizontalInput * rotS;
        transform.rotation = Quaternion.Euler(0, mouseMove, 0);
    }
}


    private void ReverseCameraRotation()
    {
        float moveInput = Input.GetAxis("Vertical");
        if (moveInput < 0)
        {
            mouseMove += Input.GetAxis("Mouse X") * rotS;
            transform.rotation = Quaternion.Euler(0, mouseMove, 0);
        }
    }

    private void CameraStuff()
{
    float moveInput = Input.GetAxis("Vertical");
    if (moveInput < 0) // Player is moving backward
    {
        offsetRotation = Quaternion.Euler(0, 270f, 0f);
    }
    else
    {
        offsetRotation = Quaternion.Euler(0, 90, 0); // Reset rotation when not moving backward
    }

    transform.position = playerTran.transform.position + playerTran.rotation * offset;
    transform.rotation = playerTran.rotation * offsetRotation;
}


    private void Move()
{
    float x = 0;
    float y = playerCamera.transform.eulerAngles.y;
    float z = Input.GetAxis("Vertical");
    float input = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2));

    if (Input.GetAxis("Vertical") < 0) // Player is moving backward
    {
        z *= -1; // Invert z axis
    }

    Vector3 inputDirection = Quaternion.AngleAxis(y, Vector3.up) * new Vector3(x, 0, z);
    Vector3 verticalSpeed = new Vector3(0, moveF, 0);
    Vector3 playerMove = inputDirection + verticalSpeed;

    if (input > 0)
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
