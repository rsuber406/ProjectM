using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerStateController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    
    public enum PlayerState
    {
        idle
        , jogging
        , sprinting
        , crouching
        , dodging
        , air
    }
    public PlayerState playerState;
    public enum CombatState
    {
        forward
        , backward
        , right
        , left
        , FR
        , FL
        , BR
        , BL
    }
    public CombatState combatState;

    public enum DodgeState
    {
        forward
        , backward
        , right
        , left
        , FR
        , FL
        , BR
        , BL
    }
    public DodgeState dodgeState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerState();
        GetCombatState();
        GetDodgeState();
    }
    public void GetPlayerState()
    {
        if (player.moveDir == Vector3.zero)
            playerState = PlayerState.idle;

        else if (player.isGrounded && player.moveDir != Vector3.zero && !player.isDodging)
            playerState = PlayerState.jogging;

        else if (player.isDodging)
            playerState = PlayerState.dodging;

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
        }
        */
        else
            playerState = PlayerState.air;

        GetPlayerStateSpeed();
    }



    void GetPlayerStateSpeed()
    {
        switch (playerState)
        {
            case PlayerState.idle:

                player.movementSpeed -= player.speedMod * Time.deltaTime;
                if (player.movementSpeed <= 0)
                    player.movementSpeed = 0f;

                break;
            case PlayerState.jogging:

                if (player.movementSpeed > player.joggingSpeed)
                {
                    player.movementSpeed -= player.speedMod * Time.deltaTime;
                    if (player.movementSpeed <= player.joggingSpeed)
                        player.movementSpeed = player.joggingSpeed;
                }

                if (player.movementSpeed < player.joggingSpeed)
                {
                    player.movementSpeed += player.speedMod * Time.deltaTime;
                    if (player.movementSpeed >= player.joggingSpeed)
                        player.movementSpeed = player.joggingSpeed;
                }

                break;
            case PlayerState.sprinting:

                player.movementSpeed += player.speedMod * Time.deltaTime;
                if (player.movementSpeed >= player.sprintingSpeed)
                    player.movementSpeed = player.sprintingSpeed;

                break;
            case PlayerState.dodging:

                player.movementSpeed = player.dodgeSpeed;

                break;
            case PlayerState.air:

                break;
        }
    }

    public void GetCombatState()
    {
        if (Input.GetButtonDown("Combat") && !player.inCombat && Time.deltaTime != 0)
            player.inCombat = true;

        else if (Input.GetButtonDown("Combat") && player.inCombat && Time.deltaTime != 0)
            player.inCombat = false;


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


    void GetDodgeState()
    {
        switch (combatState)
        {
            case CombatState.forward:

                dodgeState = DodgeState.forward;

                break;
            case CombatState.backward:

                dodgeState = DodgeState.backward;

                break;
            case CombatState.right:

                dodgeState = DodgeState.right;

                break;
            case CombatState.left:

                dodgeState = DodgeState.left;

                break;
            case CombatState.FL:

                dodgeState = DodgeState.FL;

                break;
            case CombatState.FR:

                dodgeState = DodgeState.FR;

                break;
            case CombatState.BL:

                dodgeState = DodgeState.BL;

                break;
            case CombatState.BR:

                dodgeState = DodgeState.BR;

                break;
        }
    }
}
