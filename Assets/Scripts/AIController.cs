using UnityEngine;

public  class AIController : MonoBehaviour
{
    [Header("Meshes For AI")]
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private MeshFilter[] meshFilter;
    
    private static AIController instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // The entire purpose of this script is to help manage the AI and control what gets sent as mesh renderer
    // This decreases our enemy memory usage and helps us in general performance. This is a flyweight for the AI
    private Vector3 playerPosition = Vector3.zero;
    void Awake()
    {
        // Upon creation of a game manager I need to get the player position from that manager 
        instance = this;
        playerPosition = GameManager.GetInstance().GetPlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 UpdatePlayerPosition()
    {
        // Reference Game Manager player position;
        return GameManager.GetInstance().GetPlayerPosition();
    }

   public Vector3 GetPlayerPosition()
    {
        return instance.playerPosition;
    }

    public static AIController GetAIController()
    {
        return instance;
    }
   
}
