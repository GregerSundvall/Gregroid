using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    // Basic movement
    private float torque = 1;
    private float baseJumpForce = 6f;
    private float maxVelocity = 30f;
    private float moveValue = 0;
    private bool isStopping = false;
    
    // Also movement kinda
    private bool isGrounded = false;
    private bool isTouchingWall = false;
    private bool wasGroundedSinceLastJump = true;
    
    // Abilities
    private bool hasDash = false;
    private bool hasHiJump = false;
    private bool hasDoubleJump = false;
    private bool hasInfiniteJump = false;
    private bool canBuild = false;

    // Weapons
    private bool hasBomb = false;
    private bool hasSuperBomb = false;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.maxAngularVelocity = 1000;
    }

    private void Update()
    {
        IsGrounded();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void IsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.6f);
        Debug.Log(isGrounded);
    }

    void Move()
    {
        if (rigidBody.angularVelocity.magnitude < maxVelocity && 
            rigidBody.angularVelocity.magnitude > -maxVelocity &&
            !isStopping)
        {
            rigidBody.AddTorque(Vector3.back * torque * moveValue, ForceMode.Impulse);
        }
    }

    void OnJump()
    {
        if (isGrounded)
        {
            rigidBody.AddForce(Vector3.up * baseJumpForce, ForceMode.Impulse);
        }
        
    }

    void OnMove(InputValue value)
    {
        isStopping = false;
        moveValue = value.Get<float>();
    }

    void OnStop()
    {
        isStopping = true;
        moveValue = 0;
        rigidBody.angularVelocity = Vector3.zero;
    }
  
}
