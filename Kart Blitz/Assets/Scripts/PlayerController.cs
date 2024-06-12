using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject playerCamera;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float verticalMin;
    [SerializeField] private float verticalMax;


    private float velocityY;
    private float mouseX;
    private float mouseY;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Rotate()
    {
        {
            mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            mouseY = Mathf.Clamp(mouseY, verticalMin, verticalMax);
            playerCamera.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            transform.rotation = Quaternion.Euler(0, mouseX, 0);
        }
    }

    private void Move()
    {
        float x = 0;
        float y = playerCamera.transform.eulerAngles.y;
        float z = Input.GetAxis("Vertical");
        float input = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2));

        Vector3 inputDirection = Quaternion.AngleAxis(y, Vector3.up) * new Vector3(x, 0, z);
        velocityY = characterController.isGrounded ? -0.5f : velocityY - 5;
        Vector3 verticalSpeed = new Vector3(0, velocityY, 0);
        Vector3 playerMove = inputDirection + verticalSpeed;

        characterController.Move(speed * Time.deltaTime * playerMove);
    }
}
