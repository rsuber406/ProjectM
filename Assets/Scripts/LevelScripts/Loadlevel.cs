using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MainSceneLogic.MSInstance.loadLevel();
          GameObject[] enemies =  GameObject.FindGameObjectsWithTag("Enemy");
          if (enemies != null)
          {
              for (int i = 0; i < enemies.Length; i++)
              {
                  Destroy(enemies[i]);
              }
          }
            GameManager.instance.TeleportPlayer(0f, 0f, 0f);
        }
    }

}