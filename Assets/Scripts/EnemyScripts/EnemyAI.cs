using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("AI Stats")] [SerializeField] private int health;

    [SerializeField] private int mana;

    [SerializeField] private int range;

    // Replace GameObject with the scriptable spell type
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();
    [Header("Configs")] [SerializeField] private bool isMelee;
    [SerializeField] private int rotationSpeed;
    [SerializeField] private bool canRoam;
    [SerializeField] private int roamDistance;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private int FOV;
    [SerializeField] private Transform headPos;
    protected float convertedFOV = 0;
    protected Vector3 playerPos;
    protected bool isAttacking;
    protected bool playerDetected = false;
    protected float agentStoppingDistanceOrig;

    void Start()
    {
        convertedFOV = 1f - ((float)FOV / 100f);
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
            Debug.DrawRay(headPos.position, (playerPos - headPos.position), Color.red);
            if (Physics.Raycast(headPos.position, playerPos - headPos.position, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if(!isAttacking)
                    agent.SetDestination(playerPos);

                    if (Vector3.Distance(transform.position, playerPos) < agent.stoppingDistance)
                    {
                        // Make the AI face the target
                        FaceTarget(ref playerPos);
                    }
                    if(!isAttacking)
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

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void AttackPlayer()
    {
        
    }
    
   
}