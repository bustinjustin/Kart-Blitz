using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
public GameObject player;
public Transform playerTransform;
public float downwardTilt;
public Vector3 offset;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = playerTransform.transform.position + playerTransform.rotation * offset;

        float x = playerTransform.eulerAngles.x + downwardTilt;
        float y = playerTransform.eulerAngles.y;
        float z = playerTransform.eulerAngles.z;

        transform.rotation = Quaternion.Euler(x, y, z);
        
    }
}
