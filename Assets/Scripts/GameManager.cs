using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private MasterSpellsList masterSpellsList;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject lossMenu;
    [SerializeField] private GameObject victoryMenu;

    [SerializeField] private GameObject settingsMenu;

    private SoundManager soundController;
    

    public GameObject damagePanel;
    private GameObject menuActive = null;
    public TextMeshProUGUI interactText;
    public Image healthBar;
    public Image manaBar;
    
    public MasterSpellsList MasterSpellsList => masterSpellsList;
    //private fields
    private AIController aiController;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void StatePause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

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
    }

    public void LossMenu()
    {
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
}
