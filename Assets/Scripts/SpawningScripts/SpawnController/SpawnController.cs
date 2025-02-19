using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private List<GameObject> normalEnemies = new List<GameObject>();

    [SerializeField] private List<GameObject> bossEnemies = new List<GameObject>();

    [SerializeField] private List<GameObject> dungeonSpawners = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        DetermineEnemiesForSpawner();
      BoxCollider boxCollider = this.GetComponent<BoxCollider>();
      if (boxCollider != null)
      {
          boxCollider.enabled = false;
          return;
      }
      SphereCollider sphereCollider = this.GetComponent<SphereCollider>();
      if (sphereCollider != null)
      {
          sphereCollider.enabled = false;
          return;
      }
    }

    private void Start()
    {
     // DetermineEnemiesForSpawner();
    }

    // This method allows for our teleporter to call on the parent game object for the dungeon to then get this object 
// The teleporter can then begin the determination of which area will have the boss and what areas will be normal 
    public void DetermineEnemiesForSpawner()
    {
        int randomBossLocation = Random.Range(0, dungeonSpawners.Count);
        Spawner spawnerScript = dungeonSpawners[randomBossLocation].GetComponent<Spawner>();
        spawnerScript.ChosenEnemy(bossEnemies);
        spawnerScript.HasBoss();
        for (int i = 0; i < dungeonSpawners.Count; i++)
        {
            if (i == randomBossLocation) continue;
            else
            {
                spawnerScript = dungeonSpawners[i].GetComponent<Spawner>();
                spawnerScript.ChosenEnemy(normalEnemies);
                spawnerScript.DoesNotHaveBoss();
            }
        }
        
    }
}
