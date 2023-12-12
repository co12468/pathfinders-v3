using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public Rigidbody pinRigidbody; // Expose the Rigidbody to the Inspector

    void Start()
    {
        initialPosition = transform.position; // Save initial position
        initialRotation = transform.rotation; // Save initial rotation
        pinRigidbody = GetComponent<Rigidbody>(); // Access the Rigidbody component
    }
    public void ResetPin()
    {
        transform.position = initialPosition; // Reset to initial position
        transform.rotation = initialRotation; // Reset rotation
        pinRigidbody.velocity = Vector3.zero;
        pinRigidbody.angularVelocity = Vector3.zero;
    }

    public bool PinHasFallen()
    {
        Vector3 pinsUpVector = transform.up;
        return Vector3.Angle(pinsUpVector, Vector3.up) > 10f;
    }
}