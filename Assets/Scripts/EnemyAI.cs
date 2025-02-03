using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("AI Stats")] 
    [SerializeField] private int health;

    [SerializeField] private int mana;
    [SerializeField] private int range;
    // Replace GameObject with the scriptable spell type
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();
    [Header("Configs")]
    [SerializeField] private bool isMelee;

    [SerializeField] private bool canRoam;
    [SerializeField] private int roamDistance;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private int FOV;
    private float convertedFOV = 0;
    private Vector3 playerPos;
    
    
    // private fields
    private bool playerDetected = false;
    
    
    void Start()
    {
        playerPos = AIController.GetAIController().GetPlayerPosition();
        convertedFOV = (float)FOV / 100f;
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
        
    }

    bool CanSeePlayer()
    {
        float dotProduct = Vector3.Dot(transform.forward, (playerPos - transform.position).normalized);
        if (dotProduct > 0.3f)
        {
            // Needs more logic
            Debug.Log(dotProduct);
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("Player"))
        {
            playerPos = AIController.GetAIController().UpdatePlayerPosition();
            playerDetected = true;
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
