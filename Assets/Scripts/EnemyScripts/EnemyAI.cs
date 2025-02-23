using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour, IDamage
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("AI Stats")] [SerializeField] private int health;

    [SerializeField] private int mana;

    [SerializeField] protected int range;

    // Replace GameObject with the scriptable spell type
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();
    [Header("Configs")] [SerializeField] private bool isMelee;
    [SerializeField] private int rotationSpeed;
    [SerializeField] private bool canRoam;
    [SerializeField] private int roamDistance;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private int FOV;
    [SerializeField] protected Transform headPos;

    [Header("Sound configs")] [SerializeField]
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] footsteps;
    [SerializeField] protected AudioClip[] spellSounds;
    [SerializeField] protected AudioClip[] meleeSounds;
    [SerializeField] protected float footstepVolume;
    [SerializeField] protected float spellVolume;
    [SerializeField] protected float meleeAttack;
    protected float convertedFOV = 0;
    protected Vector3 playerPos;
    protected bool isAttacking;
    protected bool playerDetected = false;
    protected float agentStoppingDistanceOrig;
    protected bool [] playSounds = new bool[3];

    private List<Color> originalColors = new List<Color>();

    protected virtual void Start()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    originalColors.Add(renderer.material.color);
                }
            }
        }

        convertedFOV = 1f - ((float)FOV / 100f);
        for (int i = 0; i < playSounds.Length; i++)
        {
            playSounds[i] = false;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckPlayerInRange();
    }

    protected void CheckPlayerInRange()
    {
        if (playerDetected && !CanSeePlayer())
        {
        }
        else
        {
            // Roam for player
        }

        if (!playerDetected)
        {
            AIController.GetAIController().RemoveFromAttackQue();
            isAttacking = false;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("Player"))
        {
            playerPos = AIController.GetAIController().GetPlayerPosition();
            Debug.Log("player detected");
            playerDetected = true;
        }
    }

    bool CanSeePlayer()
    {
        playerPos = AIController.GetAIController().GetPlayerPosition();
        float dotProduct = Vector3.Dot(transform.forward, (playerPos - transform.position).normalized);

        if (dotProduct > convertedFOV)
        {
            RaycastHit hit;
            
            if (Vector3.Distance(transform.position, playerPos) > agent.stoppingDistance && !playSounds[(int) PlayNumber.Footstep])
            {
                StartCoroutine(PlaySound(footsteps, footstepVolume, PlayNumber.Footstep));
            }
            Debug.DrawRay(headPos.position, (playerPos - headPos.position), Color.red);
            if (Physics.Raycast(headPos.position, playerPos - headPos.position, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (!isAttacking)
                        agent.SetDestination(playerPos);

                    if (Vector3.Distance(transform.position, playerPos) < agent.stoppingDistance)
                    {
                        // Make the AI face the target
                        FaceTarget(ref playerPos);
                    }

                    if (!isAttacking)
                        AttackPlayer();
                }
            }

            return true;
        }


        return false;
    }


    void FaceTarget(ref Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

    public void TakeDamage(int amount, DamageSourceType type)
    {
        health -= amount;
        StartCoroutine(FlashDamage());
            agent.SetDestination(playerPos);
        if (type == DamageSourceType.Player)
        {
        }
        
        if (health <= 0)
            StartCoroutine(OnDeath());
    }

    protected virtual void AttackPlayer()
    {
    }

    protected virtual IEnumerator OnDeath()
    {
        yield return null;
    }

    private IEnumerator FlashDamage()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    renderer.material.color = Color.red;
                }
            }
        }

        yield return new WaitForSeconds(0.1f);
        int counter = 0;
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_Color") && counter < originalColors.Count)
                {
                    renderer.material.color = originalColors[counter];
                }
            }
            counter++;

        }
    }
    
    protected IEnumerator PlaySound(AudioClip[] sounds, float volume, PlayNumber sound, bool isLightning = false)
    {
        playSounds[(int)sound] = true;
        AudioClip randomClip = isLightning ? sounds[2] : sounds[Random.Range(0, sounds.Length)];
        audioSource.PlayOneShot(randomClip, volume);
        yield return new WaitForSeconds(randomClip.length);
        playSounds[(int)sound] = false;
    }
}

public enum PlayNumber
{
    Footstep,
    Spellcast,
    MeleeAttack
}