using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] LayerMask ground;
    [SerializeField] Transform orientation;

    [Header("----- Speeds -----")]
    [SerializeField][Range(4, 10)] public int joggingSpeed;
    [SerializeField][Range(7, 15)] public int sprintingSpeed;  // disabled
    [SerializeField][Range(2, 6)] public int crouchSpeed;      // disabled
    [SerializeField][Range(5, 15)] public int dodgeSpeed;
    [SerializeField][Range(10, 20)] public int speedMod;

    [Header("----- Jump ----- ")] // added jump just in case, set to 0 for no jump
    [SerializeField] float jumpForce;
    [SerializeField][Range(0, 2)] int jumpMax;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMult;

    [Header("----- Dodge -----")]
    [SerializeField] float dodgeForce;
    [SerializeField] float dodgeDur;
    [SerializeField] float dodgeCd;

    [Header("----- Slope ----- ")] // working on slope mechanics for testing, not sure if will implement to final product
    [SerializeField] float maxSlopeAngle;
    [SerializeField] float slopeCheck;

    [Header("----- Ground -----")]
    [SerializeField] float groundDrag;
    [SerializeField] float groundCheck;

    [Header("----- Other Player Settings ----- ")]
    [SerializeField] float crouch;
    [SerializeField] float height;

    public PlayerStateController stateController;
    public Vector3 moveDir;
    
    public bool inCombat;
    public bool isGrounded;
    public bool isDodging;

    public float movementSpeed;
    public float HP;
    public float mana;
    public float dodgeCdTimer;

    // private fields
    AttributesController attributes;
    Rigidbody rb;
    Vector3 dodgeDelay;
    RaycastHit slopeRaycast;

    bool isOnSlope;
    bool canJump;
    bool isCrouching;

    int jumpCounter;

    float unCrouch;

    // for debugging
    float y;
    float x;
    float z;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        attributes = GetComponent<AttributesController>();
        stateController = GetComponent<PlayerStateController>();

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

        HP = attributes.health.currentValue;
        mana = attributes.mana.currentValue;

        isOnSlope = OnSlope();

        if (dodgeCdTimer > 0)
            dodgeCdTimer -= Time.deltaTime;

        UpdatePlayerUI();
        SpeedControl();
        
        //Jump(); // jump keybind temporarily set to "t"
        //Crouch(); // keybind set to left ctrl
        
    }

    void FixedUpdate()
    {
        IsGrounded();
        Movement();

        if (Input.GetButtonDown("Dodge") && inCombat)
            Dodge(); // key bind set to space
    }

    void Movement()
    {
        moveDir = Camera.main.transform.forward * Input.GetAxisRaw("Vertical") +
                  orientation.right * Input.GetAxisRaw("Horizontal");

        if (isOnSlope)
        {
            rb.AddForce(GetSlopeDirection() * movementSpeed * speedMod, ForceMode.Force);

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

        if (stateController.playerState != PlayerStateController.PlayerState.dodging && stateController.playerState != PlayerStateController.PlayerState.air)
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

        if (Physics.Raycast(transform.position, Vector3.down, out slopeRaycast, height * slopeCheck))
        {
            float angle = Vector3.Angle(Vector3.up, slopeRaycast.normal);
            return angle < maxSlopeAngle && angle!= 0;
        }
        return false;
    }

    Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeRaycast.normal).normalized;
    }

    void Dodge()
    {
      
        if (dodgeCdTimer > 0)
            return;
        else
            dodgeCdTimer = dodgeCd;

        isDodging = true;

        Vector3 dodge = new Vector3(moveDir.normalized.x * dodgeForce, moveDir.normalized.y, moveDir.normalized.z * dodgeForce);
        dodgeDelay = dodge;
        rb.AddForce(dodgeDelay);

        //Invoke(nameof(DodgeDelay), 0.025f);
        Invoke(nameof(ResetDodge), dodgeDur);
        //rb.linearVelocity = new Vector3(moveDir.normalized.x * dashSpeed, moveDir.normalized.y, moveDir.normalized.z * dashSpeed);
    }

    void DodgeDelay()
    {
    }

    void ResetDodge()
    {
        isDodging = false;
    }

    void UpdatePlayerUI()
    {
        GameManager.instance.healthBar.fillAmount = (float)HP / attributes.health.maxValue;
        GameManager.instance.manaBar.fillAmount = (float)mana / attributes.mana.maxValue;
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