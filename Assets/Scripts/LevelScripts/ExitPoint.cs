using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPoint : MonoBehaviour
{
    [SerializeField] private string[] levelPool; // List of all levels. To updated by devs
    [SerializeField] private string entryPointTag = "EntryPoint"; // player spawn location.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        if (levelPool.Length == 0)
        {
            Debug.LogWarning("No levels assigned in ExitPoint!");
            return;
            //TODO: This is a temp fix. if there are no levels to load, load the Boss Level.
        }

        string nextScene = levelPool[Random.Range(0, levelPool.Length)];
        SceneManager.LoadScene(nextScene);
    }
}