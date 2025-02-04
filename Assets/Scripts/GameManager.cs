using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [SerializeField] private GameObject player;
    
    //private fields
    private AIController aiController;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        aiController = this.GetComponentInParent<AIController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
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
}
