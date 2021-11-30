using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField] private float torque = 50f;
    [SerializeField] private float jumpForce = 10f;
    private Rigidbody rigidBody;
    

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void OnJump()
    {
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnMove(InputValue value)
    {
        Debug.Log(value.Get<float>());
        Debug.Log(transform.rotation);
        rigidBody.AddTorque(Vector3.back * torque * value.Get<float>(), ForceMode.Impulse);
    }
    // void GoRight()
    // {
    //     rigidBody.AddTorque(transform.forward * torque, ForceMode.Impulse);
    // }
}
