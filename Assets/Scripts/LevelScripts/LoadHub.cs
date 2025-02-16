using UnityEngine;
public class loadHub : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //MainSceneLogic.MSInstance.LoadHub();
        }
    }
}