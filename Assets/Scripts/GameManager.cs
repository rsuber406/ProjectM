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
    
    public GameObject damagePanel;
    private GameObject menuActive = null;
    
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
        menuActive = settingsMenu;
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

    public void RemoveLossMenu()
    {
        // call to move player
        
        // Remove loss screen after player is moved to specified location
        
    }

    public void Respawn()
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.RespawnSequence();
        player.transform.position = new Vector3(0.000f, 0.00f, -32f);
        GameObject playerModel = GameObject.FindGameObjectWithTag("PlayerSocket");
        MainSceneLogic.MSInstance.ResetPlayer();
        Debug.Log(player.transform.position);
        Debug.Log(playerModel.transform.position);
        ResumeGame();
        
    }
}
