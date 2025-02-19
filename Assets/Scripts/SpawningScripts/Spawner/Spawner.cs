using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private List<Transform> spawnPoints;

    private List<GameObject> enemyToSpawn;

    private bool containsBoss = false;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        
        SpawnEnemy();
        
    }
    public void ChosenEnemy(List<GameObject> enemies)
    {
        enemyToSpawn = enemies;
        
    }

    public void HasBoss()
    {
        containsBoss = true;
    }

    public void DoesNotHaveBoss()
    {
        containsBoss = false;
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (containsBoss)
            {
                Instantiate(enemyToSpawn[0], spawnPoints[i].position, spawnPoints[i].rotation);
                break;
            }
            else
            {
                int randomEnemy = Random.Range(0, enemyToSpawn.Count);
                enemyToSpawn[randomEnemy].transform.position = spawnPoints[i].position;
                // Replace this with object pooling, get it to work and go from there
                Instantiate(enemyToSpawn[randomEnemy], spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
    }
    
    
}
