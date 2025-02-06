using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] LayerMask ground;
    [SerializeField] Transform orientation;
    [SerializeField] Animator anim;

    [Header("----- Speeds -----")]
    [SerializeField][Range(4, 10)] int joggingSpeed;
    [SerializeField][Range(7, 15)] int sprintingSpeed;
    [SerializeField][Range(2, 6)] int crouchSpeed;
    [SerializeField][Range(100, 500)] int dashSpeed;
    [SerializeField][Range(5, 15)] int speedMod;

    [Header("----- Jump ----- ")] // added jump just in case, set to 0 for no jump
    [SerializeField] float jumpForce;
    [SerializeField][Range(0, 2)] int jumpMax;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMult;

    [Header("----- Slopes ----- ")] // working on slope mechanics for testing, not sure if will implement to final product
    [SerializeField] float maxSlopeAngle;

    [Header("----- Other Player Settings ----- ")]
    [SerializeField] float crouch;
    [SerializeField] float height;
    [SerializeField] float groundDrag;
    [SerializeField] float groundRayCheck;


    // private fields
    Vector3 moveDir;
    Rigidbody rb;

    bool isGrounded;
    bool canJump;
    bool isCrouching;

    int jumpCounter;

    float movementSpeed;
    float unCrouch;

    // for debugging
    float y;
    float x;
    float z;



    public enum State
    {
        idle
        ,jogging
        ,sprinting
        ,crouching
        ,dashing
        ,air
    }
    State playerState;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
        unCrouch = transform.localScale.y;

    }

    // Update is called once per frame
    void Update()
    {
        // for debugging
        x = rb.linearVelocity.x;
        z = rb.linearVelocity.z;
        y = rb.linearVelocity.y;

        SpeedControl();
        GetPlayerState();

        Jump(); // jump keybind temporarily set to "t"
        Crouch(); // keybind set to left ctrl
        Dash(); // key bind set to space
    }

    void FixedUpdate()
    {
        IsGrounded();
        Movement();
    }

    void Movement()
    {
        moveDir = orientation.forward * Input.GetAxisRaw("Vertical") +
                  orientation.right * Input.GetAxisRaw("Horizontal");

        if (isGrounded)
            rb.AddForce(moveDir.normalized * movementSpeed * 15f, ForceMode.Force);

        else
            rb.AddForce(moveDir.normalized * movementSpeed * airMult * 15f, ForceMode.Force);
    }

    void IsGrounded()
    {
        Debug.DrawRay(orientation.position, Vector3.down * height * groundRayCheck, Color.red);
        isGrounded = Physics.Raycast(orientation.position, Vector3.down, height * groundRayCheck, ground);

        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
        jumpCounter = 0;
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.linearVelocity = new Vector3(limitedVel.normalized.x * movementSpeed, rb.linearVelocity.y, limitedVel.normalized.z * movementSpeed);
        }
    }

    void Jump()
    {
        if (Input.GetButton("Jump") && canJump && isGrounded && (jumpCounter < jumpMax))
        {
            ++jumpCounter;
            canJump = false;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce * 2f, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void ResetJump()
    {
        canJump = true;
    }

    void GetPlayerState()
    {
        if (moveDir == Vector3.zero)
        {
            playerState = State.idle;
        }
        else if (Input.GetButton("Sprint") && isGrounded)
        {
            playerState = State.sprinting;
        }
        else if (isGrounded && moveDir != Vector3.zero && !isCrouching)
        {
            playerState = State.jogging;
        }
        else if (isCrouching)
        {
            playerState = State.crouching;
            movementSpeed = crouchSpeed;
        }
        else
        {
            playerState = State.air;
        }

        GetPlayerStateSpeed();
        GetPlayerStateAnimation();
    }

    void GetPlayerStateSpeed()
    {
        switch (playerState)
        {
            case State.idle:
                
                movementSpeed -= speedMod * Time.deltaTime;
                if (movementSpeed <= 0)
                    movementSpeed = 0f;

                break;

            case State.jogging:

                if (movementSpeed > joggingSpeed)
                {
                    movementSpeed -= speedMod * Time.deltaTime;
                    if (movementSpeed <= joggingSpeed)
                        movementSpeed = joggingSpeed;
                }

                if (movementSpeed < joggingSpeed)
                {
                    movementSpeed += speedMod * Time.deltaTime;
                    if (movementSpeed >= joggingSpeed)
                        movementSpeed = joggingSpeed;
                }

                break;

            case State.sprinting:

                movementSpeed += speedMod * Time.deltaTime;
                if (movementSpeed >= sprintingSpeed)
                    movementSpeed = sprintingSpeed;

                break;

            case State.dashing:

                break;

            case State.air:

                break;
        }
    }

    void GetPlayerStateAnimation()
    {
        switch (playerState)
        {
            case State.idle:
                anim.SetBool("isMoving", false);
                anim.SetBool("isSprinting", false);
                break;

            case State.jogging:
                if (playerState != State.sprinting)
                    anim.SetBool("isMoving", true);

                anim.SetBool("isSprinting", false);
                break;

            case State.sprinting:
                anim.SetBool("isMoving", true);
                anim.SetBool("isSprinting", true);
                break;
        }
    }

    void Crouch()
    {
        if (Input.GetButtonDown("Crouch") && !isCrouching)
        {
            isCrouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouch, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else if (Input.GetButtonDown("Crouch") && isCrouching)
        {
            isCrouching = false;
            transform.localScale = new Vector3(transform.localScale.x, unCrouch, transform.localScale.z);
            rb.AddForce(Vector3.up * 3f, ForceMode.Impulse);
        }
    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash") && isGrounded)
        {
            rb.AddForce(moveDir.normalized * dashSpeed, ForceMode.Impulse);
        }
    }







    /*  bool OnSlope()
      {
          RaycastHit hit;
          if (Physiscs.Raycast(transform.position, Vector3.down, out  hit, ))
          {
              if (hit.normal != Vector3.up)
              {
                  return true;
              }
          }

      }*/
}