using System;
using System.Collections;
using UnityEngine;


public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Animator anim;
    [SerializeField][Range(1, 10)] int animTransSpeed;
    
    // Animation Speeds
    float ICSpeed;
    float OCSpeed;
    float XZMovement;
    float BFDir;
    float LRDir;
    
    public event Action onActionModeEnabled;
    public event Action onActionModeDisabled;

    float DodgeXZ;
    float DodgeX;
    float DodgeZ;
    float OR;   //override layer speed


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OCSpeed = anim.GetFloat("OCSpeed");
        ICSpeed = anim.GetFloat("ICSpeed");
        XZMovement = anim.GetFloat("XZMovement");
        BFDir = anim.GetFloat("BF");
        LRDir = anim.GetFloat("LR");
        DodgeXZ = anim.GetFloat("DodgeXZ");
        DodgeX = anim.GetFloat("DodgeX");
        DodgeZ = anim.GetFloat("DodgeZ");

        OR = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerStateAnimation();
        GetCombatStateAnimation();
        GetDodgeStateAnimation();
        
    }

   public IEnumerator PlayerDeathAnimation()
    {
       anim.SetBool("Death", true);
       yield return new WaitForSeconds(1.5f);
       GameManager.GetInstance().LossMenu();
       
    }

    void GetPlayerStateAnimation()
    {
        if (player.inCombat) {
            anim.SetBool("CombatMode", true);
            onActionModeEnabled?.Invoke();
        }
            
        else {
            anim.SetBool("CombatMode", false);
            onActionModeDisabled?.Invoke();
        }

        if (!player.inCombat)
        {
            switch (player.stateController.playerState)
            {
                case PlayerStateController.PlayerState.idle:

                    OCSpeed -= Time.deltaTime * animTransSpeed;
                    if (OCSpeed <= 0)
                        OCSpeed = 0f;

                    break;
                case PlayerStateController.PlayerState.jogging:

                    OCSpeed += Time.deltaTime * animTransSpeed;
                    if (OCSpeed >= 1)
                        OCSpeed = 1f;

                    break;
            }
            anim.SetFloat("OCSpeed", OCSpeed);
        }
        else
        {
            switch (player.stateController.playerState)
            {
                case PlayerStateController.PlayerState.idle:

                    ICSpeed -= Time.deltaTime * animTransSpeed;
                    if (ICSpeed <= 0)
                        ICSpeed = 0f;

                    break;
                case PlayerStateController.PlayerState.jogging:

                    ICSpeed += Time.deltaTime * animTransSpeed;
                    if (ICSpeed >= 1)
                        ICSpeed = 1f;

                    break;
                case PlayerStateController.PlayerState.dodging:

                    anim.SetBool("isDodging", true);
                    DodgeLayerOverride();


                    break;
            }
            anim.SetFloat("ICSpeed", ICSpeed);
            
            if (player.dodgeCdTimer < 0)
            {
                anim.SetBool("isDodging", false);
                BaseLayerOverride();
            }
        }
    }

    void GetXZCombatStateAnimation()
    {
        // will calculate the direction of the player (horizontal or vertical)
        switch (player.stateController.combatState)
        {
            case PlayerStateController.CombatState.forward:

                XZMovement += Time.deltaTime * animTransSpeed;
                if (XZMovement >= 1f)
                    XZMovement = 1f;

                break;
            case PlayerStateController.CombatState.backward:

                XZMovement += Time.deltaTime * animTransSpeed;
                if (XZMovement >= 1f)
                    XZMovement = 1f;

                break;
            case PlayerStateController.CombatState.left:

                XZMovement -= Time.deltaTime * animTransSpeed;
                if (XZMovement <= 0f)
                    XZMovement = 0f;

                break;
            case PlayerStateController.CombatState.right:

                XZMovement -= Time.deltaTime * animTransSpeed;
                if (XZMovement <= 0f)
                    XZMovement = 0f;

                break;
            default:    // if player it moving diagonally

                if (XZMovement > 0.5f)
                {
                    XZMovement -= Time.deltaTime * animTransSpeed;
                    if (XZMovement <= 0.5f)
                        XZMovement = 0.5f;
                }
                else if (XZMovement < 0.5f)
                {
                    XZMovement += Time.deltaTime * animTransSpeed;
                    if (XZMovement >= 0.5f)
                        XZMovement = 0.5f;
                }

                break;
        }
        anim.SetFloat("XZMovement", XZMovement);
    }

    void GetCombatStateAnimation()
    {
        if (player.inCombat)
        {
            GetXZCombatStateAnimation();

            switch (player.stateController.combatState)
            {
                case    PlayerStateController.CombatState.forward:

                    BFDir += Time.deltaTime * animTransSpeed;
                    if (BFDir >= 1f)
                        BFDir = 1f;

                    break;
                case PlayerStateController.CombatState.backward:

                    BFDir -= Time.deltaTime * animTransSpeed;
                    if (BFDir <= 0f)
                        BFDir = 0f;

                    break;
                case PlayerStateController.CombatState.right:

                    LRDir += Time.deltaTime * animTransSpeed;
                    if (LRDir >= 1f)
                        LRDir = 1f;

                    break;
                case PlayerStateController.CombatState.left:

                    LRDir -= Time.deltaTime * animTransSpeed;
                    if (LRDir <= 0f)
                        LRDir = 0f;

                    break;
                case PlayerStateController.CombatState.FR:

                    BFDir += Time.deltaTime * animTransSpeed;
                    if (BFDir >= 1f)
                        BFDir = 1f;

                    LRDir += Time.deltaTime * animTransSpeed;
                    if (LRDir >= 1f)
                        LRDir = 1f;

                    break;
                case PlayerStateController.CombatState.FL:

                    BFDir += Time.deltaTime * animTransSpeed;
                    if (BFDir >= 1f)
                        BFDir = 1f;

                    LRDir -= Time.deltaTime * animTransSpeed;
                    if (LRDir <= 0f)
                        LRDir = 0f;

                    break;
                case PlayerStateController.CombatState.BR:

                    BFDir -= Time.deltaTime * animTransSpeed;
                    if (BFDir <= 0f)
                        BFDir = 0f;

                    LRDir += Time.deltaTime * animTransSpeed;
                    if (LRDir >= 1f)
                        LRDir = 1f;

                    break;
                case PlayerStateController.CombatState.BL:

                    BFDir -= Time.deltaTime * animTransSpeed;
                    if (BFDir <= 0f)
                        BFDir = 0f;

                    LRDir -= Time.deltaTime * animTransSpeed;
                    if (LRDir <= 0f)
                        LRDir = 0f;

                    break;
            }

            anim.SetFloat("BF", BFDir);
            anim.SetFloat("LR", LRDir);
        }
    }

    void GetXZDodgeStateAnimation()
    {
        switch (player.stateController.dodgeState)
        {
            case PlayerStateController.DodgeState.forward:

                DodgeXZ += Time.deltaTime * animTransSpeed;
                if (DodgeXZ >= 1f)
                    DodgeXZ = 1f;

                break;
            case PlayerStateController.DodgeState.backward:

                DodgeXZ += Time.deltaTime * animTransSpeed;
                if (DodgeXZ >= 1f)
                    DodgeXZ = 1f;

                break;
            case PlayerStateController.DodgeState.left:

                DodgeXZ -= Time.deltaTime * animTransSpeed;
                if (DodgeXZ <= 0f)
                    DodgeXZ = 0f;

                break;
            case PlayerStateController.DodgeState.right:

                DodgeXZ -= Time.deltaTime * animTransSpeed;
                if (DodgeXZ <= 0f)
                    DodgeXZ = 0f;

                break;
            default:    // if player it dodging diagonally

                if (DodgeXZ > 0.5f)
                {
                    DodgeXZ -= Time.deltaTime * animTransSpeed;
                    if (DodgeXZ <= 0.5f)
                        DodgeXZ = 0.5f;
                }
                else if (DodgeXZ < 0.5f)
                {
                    DodgeXZ += Time.deltaTime * animTransSpeed;
                    if (DodgeXZ >= 0.5f)
                        DodgeXZ = 0.5f;
                }

                break;
        }
        anim.SetFloat("DodgeXZ", DodgeXZ);
    }

    void GetDodgeStateAnimation()
    {
        GetXZDodgeStateAnimation();

        switch (player.stateController.dodgeState)
        {
            case PlayerStateController.DodgeState.forward:

                DodgeZ += Time.deltaTime * animTransSpeed;
                if (DodgeZ >= 1f)
                    DodgeZ = 1f;

                break;
            case PlayerStateController.DodgeState.backward:

                DodgeZ -= Time.deltaTime * animTransSpeed;
                if (DodgeZ <= 0f)
                    DodgeZ = 0f;

                break;
            case PlayerStateController.DodgeState.right:

                DodgeX += Time.deltaTime * animTransSpeed;
                if (DodgeX >= 1f)
                    DodgeX = 1f;

                break;
            case PlayerStateController.DodgeState.left:

                DodgeX -= Time.deltaTime * animTransSpeed;
                if (DodgeX <= 0f)
                    DodgeX = 0f;

                break;
            case PlayerStateController.DodgeState.FR:

                DodgeZ += Time.deltaTime * animTransSpeed;
                if (DodgeZ >= 1f)
                    DodgeZ = 1f;

                DodgeX += Time.deltaTime * animTransSpeed;
                if (DodgeX >= 1f)
                    DodgeX = 1f;

                break;
            case PlayerStateController.DodgeState.FL:

                DodgeZ += Time.deltaTime * animTransSpeed;
                if (DodgeZ >= 1f)
                    DodgeZ = 1f;

                DodgeX -= Time.deltaTime * animTransSpeed;
                if (DodgeX <= 0f)
                    DodgeX = 0f;

                break;
            case PlayerStateController.DodgeState.BR:

                DodgeZ -= Time.deltaTime * animTransSpeed;
                if (DodgeZ <= 0f)
                    DodgeZ = 0f;

                DodgeX += Time.deltaTime * animTransSpeed;
                if (DodgeX >= 1f)
                    DodgeX = 1f;

                break;
            case PlayerStateController.DodgeState.BL:

                DodgeZ -= Time.deltaTime * animTransSpeed;
                if (DodgeZ <= 0f)
                    DodgeZ = 0f;

                DodgeX -= Time.deltaTime * animTransSpeed;
                if (DodgeX <= 0f)
                    DodgeX = 0f;

                break;
        }
        anim.SetFloat("DodgeZ", DodgeZ);
        anim.SetFloat("DodgeX", DodgeX);
    }

    void DodgeLayerOverride()
    {
        OR += 13f * Time.deltaTime;
        if (OR > 1)
            OR = 1f;
        anim.SetLayerWeight(1, OR);
    }

    void BaseLayerOverride()
    {
        OR -= 9f * Time.deltaTime;
        if (OR < 0)
            OR = 0f;
        anim.SetLayerWeight(1, OR);
    }

    public void ResetPlayerDeath()
    {
        anim.SetBool("Death", false);
        anim.SetTrigger("ResetDeath");
        GameManager.GetInstance().removeLossMenu();
    }

    public void PlayAbilityByTriggerName(string name)
    {
        anim.SetTrigger(name);
    }
}
