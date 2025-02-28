using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Random = System.Random;

public class MainSceneLogic : MonoBehaviour
{
    public static MainSceneLogic MSInstance;

    [Header("---Main Menu Objects---")] 
    [SerializeField] public GameObject loadScreen;
    [SerializeField] public GameObject MMCamera;

    [SerializeField] private GameObject MenuActivateables;
    [SerializeField] private GameObject PauseMenuActivateables;
    [SerializeField] private GameObject SettingsActivateables;
    [SerializeField] public GameObject CreditsActivateables;

    [SerializeField] private GameObject returnBtn;
    [SerializeField] private GameObject backBtn;

    [Header("---World 1 Levels---")] 
    [SerializeField] private string _Hub = "Hub";

    public string[] DynamicMaps = { "Map1", "Map2" };
    [SerializeField] private string _tutScene = "TutScene";
    [SerializeField] private string _bossScene = "BossRoom";

    [Header("---MainStage Player Controller---")] 
    [SerializeField] private GameObject[] PlayerActivateables;

    public string currLvl;
    public GameObject _Hublvl;
    public int mapnum = 0;
    bool tutorialComplete = false;

    [Header("---Credits Scroll---")]
    public GameObject TextToScroll;
    [SerializeField] int scrollSpeed;
    private Vector3 originalPosition;

    private bool cursorOn;

    private void Start()
    {
        MSInstance = this;
        SceneManager.LoadScene(_Hub, LoadSceneMode.Additive);

        returnToMenu();

        originalPosition = TextToScroll.transform.position;
    }

    private void Update()
    {
        if (!cursorOn)
            Cursor.visible = true;

        CreditsText();
        ESC();
    }

    public static MainSceneLogic GetInstance()
    {
        return MSInstance;
    }

    public void PlayGame()
    {
        GameManager.GetInstance().GetSoundManager().MenuClick(0);
        HideMenu();
        
        MMCamera.SetActive(false);

        Time.timeScale = 1;
        GameManager.GetInstance().ToggleCursorVisibility();
        tutorialComplete = PersistentDataSystem.LoadPlayerProgress();
        if (tutorialComplete)
        {
            
            GameManager.GetInstance().SetGameMode(GameMode.Hub);
            GameManager.GetInstance().TeleportPlayer(0,0, -32f);
        }
        else
        {
            currLvl = _tutScene;
            SceneManager.LoadSceneAsync(currLvl, LoadSceneMode.Additive);
            GameManager.GetInstance().SetGameMode(GameMode.Dungeon);
        }
        GameManager.GetInstance().SetGameState(GameState.Playing);
        //LoadScenes.Add(SceneManager.LoadSceneAsync(_DynamicScenes, LoadSceneMode.Additive));
        for (int i = 0; i < PlayerActivateables.Length; i++)
        {
            PlayerActivateables[i].SetActive(true);
        }
    }
    
    public void HideMenu()
    {
        MenuActivateables.SetActive(false);
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
            loadScreen.SetActive(false);
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
       
        
        GameManager.GetInstance().SetGameMode(GameMode.Dungeon);
    }

    private void ESC()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (SettingsActivateables.activeInHierarchy || CreditsActivateables.activeInHierarchy)
            {
                GameManager.GetInstance().GetSoundManager().MenuClick(1);
                returnToMenu();
            }
        }
    }

    public void ResetPlayer()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Destroy(enemies[i]);
            }
        }

        SceneManager.UnloadSceneAsync(currLvl);

        currLvl = null;
        mapnum = 0;
    }

    public void CreditsScreen()
    {
        HideMenu();
        GameManager.GetInstance().GetSoundManager().MenuClick(0);
        CreditsActivateables.SetActive(true);
        //Start Scroll of Text

    }

    public void SettingsScreen()
    {
        HideMenu();
        GameManager.GetInstance().GetSoundManager().MenuClick(0);
        SettingsActivateables.SetActive(true);
        returnBtn.SetActive(true);
        backBtn.SetActive(false);
    }

    public void DisableTabs()
    {
        GameManager.GetInstance().audioTab.SetActive(false);
        GameManager.GetInstance().controlsTab.SetActive(false);
        GameManager.GetInstance().graphicsTab.SetActive(false);
        GameManager.GetInstance().tabActive = null;
    }

    private void CreditsText()
    {
        if (CreditsActivateables.activeInHierarchy)
            TextToScroll.transform.position = new Vector3(TextToScroll.transform.position.x, TextToScroll.transform.position.y + scrollSpeed, TextToScroll.transform.position.z);
        else
            TextToScroll.transform.position = originalPosition;
    }

    public void returnToMenu()
    {
        mapnum = 0;
        PauseMenuActivateables.SetActive(false);
        SettingsActivateables.SetActive(false);
        CreditsActivateables.SetActive(false);
        loadScreen.SetActive(false);
        MMCamera.SetActive(true);
        DisableTabs();
        GameManager.GetInstance().removeLossMenu();


        // Only process main menu things when the game mode is overridden
        if (GameManager.GetInstance().GetGameMode() == GameMode.Dungeon)
        {
        //    return;
        }
        GameManager.GetInstance().GetSoundManager().MenuClick(1);

        GameManager.GetInstance().SetGameMode(GameMode.MainMenu);
        Time.timeScale = 0;
        GameManager.GetInstance().ToggleCursorVisibility();

        //Lock Cursor so the player can make a selection
        //Load Hub and move it for use later
        for (int i = 0; i < PlayerActivateables.Length; i++)
        {
            PlayerActivateables[i].SetActive(false);
        }
        MenuActivateables.SetActive(true);
    }
    public void Quitgame()
    {
        GameManager.GetInstance().GetSoundManager().MenuClick(1);
        GameManager.GetInstance().SavePlayerData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}