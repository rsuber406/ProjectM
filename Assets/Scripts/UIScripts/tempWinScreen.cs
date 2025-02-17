using UnityEngine;
using System.Collections.Generic;

public class tempWinScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.tmpVictoryScreen();
        }
    }
}
