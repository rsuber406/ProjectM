using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = System.Random;

public class MainSceneLogic : MonoBehaviour
{
    public static MainSceneLogic MSInstance;

    [Header("---Main Menu Objects---")]
    [SerializeField] private GameObject loadScreen;
    [SerializeField] private GameObject MenuActivatebles;

    [Header("---World 1 Levels---")]
    [SerializeField] private string _Hub = "Hub";
    public string[] DynamicMaps = { "Map1", "Map2"};
    [SerializeField] private string _tutScene = "TutScene";
    [SerializeField] private string _bossScene = "BossRoom";

    [Header("---MainStage Player Controller---")]
    [SerializeField] private GameObject PlayerActivateables;

    public string currLvl;
    public GameObject _Hublvl;

    public int mapnum = 0;

    private void Start()
    {
        MSInstance = this;

        Time.timeScale = 0;
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;

        SceneManager.LoadScene(_Hub, LoadSceneMode.Additive);

        //Lock Cursor so the player can make a selection
        //Load Hub and move it for use later
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

        currLvl = _tutScene;
        SceneManager.LoadSceneAsync(currLvl, LoadSceneMode.Additive);
        //LoadScenes.Add(SceneManager.LoadSceneAsync(_DynamicScenes, LoadSceneMode.Additive));
        PlayerActivateables.SetActive (true);
    }

    public void HideMenu()
    {
        MenuActivatebles.SetActive (false);
    }

    public void loadLevel()
    {
        //unload any active scene.
        if (!string.IsNullOrEmpty(currLvl))
        {
            SceneManager.UnloadSceneAsync(currLvl);
        }
        if (mapnum >= DynamicMaps.Count())
        {
            currLvl = _bossScene;
            SceneManager.LoadSceneAsync(currLvl, LoadSceneMode.Additive);
            return;
        }
        else
        {
            currLvl = DynamicMaps[mapnum];
            mapnum++;
            SceneManager.LoadSceneAsync(currLvl, LoadSceneMode.Additive);
        }
        // Remove the selected map
        //DynamicMaps.RemoveAt(0);

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
