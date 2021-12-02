using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    // Movement
    private float torque = 1f;
    private float jumpForce = 6f;
    private float jumpForceMultiplier = 1f;
    private float maxVelocity = 30f;
    private float moveValue = 0f;
    private bool isStopping = false;
    private float dashForce = 8f;
    
    // Also movement kinda
    private bool isGrounded = false;
    private bool isTouchingWall = false;
    private int jumpsSinceGrounded = 0;
    
    // Abilities
    private bool hasDash = true;
    //private bool hasDoubleDash = false;
    private bool hasDoubleJump = false;
    private bool hasWallJump = false;
    private bool hasInfiniteJump = false;
    private bool canBuild = false;
    
    // // Weapons
    // private bool hasBomb = false;
    // private bool hasSuperBomb = false;

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
        Roll();
    }

    void IsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.8f);
        if (isGrounded)
        {
            jumpsSinceGrounded = 0;
        }
    }

    void IsTouchingWall()
    {
        if (Physics.Raycast(transform.position, Vector3.left, 0.8f) ||
            Physics.Raycast(transform.position, Vector3.right, 0.8f))
        {
            isTouchingWall = true;
            return;
        }
        isTouchingWall = false;
    }

    void OnDash()
    {
        if (hasDash)
        {
            rigidBody.AddForce(rigidBody.velocity.normalized * dashForce, ForceMode.Impulse);
        }
    }

    void Roll()
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
        var currentJumpForce = jumpForce * jumpForceMultiplier;
        var forceMode = ForceMode.Impulse;
        
        if (isGrounded)
        {
            rigidBody.AddForce(Vector3.up * currentJumpForce, forceMode);
            jumpsSinceGrounded++;
        } else if (isTouchingWall && hasWallJump)
        {
            rigidBody.AddForce(Vector3.up * currentJumpForce, forceMode);
            //jumpsSinceGrounded++;
        } else if (hasDoubleJump && jumpsSinceGrounded == 1)
        {
            rigidBody.AddForce(Vector3.up * currentJumpForce, forceMode);
            jumpsSinceGrounded = 2;
        } else if (hasInfiniteJump)
        {
            rigidBody.AddForce(Vector3.up * currentJumpForce, forceMode);
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
