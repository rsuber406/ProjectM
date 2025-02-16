using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject player;
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
        // 2 is reference to the position of the model that is moving
       // Debug.Log(player.transform.GetChild(0).GetChild(0).position);
        
      // return player.transform.GetChild(0).GetChild(0).position;
      return player.transform.position;
        
    }

    public GameObject GetPlayer()
    {
        return player;
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
        menuActive = settingsMenu;
        menuActive.SetActive(true);
    }

    public void LossMenu()
    {
        menuActive = lossMenu;
        menuActive.SetActive(true);
        StatePause();
    }
    
    
    
    
}
