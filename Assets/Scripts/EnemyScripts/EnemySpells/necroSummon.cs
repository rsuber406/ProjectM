using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class necroSummon : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int destroyTime;
    [SerializeField] private ParticleSystem effects;
    [SerializeField] private GameObject summonCircle;
    [SerializeField] private List<GameObject> enemies;
    private Rigidbody rigidBody;

    private bool isSpawning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        rigidBody.linearVelocity = Vector3.forward * speed * Time.deltaTime - (Vector3.up * 150 * Time.deltaTime);
        
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.isTrigger) return;
    //
    //     if (other.CompareTag("Ground"))
    //     {
    //         Instantiate(effects, , Quaternion.identity);
    //         summonCircle.transform.position = other.transform.position;
    //         summonCircle.SetActive(true);
    //         SpawnEnemies();
    //     }
    //     
    // }
    private void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.CompareTag("Ground"))
        {
            TrailRenderer trailRenderer = gameObject.GetComponent<TrailRenderer>();
            trailRenderer.enabled = false;
            Instantiate(effects, collision.contacts[0].point, Quaternion.identity);
            summonCircle.transform.position = collision.contacts[0].point + Vector3.up;
            summonCircle.SetActive(true);
            if(!isSpawning)
            StartCoroutine(SpawnEnemies(collision.contacts[0].point));
        }
    }

    private IEnumerator SpawnEnemies(Vector3 spawnPosition)
    {
        isSpawning = true;
       
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < enemies.Count; i++)
        {
            Instantiate(enemies[i], spawnPosition, Quaternion.identity);
        }
        summonCircle.SetActive(false);
        
        Destroy(gameObject);
    }
    
}
