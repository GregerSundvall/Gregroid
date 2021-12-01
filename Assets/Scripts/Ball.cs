using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField] private float torque = 50;
    [SerializeField] private float jumpForce = 10f;
    private float maxVelocity = 30f;
    private Rigidbody rigidBody;
    private float moveValue = 0;
    

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.maxAngularVelocity = 1000;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // rigidBody.rotation = new Quaternion(0,0,rigidBody.rotation.z, rigidBody.rotation.w);
        
        // transform.Rotate(0,0,transform.rotation.z);
        
        if (rigidBody.angularVelocity.magnitude < maxVelocity && 
            rigidBody.angularVelocity.magnitude > -maxVelocity)
        {
            rigidBody.AddTorque(Vector3.back * torque * moveValue, ForceMode.Impulse);
        }
        
    }

    void OnJump()
    {
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<float>();
        Debug.Log("input: "+ value.Get<float>());
    }

  
}
