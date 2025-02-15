using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] LayerMask ground;
    [SerializeField] Transform orientation;

    [Header("----- Speeds -----")]
    [SerializeField][Range(4, 10)] int joggingSpeed;
    [SerializeField][Range(7, 15)] int sprintingSpeed;
    [SerializeField][Range(2, 6)] int crouchSpeed;
    [SerializeField][Range(100, 500)] int dashSpeed;
    [SerializeField][Range(10, 20)] int speedMod;

    [Header("----- Jump ----- ")] // added jump just in case, set to 0 for no jump
    [SerializeField] float jumpForce;
    [SerializeField][Range(0, 2)] int jumpMax;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMult;


    [Header("----- Slope ----- ")] // working on slope mechanics for testing, not sure if will implement to final product
    [SerializeField] float slopeAngle;
    [SerializeField] float slopeCheck;
    RaycastHit slopeRaycast;

    [Header("----- Other Player Settings ----- ")]
    [SerializeField] float crouch;
    [SerializeField] float height;
    [SerializeField] float groundDrag;
    [SerializeField] float groundCheck;


    public float health;

    // private fields
    AttributesController attributes;
    Rigidbody rb;
    Vector3 moveDir;

    bool isGrounded;
    bool isOnSlope;
    bool canJump;
    bool isCrouching;

    int jumpCounter;

    float movementSpeed;
    float unCrouch;


    // for debugging
    float y;
    float x;
    float z;
    


    public enum PlayerState
    {
        idle
        ,jogging
        ,sprinting
        ,crouching
        ,dashing
        ,air
    }
    public PlayerState playerState;
    public enum CombatState
    {
        forward
        ,backward
        ,right
        ,left
        ,FR
        ,FL
        ,BR
        ,BL
        ,casting
        ,dodging
    }
    public CombatState combatState;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canJump = true;
        unCrouch = transform.localScale.y;

        attributes = GetComponent<AttributesController>();
    }

    // Update is called once per frame
    void Update()
    {
        // for debugging
        x = rb.linearVelocity.x;
        z = rb.linearVelocity.z;
        y = rb.linearVelocity.y;

        health = attributes.health.currentValue;

        SpeedControl();
        GetPlayerState();
        GetCombatState();

        //Jump(); // jump keybind temporarily set to "t"
        //Crouch(); // keybind set to left ctrl
        Dash(); // key bind set to space
        
        isOnSlope = OnSlope();
    }

    void FixedUpdate()
    {
        IsGrounded();
        Movement();
    }

    void Movement()
    {
        moveDir = Camera.main.transform.forward * Input.GetAxisRaw("Vertical") +
                  orientation.right * Input.GetAxisRaw("Horizontal");

        if (isOnSlope)
        {
            rb.AddForce(GetSlopeDirection() * movementSpeed * 15f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(moveDir.normalized.x * movementSpeed, rb.linearVelocity.y, moveDir.normalized.z * movementSpeed);
    
            if (isOnSlope)
            {
                if (rb.linearVelocity.y < 0)
                    rb.linearVelocity = new Vector3(moveDir.normalized.x * movementSpeed, rb.linearVelocity.y - 0.15f, moveDir.normalized.z * movementSpeed);
            }

        }

        else
            rb.linearVelocity = new Vector3(moveDir.normalized.x * movementSpeed * airMult, rb.linearVelocity.y, moveDir.normalized.z * movementSpeed * airMult);

        rb.useGravity = !isOnSlope;
    }

    void IsGrounded()
    {
        Debug.DrawRay(orientation.position, Vector3.down * height * groundCheck, Color.red);
        isGrounded = Physics.Raycast(orientation.position, Vector3.down, height * groundCheck, ground);

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
        if (isOnSlope)
        {
            if (rb.linearVelocity.magnitude > movementSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * movementSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > movementSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * movementSpeed;
                rb.linearVelocity = new Vector3(limitedVel.normalized.x * movementSpeed, rb.linearVelocity.y, limitedVel.normalized.z * movementSpeed);
            }
        }
    }

    bool OnSlope()
    {
        Debug.DrawRay(orientation.position, Vector3.down * height * slopeCheck, Color.blue);

        if (Physics.Raycast(orientation.position, Vector3.down, out slopeRaycast, height * slopeCheck))
        {
            float angle = Vector3.Angle(Vector3.up, slopeRaycast.normal);
            return angle < slopeAngle && angle!= 0;
        }
        return false;
    }

    Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeRaycast.normal).normalized;
    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash") && isGrounded)
        {
            rb.linearVelocity = new Vector3(moveDir.normalized.x * dashSpeed, moveDir.normalized.y, moveDir.normalized.z * dashSpeed);

        }
    }

    public void GetPlayerState()
    {
        if (moveDir == Vector3.zero)
        {
            playerState = PlayerState.idle;
        }
        else if (isGrounded && moveDir != Vector3.zero && !isCrouching)
        {
            playerState = PlayerState.jogging;
        }
        /*
        else if (Input.GetButton("Sprint") && isGrounded)
        {
            playerState = State.sprinting;
        }
        */
        /*
        else if (isCrouching)
        {
            playerState = State.crouching;
            movementSpeed = crouchSpeed;
        }
        */
        else
        {
            playerState = PlayerState.air;
        }

        GetPlayerStateSpeed();
    }

   

    void GetPlayerStateSpeed()
    {
        switch (playerState)
        {
            case PlayerState.idle:
                
                movementSpeed -= speedMod * Time.deltaTime;
                if (movementSpeed <= 0)
                    movementSpeed = 0f;

                break;
            case PlayerState.jogging:

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
            case PlayerState.sprinting:

                movementSpeed += speedMod * Time.deltaTime;
                if (movementSpeed >= sprintingSpeed)
                    movementSpeed = sprintingSpeed;

                break;
            case PlayerState.dashing:

                break;
            case PlayerState.air:

                break;
        }
    }

    public void GetCombatState()
    {
        if (Input.GetKey(KeyCode.W))
        {
            combatState = CombatState.forward;
            
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
                combatState = CombatState.FR;

            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
                combatState = CombatState.FL;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            combatState = CombatState.backward;

            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
                combatState = CombatState.BR;

            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
                combatState = CombatState.BL;
        }

        else if (Input.GetKey(KeyCode.A))
            combatState = CombatState.left;

        else if (Input.GetKey(KeyCode.D))
            combatState = CombatState.right;
    }

 
    IEnumerator FlashDamagePanel()
    {
        GameManager.instance.damagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.damagePanel.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        attributes.TakeDamage(amount);
        StartCoroutine(FlashDamagePanel());

    }

    

    // ----- SCRAPPED CODE ----- //

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
    void Jump()
    {
        if (Input.GetButton("Jump") && canJump && isGrounded && (jumpCounter < jumpMax))
        {
            ++jumpCounter;
            canJump = false;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void ResetJump()
    {
        canJump = true;
    }


}