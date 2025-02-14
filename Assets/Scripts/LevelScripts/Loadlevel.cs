using UnityEngine;
using System.Collections.Generic;

public class LoadLevel : MonoBehaviour
{
    public static LoadLevel Instance { get; private set; }

    [SerializeField] private List<string> Levels = new List<string>();

    private List<string> remainingLevels;

    private string lastScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ResetLevelPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetLevelPool()
    {
        remainingLevels = new List<string>(Levels);
        lastScene = "";
    }

    public string GetNextLevel()
    {
        if (remainingLevels.Count == 0)
        {
            ResetLevelPool();
        }

        string nextScene;
        do
        {
            nextScene = remainingLevels[Random.Range(0, remainingLevels.Count)];
        } while (nextScene == lastScene && remainingLevels.Count > 1);

        remainingLevels.Remove(nextScene);
        lastScene = nextScene;

        return nextScene;
    }
}