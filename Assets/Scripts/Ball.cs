using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    // Movement
    private float torque = 1f;
    private float jumpForce = 7f;
    private float jumpForceMultiplier = 1f;
    private float maxVelocity = 30f;
    private float moveValue = 0f;
    private bool isStopping = false;
    private float dashForce = 8f;
    
    // Also movement kinda
    private bool isGrounded = false;
    private int jumpsSinceGrounded = 0;
    private bool rollInput = false;
    
    // Abilities
    private bool hasDash = false;
    private bool hasDoubleJump = false;
    private bool hasWallJump = false;
    private bool hasInfiniteJump = false;
    //private bool canBuild = false;
    //private bool hasDoubleDash = false;

    
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
        if (isStopping)
        {
            Stop();
        }
        else
        {
            Roll();
        }
    }

    private void FixedUpdate()
    {
        Roll();
        //Debug.Log(rollInput);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpsSinceGrounded = 0;
            isGrounded = true;
        }

        
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoubleJumpPickUp"))
        {
            hasDoubleJump = true;
            Destroy(other.gameObject);
            Debug.Log("DoubleJump " + hasDoubleJump);
        }
        
        if (other.gameObject.CompareTag("InfiniteJumpPickUp"))
        {
            hasInfiniteJump = true;
            Destroy(other.gameObject);
            Debug.Log("InfiniteJump " + hasInfiniteJump);
        }
        
        if (other.gameObject.CompareTag("WallJumpPickUp"))
        {
            hasWallJump = true;
            Destroy(other.gameObject);
            Debug.Log("WallJump " + hasWallJump);
        }
    }

    bool IsTouchingWall()
    {
        if (Physics.Raycast(transform.position, Vector3.left, 0.8f) ||
            Physics.Raycast(transform.position, Vector3.right, 0.8f))
        {
            return true;
        }
        return false;
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
            rigidBody.AddTorque(Vector3.back * torque * moveValue, ForceMode.VelocityChange);
        }
    }
    

    void OnJump()
    {
        if (isGrounded ||
            hasDoubleJump && jumpsSinceGrounded == 1 ||
            IsTouchingWall() && hasWallJump ||
            hasInfiniteJump)
        {
            Jump();
        } 
    }

    void Jump()
    {
        var currentJumpForce = jumpForce * jumpForceMultiplier;
        var forceMode = ForceMode.Impulse;
        rigidBody.AddForce(Vector3.up * currentJumpForce, forceMode);
        jumpsSinceGrounded++;
    }

    void OnMove(InputValue value)
    {
        isStopping = false;
        moveValue = value.Get<float>();
        //rollInput = value.isPressed;
    }

    void OnStop()
    {
        isStopping = true;
    }

    void Stop()
    {
        moveValue = 0;
        rigidBody.angularVelocity = Vector3.zero;
    }
  
}
