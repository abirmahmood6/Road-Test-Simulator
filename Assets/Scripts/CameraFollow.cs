using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Reference to the car object
    public Transform car;

    // Camera offset from the car
    public Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
    {
        if (car != null)
        {
            // Set the camera's position to follow the car
            transform.position = car.position + offset;
        }
    }
}
