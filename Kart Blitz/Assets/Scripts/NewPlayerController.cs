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
    [SerializeField] private GameObject SteeringWheel;
    private float steeringWheelRotation;
    [SerializeField] private GameObject[] wheels;

    private float RotAS = 90;
    private float targetVelocity;
    private Vector3 gravity;

    // Sound Stuff
    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip accelerationSound;

    private AudioSource audioSource;
    private bool isStartSoundPlaying = true;
    private bool isIdleSoundPlaying = false;
    private bool isRevSoundPlaying = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component could not be added.");
            return;
        }

        audioSource.clip = startSound;
        if (startSound == null)
        {
            Debug.LogError("Start sound clip is not assigned.");
            return;
        }

        Debug.Log("StartSFX will be called now.");
        StartSFX();
    }

  private void Update()
{
    if (!isStartSoundPlaying)
    {
        Move();
        Rotate();
        RotateWheels();
        Brake();

        // Check if the player has stopped moving
        if (characterController.velocity.magnitude < 0.1f)
        {
            if (!isIdleSoundPlaying)
            {
                PlayIdleSound();
            }
        }
        else
        {
            if (isIdleSoundPlaying)
            {
                StopIdleSound();
            }
        }

        if (Input.GetAxis("Vertical") >= 0.5f)
        {
            if(!isRevSoundPlaying)
            PlayRevSound();
        }
        else
        {
            if(isRevSoundPlaying)
            StopRevSound();
        }
    }

    // Check if the start sound is still playing
    if (isStartSoundPlaying && !audioSource.isPlaying)
    {
        isStartSoundPlaying = false;
    }
}



    private void LateUpdate()
    {
        CameraStuff();
    }

    private void StartSFX()
    {
        Debug.Log("StartSFX called.");
        if (audioSource != null && startSound != null)
        {
            Debug.Log("Playing start sound.");
            audioSource.Play();
            isStartSoundPlaying = true;
        }
        else
        {
            Debug.LogError("AudioSource or StartSound is null.");
        }
    }

    private void PlayRevSound()
    {
        Debug.Log("Playing rev sound.");
        audioSource.clip = accelerationSound;
        audioSource.loop = true;
        audioSource.Play();
        isRevSoundPlaying = true;
    }

    private void StopRevSound()
    {
        Debug.Log("Stopping rev sound.");
        audioSource.Stop();
        isRevSoundPlaying = false;
    }

    private void PlayIdleSound()
    {
        Debug.Log("Playing idle sound.");
        audioSource.clip = idleSound;
        audioSource.loop = true;
        audioSource.Play();
        isIdleSoundPlaying = true;
    }

    private void StopIdleSound()
    {
        Debug.Log("Stopping idle sound.");
        audioSource.Stop();
        isIdleSoundPlaying = false;
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
    }

    private void CameraStuff()
    {
        float moveInput = Input.GetAxis("Vertical");
        if (moveInput < 0) // Player is moving backward
        {
            offsetRotation = Quaternion.Euler(0, 180, 0);
            offset = new Vector3(0.45f, 3, 1.5f);
        }
        else
        {
            offsetRotation = Quaternion.Euler(0, 0, 0); // Reset rotation when not moving backward
            offset = new Vector3(0.45f, 3, 1.5f);
        }

        Vector3 cameraPosition = driver.transform.position + driver.transform.rotation * offset;
        Quaternion cameraRotation = driver.transform.rotation * offsetRotation;

        playerCamera.transform.SetPositionAndRotation(cameraPosition, cameraRotation);
    }

    private void Move()
    {
        float y = transform.eulerAngles.y;
        float z = Input.GetAxis("Vertical");

        float previousVelocity = targetVelocity;
        targetVelocity = Mathf.Lerp(targetVelocity, z, 0.5f * Time.deltaTime);

        Vector3 inputDirection = new Vector3(0, 0, targetVelocity);
        Vector3 playerMove = Quaternion.AngleAxis(y, Vector3.up) * inputDirection * speed;

        float gravityY = characterController.isGrounded ? -5f : gravity.y - 2;
        gravity = new Vector3(0, gravityY, 0);

        characterController.Move(Time.deltaTime * (playerMove + gravity));
    }





    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            targetVelocity = Mathf.Lerp(targetVelocity, 0, 2 * Time.deltaTime); 
        }
    }

    private void RotateWheels()
    {
        float moveInput = Input.GetAxis("Vertical");

        // Calculate rotation amount based on player's input
        float rotationAmount = Input.GetAxis("Vertical") * Time.deltaTime * 500;

        if (moveInput > 0) // Player is moving forward
        {
            // Rotate each wheel in the array
            foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(Vector3.left * -rotationAmount);
            }
        }
        else if (moveInput < 0)
        {
            foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(Vector3.right * rotationAmount);
            }
        }
    }
}
