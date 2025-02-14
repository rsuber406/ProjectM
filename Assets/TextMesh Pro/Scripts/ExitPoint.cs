using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ExitPoint : MonoBehaviour
{
    [SerializeField] private string[] levelPool; // List of all levels. To updated by devs
    [SerializeField] private string entryPointTag = "EntryPoint"; // player spawn location.

    private static List<string> Levels;
    private static string lastScene;

    private void Start()
    {
        //call from lobby. NOT FROM THE TUTORIAL.
        if (Levels == null || Levels.Count == 0)
        {
            Levels = new List<string>(levelPool);
            lastScene = SceneManager.GetActiveScene().name;
        }
    }

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

        string nextScene;
        do
        {
            nextScene = remainingLevels[Random.Range(0, remainingLevels.Count)];
        } while (nextScene == lastScene && remainingLevels.Count > 1);

        // Remove the chosen level from the pool
        remainingLevels.Remove(nextScene);
        lastScene = nextScene;

        SceneManager.LoadScene(nextScene);
    }
}