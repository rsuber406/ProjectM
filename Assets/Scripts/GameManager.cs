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

    public Vector3 GetPlayerPosition()
    {
      return player.transform.position;
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

    private void StatePause()
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
}
