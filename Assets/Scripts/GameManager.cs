using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject player;
    [SerializeField] private MasterSpellsList masterSpellsList;

    public GameObject damagePanel;

    
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
}
