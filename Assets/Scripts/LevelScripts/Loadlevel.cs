using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class loadHub : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //ResetLevelPool();
            LoadScene();
        }
    }
    public void ResetLevelPool()
    {

    }
    public void LoadScene()
    {
        SceneManager.LoadScene("Hub");
    }

}