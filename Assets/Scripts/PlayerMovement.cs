﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE;
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public CharacterController mCharacterController;
    public Animator mAnimator;
    public float mWalkSpeed = 1.5f;
    public float mRotationSpeed = 50.0f;
    public bool mFollowCameraForward = false;
    public float mTurnRate = 10.0f;
    protected Transform mPlayerTransform;

    private float hInput;
    private float vInput;
    private float speed;
    private bool jump = false;
    private bool crouch = false;
    public float mGravity = -30.0f;
    public float mJumpHeight = 1.0f;
    private Vector3 mVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    void Start()
    {
        mCharacterController = GetComponent<CharacterController>();
    }
    void Update()
    {
 
    }
    private void FixedUpdate()
    {
        ApplyGravity();
    }
    public void HandleInputs()
    {
#if UNITY_STANDALONE
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
#endif
        speed = mWalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = mWalkSpeed * 2.0f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jump = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouch = !crouch;
            Crouch();
        }
    }

    public void Move()
    {
        if (crouch) return;
        if (mAnimator == null) return;
        if (mFollowCameraForward)
        {
            Vector3 eu = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0.0f, eu.y, 0.0f),
                mTurnRate * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0.0f, hInput * mRotationSpeed * Time.deltaTime, 0.0f);
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward).normalized;
        forward.y = 0.0f;

        mCharacterController.Move(forward * vInput * speed * Time.deltaTime);
        mAnimator.SetFloat("PosX", 0);
        mAnimator.SetFloat("PosZ", vInput * speed / (2.0f * mWalkSpeed));

        if(jump)
        {
            Jump();
            jump = false;
        }
    }

    void Jump()
    {
        mAnimator.SetTrigger("Jump");
        mVelocity.y += Mathf.Sqrt(mJumpHeight * -2f * mGravity);
    }
    private Vector3 HalfHeight;
    private Vector3 tempHeight;
    void Crouch()
    {
        mAnimator.SetBool("Crouch", crouch);
        if (crouch)
        {
            tempHeight = CameraConstants.CameraPositionOffset;
            HalfHeight = tempHeight;
            HalfHeight.y *= 0.5f;
            CameraConstants.CameraPositionOffset = HalfHeight;
        }
        else
        {
            CameraConstants.CameraPositionOffset = tempHeight;
        }
    }
    void ApplyGravity()
    {
        mVelocity.y += mGravity * Time.deltaTime;
        if (mCharacterController.isGrounded && mVelocity.y < 0)
            mVelocity.y = 0f;
    }
}
