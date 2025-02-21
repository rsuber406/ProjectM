
using UnityEngine;

public class NpcAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float range;
    private bool playerInRange = false;
    private bool toggleInventory = false;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject bagGrid;
    void Start()
    {
        range = this.GetComponent<SphereCollider>().radius;
        
    }

    // Update is called once per frame
    void Update()
    {
        FacePlayer();
        SendInventory();
    }

    private void FacePlayer()
    {
        if (Vector3.Distance(GameManager.GetInstance().GetPlayerPosition(), this.transform.position) <= range)
        {
            Vector3 direction = GameManager.GetInstance().GetPlayerPosition() - this.transform.position;
            direction.Normalize();
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * 5f);
            playerInRange = true;
        }
        else playerInRange = false;
    }

    private void SendInventory()
    {
        if (!playerInRange)
        {
            inventoryPanel.SetActive(false);
            toggleInventory = false;
        }
        PlayerController player = GameManager.GetInstance().GetPlayer().GetComponent<PlayerController>();
        if (player != null)
        {
            if (Input.GetButtonDown("Interact"))
            {
                toggleInventory = !toggleInventory;
                inventoryPanel.SetActive(toggleInventory);
                bagGrid.SetActive(toggleInventory);
                
            }
            
            
        }
        

    }
}
