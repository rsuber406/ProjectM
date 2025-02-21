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
    [SerializeField] private bool enableDebug;

    [SerializeField] private GameObject audioTab;


    private SoundManager soundController;
    

    public GameObject damagePanel;
    private GameObject menuActive = null;
    private GameObject tabActive = null;

    public TextMeshProUGUI interactText;

    public Image healthBar;
    public Image manaBar;
    
    public MasterSpellsList MasterSpellsList => masterSpellsList;
    private AIController aiController;


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
        GetInstance().GetSoundManager().Ambience();

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
                else if(menuActive == pauseMenu) ResumeGame();
            }
        }
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
    }

    public void StatePause()
    {
        Time.timeScale = 0;
        gameState = GameState.Paused;
        ToggleCursorVisibility();
    }

    public void ToggleCursorVisibility()
    {
        if (gameMode == GameMode.MainMenu)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
            return;
        }
        
        
        if (gameState == GameState.Paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
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
    }

    public void PauseMenu()
    {
        menuActive.SetActive(false);
        menuActive = pauseMenu;
        menuActive.SetActive(true);
        tabActive.SetActive(false);
        tabActive = null;
    }


    public void AudioTab()
    {
        if (tabActive == null)
        {
            tabActive = audioTab;
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

    public void RemoveLossMenu()
    {
        // call to move player
        
        // Remove loss screen after player is moved to specified location
        
    }

    public void Respawn()
    {
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
        Inventory inventory = player.GetComponent<Inventory>();
        Item[] playerItems = inventory.GetInventoryItems();
        EquipmentManager equipment = player.GetComponent<EquipmentManager>();
        ItemData[] equippedItems = equipment.GetEquippedItems();
        PlayerController playerScript = player.GetComponent<PlayerController>();
        float mana = playerScript.GetMana();
        float health = playerScript.GetHealth();
        PersistentDataSystem.SavePlayerData((int)health, (int)mana, playerItems, equippedItems);
    }

    public void SetGameState(GameState target)
    {
        gameState = target;
        ToggleCursorVisibility();
    }
}
