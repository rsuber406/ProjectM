using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadHub : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Call MainScene and load a random level
            //if there are no more levels available, load the boss scene
        }
    }

}