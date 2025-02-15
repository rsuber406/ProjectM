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
    float LFRDir;

    bool inCombat;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OCSpeed = anim.GetFloat("OCSpeed");
        ICSpeed = anim.GetFloat("ICSpeed");
        LFRDir = anim.GetFloat("LFR");
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerStateAnimation();
        GetCombatStateAnimation();
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
            anim.SetFloat("CombatMovement", ICSpeed);
        }
    }

    void GetCombatStateAnimation()
    {
        if (inCombat)
        {
            switch (player.combatState)
            {
                case PlayerController.CombatState.forward:

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
                case PlayerController.CombatState.right:

                    LFRDir += Time.deltaTime * animTransSpeed;
                    if (LFRDir >= 1f)
                        LFRDir = 1f;

                    break;
                case PlayerController.CombatState.left:

                    LFRDir -= Time.deltaTime * animTransSpeed;
                    if (LFRDir <= 0f)
                        LFRDir = 0f;

                    break;
                case PlayerController.CombatState.FR:

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
                case PlayerController.CombatState.FL:

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
}
