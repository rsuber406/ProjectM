using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("AI Stats")] 
    [SerializeField] private int health;

    [SerializeField] private int mana;
    [SerializeField] private bool isMelee;
    [SerializeField] private int range;
    // Replace GameObject with the scriptable spell type
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();
    [Header("Configs")]
    [SerializeField] private NavMeshAgent agent;
    private Vector3 playerPos;
    
    
    // private fields
    private bool playerDetected = false;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckPlayerInRange()
    {
        if (playerDetected)
        {
            
        }
    }

    void CanSeePlayer()
    {
        float dotProduct = Vector3.Dot(transform.forward, (playerPos - transform.position).normalized);
        if (dotProduct > 0.3f)
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("Player"))
        {
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
