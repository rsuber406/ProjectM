using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainSceneLogic : MonoBehaviour
{
    public static MainSceneLogic MSInstance { get; private set; }

    [Header("---Main Menu Objects---")]
    [SerializeField] private GameObject loadScreen;
    [SerializeField] private GameObject MenuActivatebles;

    [Header("---World 1 Levels---")]
    [SerializeField] private string _Hub = "Hub";
    [SerializeField] private string[] _DynamicScenes;
    [SerializeField] private string _tutScene = "TutScene";
    [SerializeField] private string _bossScene = "BossRoom";

    [Header("---MainStage Player Controller---")]
    [SerializeField] private GameObject PlayerActivateables;

    private List<AsyncOperation> LoadScenes = new List<AsyncOperation> ();

    private string activeLevel;

    private void Start()
    {
        Time.timeScale = 0;
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        //Lock Cursor so the player can make a selection

        PlayerActivateables.SetActive(false);
        loadScreen.SetActive(false);

    }

    public void PlayGame()
    {
        HideMenu();

        //Player Has made a selection. Hide menu, load first scene and lock the cursor
        //Enable player Hud Here


        Time.timeScale = 1;
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;


        LoadScenes.Add(SceneManager.LoadSceneAsync(_Hub, LoadSceneMode.Additive));
        //LoadScenes.Add(SceneManager.LoadSceneAsync(_DynamicScenes, LoadSceneMode.Additive));
        PlayerActivateables.SetActive (true);
    }

    public void HideMenu()
    {
        MenuActivatebles.SetActive (false);
    }

    public void loadHub()
    {
        //unload current scene and move hub prefab to 14.003, 0.8610, 2.8657
        SceneManager.UnloadSceneAsync(activeLevel);
        //_Hub.transform.position += new Vector3(0, 0, -100);
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
