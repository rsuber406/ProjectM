using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameMode gameMode;
    [SerializeField] private GameState gameState;

    [SerializeField] private GameObject player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private MasterSpellsList masterSpellsList;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject lossMenu;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] public bool enableDebug;

    [SerializeField] public GameObject audioTab;
    [SerializeField] public GameObject controlsTab;
    [SerializeField] public GameObject graphicsTab;

    [SerializeField] private GameObject returnBtn;
    [SerializeField] private GameObject backBtn;


    private SoundManager soundController;
    

    public GameObject damagePanel;
    private GameObject menuActive = null;
    public GameObject tabActive = null;

    public TextMeshProUGUI interactText;

    public Image healthBar;
    public Image manaBar;
    
    public MasterSpellsList MasterSpellsList => masterSpellsList;
    private AIController aiController;
    private bool playerIsInCombatMode = false;
    public event Action OnGameResumed;
    public event Action OnGamePaused;
    
    void Awake()
    {
        instance = this;
        aiController = this.GetComponentInParent<AIController>();
        soundController = this.GetComponent<SoundManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        HandleInDungeonMenuBindings();

    }

    private void HandleInDungeonMenuBindings()
    {
        if (gameMode == GameMode.Dungeon || gameMode == GameMode.Hub || enableDebug)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (menuActive == null)
                {
                    menuActive = pauseMenu;
                    menuActive.SetActive(true);
                    StatePause();
                }
                else if (menuActive == settingsMenu)
                {
                    GameManager.GetInstance().GetSoundManager().MenuClick(1);
                    menuActive = pauseMenu;
                }
                else if (menuActive == pauseMenu)
                {
                    GameManager.GetInstance().GetSoundManager().MenuClick(1);
                    ResumeGame();
                }
            }
        }

        if (enableDebug)
            gameMode = GameMode.Dungeon;

    }

    public void toggleKinematics()
    {
        Rigidbody plrb = GetComponent<Rigidbody>();
        if(plrb == null)
            plrb.isKinematic = true;
        else
            plrb.isKinematic = false;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public void resetPlayerStats()
    {
        //player Health and Mana needs to be reset here. This is urgent
    }

    public Vector3 GetPlayerPosition()
    {
      return player.transform.position;
    }

    public void TeleportPlayer(float xcords, float ycords, float zcords)
    {
        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        playerRB.position = new Vector3(xcords, ycords, zcords);
        player.transform.position = new Vector3(xcords, ycords, zcords);
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public SoundManager GetSoundManager()
    {
        return soundController;
    }

    public Camera GetPlayerCamera()
    {
        return playerCamera;
    }

    public GameMode GetGameMode()
    {
        return gameMode;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameMode(GameMode target)
    {
        gameMode = target;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menuActive.SetActive(false);
        menuActive = null;
        gameState = GameState.Playing;
        ToggleCursorVisibility();
        OnGameResumed?.Invoke();
        victoryMenu.SetActive(false);
    }

    public void StatePause()
    {
        Time.timeScale = 0;
        gameState = GameState.Paused;
        ToggleCursorVisibility();
        OnGamePaused?.Invoke();

    }

    public void ToggleCursorVisibility()
    {
        if (gameMode == GameMode.MainMenu || gameState == GameState.Paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
            return;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SettingsMenu()
    {
        menuActive.SetActive(false);
        menuActive = settingsMenu;
        menuActive.SetActive(true);

        returnBtn.SetActive(false);
        backBtn.SetActive(true);
    }

    public void PauseMenu()
    {
        menuActive.SetActive(false);
        menuActive = pauseMenu;
        menuActive.SetActive(true);
        tabActive.SetActive(false);
        tabActive = null;
    }


    public void GetAudioTab()
    {
        if (tabActive == null)
        {
            tabActive = audioTab;
            tabActive.SetActive(true);
        }
        else if (tabActive == controlsTab || tabActive == graphicsTab)
        {
            tabActive.SetActive(false);
            tabActive = audioTab;
            tabActive.SetActive(true);
        }
        else
        {
            tabActive.SetActive(false);
            tabActive = null;
        }
    }

    public void GetControlsTab()
    {
        if (tabActive == null)
        {
            tabActive = controlsTab;
            tabActive.SetActive(true);
        }
        else if (tabActive == audioTab || tabActive == graphicsTab)
        {
            tabActive.SetActive(false);
            tabActive = controlsTab;
            tabActive.SetActive(true);
        }
        else
        {
            tabActive.SetActive(false);
            tabActive = null;
        }
    }
    
    public void GetGraphicsTab()
    {
        if (tabActive == null)
        {
            tabActive = graphicsTab;
            tabActive.SetActive(true);
        }
        else if (tabActive == audioTab || tabActive == controlsTab)
        {
            tabActive.SetActive(false);
            tabActive = graphicsTab;
            tabActive.SetActive(true);
        }
        else
        {
            tabActive.SetActive(false);
            tabActive = null;
        }
    }

    public void LossMenu()
    {
        GetInstance().GetSoundManager().LossJingle();
        menuActive = lossMenu;
        menuActive.SetActive(true);
        StatePause();
    }

    public void tmpVictoryScreen()
    {
        menuActive = victoryMenu;
        SphereCollider returnToHub = GameObject.FindGameObjectWithTag("LoadHubTag").GetComponent<SphereCollider>();
        returnToHub.enabled = true;
        menuActive.SetActive(true);
        StatePause();
    }

    public void PlayerInteractShow()
    {
        interactText.gameObject.SetActive(true);
    }

    public void PlayerInteractHide()
    { 
        interactText.gameObject.SetActive(false);
    }

    public void Respawn()
    {
        removeLossMenu();
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.RespawnSequence();
        //player.transform.position = new Vector3(0.000f, 0.00f, -32f);
        MainSceneLogic.MSInstance.ResetPlayer();
        ResumeGame();
        
    }

    public void SavePlayerData()
    {
        // Begin the agony of saving the player data
        // I need health mana, and some reference to their inventory
        
        Inventory inventory = player.GetComponentInChildren<Inventory>();
        Item[] playerItems = inventory.GetInventoryItems();
        EquipmentManager equipment = player.GetComponentInChildren<EquipmentManager>();
        ItemData[] equippedItems = equipment.GetEquippedItems();
        PlayerController playerScript = player.GetComponent<PlayerController>();
        float mana = playerScript.GetMana();
        float health = playerScript.GetHealth();
        bool completeTutorial = playerScript.HasCompletedTutorial();
        PersistentDataSystem.SavePlayerData((int)health, (int)mana, playerItems, equippedItems);
        PersistentDataSystem.SavePlayerProgress(completeTutorial);
    }

    public void SetGameState(GameState target)
    {
        gameState = target;
        ToggleCursorVisibility();
    }

    public void removeLossMenu()
    {
        lossMenu.SetActive(false);
    }
    public bool PlayerCompletedTutorial()
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        return playerScript.HasCompletedTutorial();
        
    }

    public void SetPlayerCompleteTutorial(bool complete)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        playerScript.HasCompletedTutorial(complete);
    }

    public bool IsInCombatMode()
    {
        return playerIsInCombatMode;
    }

    public void SetPlayerIsInCombatMode(bool isInCombatMode)
    {
        playerIsInCombatMode = isInCombatMode;
    }
}
