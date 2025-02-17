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
            GameManager.instance.TeleportPlayer(0f, 0f, 0f);
        }
    }

}