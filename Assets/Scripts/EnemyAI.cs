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

    [SerializeField] private bool canRoam;
    [SerializeField] private int roamDistance;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private int FOV;
    [SerializeField] private Transform headPos;
    private float convertedFOV = 0;
    private Vector3 playerPos;


    // private fields
    private bool playerDetected = false;


    void Start()
    {
        convertedFOV = 1f - ((float)FOV / 100);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInRange();
    }

    void CheckPlayerInRange()
    {
        if (playerDetected && !CanSeePlayer())
        {
        }
        else
        {
            // Roam for player
        }
    }

    bool CanSeePlayer()
    {
        playerPos = AIController.GetAIController().GetPlayerPosition();
        float dotProduct = Vector3.Dot(transform.forward, (playerPos - transform.position).normalized);
        if (dotProduct > convertedFOV)
        {
            RaycastHit hit;
            if (Physics.Raycast(headPos.position, playerPos - transform.position, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    agent.SetDestination(playerPos);
                }
            }

            return true;
        }


        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("Player"))
        {
            playerPos = AIController.GetAIController().GetPlayerPosition();
            Debug.Log("Player detected");
            playerDetected = true;
        }

        Debug.Log(other.tag);
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
}