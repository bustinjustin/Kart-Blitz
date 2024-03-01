using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
public GameObject player;
public Transform playerCamera;
public Vector3 offset;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = playerCamera.transform.position + playerCamera.rotation * offset;
        transform.rotation = playerCamera.rotation;
        
    }
}
