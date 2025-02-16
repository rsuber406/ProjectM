using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour
{
    [Header("---Main Menu Objects---")]
    [SerializeField] private GameObject loadScreen;
    [SerializeField] private GameObject MenuActivatebles;

    [Header("---World 1 Levels---")]
    [SerializeField] private string _Hub = "PersistentLevel";
    [SerializeField] private string _DynamicScenes = "Room1";

    [Header("---MainStage Player Controller---")]
    [SerializeField] private GameObject PlayerActivateables;

    private List<AsyncOperation> LoadScenes = new List<AsyncOperation> ();

    private void Start()
    {
        PlayerActivateables.SetActive(false);
        loadScreen.SetActive(false);

        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.Confined;
        //Lock Cursor so the player can make a selection

    }

    public void PlayGame()
    {
        HideMenu();

        //Player Has made a selection. Hide menu, load first scene and lock the cursor

        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        LoadScenes.Add(SceneManager.LoadSceneAsync(_Hub));
        LoadScenes.Add(SceneManager.LoadSceneAsync(_DynamicScenes, LoadSceneMode.Additive));
        PlayerActivateables.SetActive (true);
    }

    public void HideMenu()
    {
        MenuActivatebles.SetActive (false);
    }

    private IEnumerator pauseBar()
    {
        for (int i = 0; i < LoadScenes.Count; i++)
        {
            while (!LoadScenes[i].isDone)
            {
                loadScreen.SetActive(true);
                //Hold on a black still frame.
                yield return null;
            }
        }
        loadScreen.SetActive(false);
        //hide black still
    }

    public void Quitgame()
    {   
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
