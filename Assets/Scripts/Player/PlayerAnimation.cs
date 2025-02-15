using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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


    bool inCombat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OCSpeed = anim.GetFloat("OCSpeed");
        ICSpeed = anim.GetFloat("ICSpeed");
        XZMovement = anim.GetFloat("XZMovement");
        BFDir = anim.GetFloat("BF");
        LRDir = anim.GetFloat("LR");
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerStateAnimation();
        GetCombatStateAnimation();
        PlayerDeathAnimation();
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
            switch (player.playerState)
            {
                case PlayerController.PlayerState.idle:

                    OCSpeed -= Time.deltaTime * animTransSpeed;
                    if (OCSpeed <= 0)
                        OCSpeed = 0f;

                    break;
                case PlayerController.PlayerState.jogging:

                    OCSpeed += Time.deltaTime * animTransSpeed;
                    if (OCSpeed >= 1)
                        OCSpeed = 1f;

                    break;
            }
            anim.SetFloat("OCSpeed", OCSpeed);
        }
        else
        {
            switch (player.playerState)
            {
                case PlayerController.PlayerState.idle:

                    ICSpeed -= Time.deltaTime * animTransSpeed;
                    if (ICSpeed <= 0)
                        ICSpeed = 0f;

                    break;
                case PlayerController.PlayerState.jogging:

                    ICSpeed += Time.deltaTime * animTransSpeed;
                    if (ICSpeed >= 1)
                        ICSpeed = 1f;

                    break;
            }
            anim.SetFloat("ICSpeed", ICSpeed);
        }
    }

    void GetXZCombatStateAnimation()
    {
        if (inCombat)
        {
            // will calculate the direction of the player (horizontal or vertical)
            switch (player.combatState)
            {
                case PlayerController.CombatState.forward:

                    XZMovement += Time.deltaTime * animTransSpeed;
                    if (XZMovement >= 1f)
                        XZMovement = 1f;

                    break;
                case PlayerController.CombatState.backward:

                    XZMovement += Time.deltaTime * animTransSpeed;
                    if (XZMovement >= 1f)
                        XZMovement = 1f;

                    break;
                case PlayerController.CombatState.left:

                    XZMovement -= Time.deltaTime * animTransSpeed;
                    if (XZMovement <= 0f)
                        XZMovement = 0f;

                    break;
                case PlayerController.CombatState.right:

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
    }

    void GetCombatStateAnimation()
    {
        if (inCombat)
        {
            GetXZCombatStateAnimation();

            // 
            switch (player.combatState)
            {
                case PlayerController.CombatState.forward:

                    BFDir += Time.deltaTime * animTransSpeed;
                    if (BFDir >= 1f)
                        BFDir = 1f;

                    break;
                case PlayerController.CombatState.backward:

                    BFDir -= Time.deltaTime * animTransSpeed;
                    if (BFDir <= 0f)
                        BFDir = 0f;

                    break;
                case PlayerController.CombatState.right:

                    LRDir += Time.deltaTime * animTransSpeed;
                    if (LRDir >= 1f)
                        LRDir = 1f;

                    break;
                case PlayerController.CombatState.left:

                    LRDir -= Time.deltaTime * animTransSpeed;
                    if (LRDir <= 0f)
                        LRDir = 0f;

                    break;
                case PlayerController.CombatState.FR:

                    BFDir += Time.deltaTime * animTransSpeed;
                    if (BFDir >= 1f)
                        BFDir = 1f;

                    LRDir += Time.deltaTime * animTransSpeed;
                    if (LRDir >= 1f)
                        LRDir = 1f;

                    break;
                case PlayerController.CombatState.FL:

                    BFDir += Time.deltaTime * animTransSpeed;
                    if (BFDir >= 1f)
                        BFDir = 1f;

                    LRDir -= Time.deltaTime * animTransSpeed;
                    if (LRDir <= 0f)
                        LRDir = 0f;

                    break;
                case PlayerController.CombatState.BR:

                    BFDir -= Time.deltaTime * animTransSpeed;
                    if (BFDir <= 0f)
                        BFDir = 0f;

                    LRDir += Time.deltaTime * animTransSpeed;
                    if (LRDir >= 1f)
                        LRDir = 1f;

                    break;
                case PlayerController.CombatState.BL:

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

    void PlayerDeathAnimation()
    {
        if (player.HP <= 0)
        {
            anim.SetBool("Death", true);
        }
    }
}
