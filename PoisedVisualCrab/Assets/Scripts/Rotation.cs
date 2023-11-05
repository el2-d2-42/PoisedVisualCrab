using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 0.5f; // Degrees per second, calculated as 360 / (24 * 60 * 60)
    private Vector3 axisOfRotation;

    void Start()
    {
        // Set the axis of rotation based on Earth"s axial tilt
        // test
        //axisOfRotation = Quaternion.Euler(0, 0,23.5f) * Vector3.up;
    }

    void Update()
    {
        //transform.RotateAround(transform.position, axisOfRotation, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
