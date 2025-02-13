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
    [SerializeField][Range(1, 10)] int animTransSpeed;
    
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

    //[Header("----- Slopes ----- ")] // working on slope mechanics for testing, not sure if will implement to final product
    //[SerializeField] float maxSlopeAngle;

    [Header("----- Other Player Settings ----- ")]
    [SerializeField] float crouch;
    [SerializeField] float height;
    [SerializeField] float groundDrag;
    [SerializeField] float groundRayCheck;


    // private fields
    public Vector3 moveDir;
    Rigidbody rb;

    bool isGrounded;
    bool canJump;
    bool isCrouching;
    bool inCombat;

    int jumpCounter;

    float movementSpeed;
    float unCrouch;

    // Animation Speeds
    float ICSpeed;
    float OCSpeed;
    float LFRDir;

    // for debugging
    float y;
    float x;
    float z;
    


    enum PlayerState
    {
        idle
        ,jogging
        ,sprinting
        ,crouching
        ,dashing
        ,air
    }
    PlayerState playerState;

    enum CombatState
    {
        forward
        , backward
        , right
        , left
        , FR
        , FL
        , casting
        , dodging
    }
    CombatState combatState;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
        unCrouch = transform.localScale.y;
        OCSpeed = anim.GetFloat("OCSpeed");
        ICSpeed = anim.GetFloat("ICSpeed");
        LFRDir = anim.GetFloat("LFR");

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
        GetCombatState();

        //Jump(); // jump keybind temporarily set to "t"
        //Crouch(); // keybind set to left ctrl
        Dash(); // key bind set to space
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

    void Dash()
    {
        if (Input.GetButtonDown("Dash") && isGrounded)
        {
            rb.AddForce(moveDir.normalized * dashSpeed, ForceMode.Impulse);
        }
    }

    void GetPlayerState()
    {
        if (moveDir == Vector3.zero)
        {
            playerState = PlayerState.idle;
        }
        /*
        else if (Input.GetButton("Sprint") && isGrounded)
        {
            playerState = State.sprinting;
        }
        */
        else if (isGrounded && moveDir != Vector3.zero && !isCrouching)
        {
            playerState = PlayerState.jogging;
        }
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
        GetPlayerStateAnimation();
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

    void GetPlayerStateAnimation()
    {
        if (Input.GetButtonDown("Combat"))
        {
            inCombat = !inCombat;

            if (inCombat)
                anim.SetBool("CombatMode", true);
            else
                anim.SetBool("CombatMode", false);
        }

        if (!inCombat)
        {
            switch (playerState)
            {
                case PlayerState.idle:

                    OCSpeed -= Time.deltaTime * animTransSpeed;
                    if (OCSpeed <= 0)
                        OCSpeed = 0f;

                    break;
                case PlayerState.jogging:

                    OCSpeed += Time.deltaTime * animTransSpeed;
                    if (OCSpeed >= 1)
                        OCSpeed = 1f;

                    break;
            }
            anim.SetFloat("OCSpeed", OCSpeed);
        }
        else
        {
            switch (playerState)
            {
                case PlayerState.idle:

                    ICSpeed -= Time.deltaTime * animTransSpeed;
                    if (ICSpeed <= 0)
                        ICSpeed = 0f;

                    break;
                case PlayerState.jogging:

                    ICSpeed += Time.deltaTime * animTransSpeed;
                    if (ICSpeed >= 1)
                        ICSpeed = 1f;

                    break;
            }
            anim.SetFloat("ICSpeed", ICSpeed);
        }
    }

    void GetCombatState()
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
            combatState = CombatState.backward;

        else if (Input.GetKey(KeyCode.D))
            combatState = CombatState.right;

        else if (Input.GetKey(KeyCode.A))
        {
            combatState = CombatState.left;

            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
                combatState = CombatState.FR;

            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
                combatState = CombatState.FL;
        }

        GetCombatStateAnimation();
    }

    void GetCombatStateAnimation()
    {
        if (inCombat)
        {
            switch(combatState)
            {
                case CombatState.forward:

                    if (LFRDir > 0.5f)
                    {
                        LFRDir -= Time.deltaTime * animTransSpeed;
                        if (LFRDir <= 0.5f)
                            LFRDir = 0.5f;
                    }
                    else if (LFRDir < 0.5f)
                    {
                        LFRDir += Time.deltaTime * animTransSpeed;
                        if (LFRDir >= 0.5f)
                            LFRDir = 0.5f;
                    }

                    break;
                case CombatState.right:

                    LFRDir += Time.deltaTime * animTransSpeed;
                    if (LFRDir >= 1f)
                        LFRDir = 1f;

                    break;
                case CombatState.left:

                    LFRDir -= Time.deltaTime * animTransSpeed;
                    if (LFRDir <= 0f)
                        LFRDir = 0f;

                    break;
                case CombatState.FR:

                    if (LFRDir > 0.75)
                    {
                        LFRDir -= Time.deltaTime * animTransSpeed;
                        if (LFRDir <= 0.75f)
                            LFRDir = 0.75f;
                    }
                    else if (LFRDir < 0.75f)
                    {
                        LFRDir += Time.deltaTime * animTransSpeed;
                        if (LFRDir >= 0.75f)
                            LFRDir = 0.75f;
                    }

                    break;
                case CombatState.FL:

                    if (LFRDir > 0.25)
                    {
                        LFRDir -= Time.deltaTime * animTransSpeed;
                        if (LFRDir <= 0.25f)
                            LFRDir = 0.25f;
                    }
                    else if (LFRDir < 0.25f)
                    {
                        LFRDir += Time.deltaTime * animTransSpeed;
                        if (LFRDir >= 0.25f)
                            LFRDir = 0.25f;
                    }

                    break;
            }
            anim.SetFloat("LFR", LFRDir);
        }
        
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