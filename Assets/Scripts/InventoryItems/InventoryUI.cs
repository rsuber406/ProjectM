using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject[] itemIconSlots;
    [SerializeField] private GameObject inventoryPanel;
    
    void Start()
    {
        FindAnyObjectByType<Inventory>().OnInventoryChanged += UpdateInventoryUI;
    }
    void Update()
    {
            ToggleInventory(); 
    }

    private void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }



    private void UpdateInventoryUI()
    {
        Inventory inventory = FindAnyObjectByType<Inventory>();
        Debug.Log($"itemIconSlots: {itemIconSlots != null}");
        Debug.Log($"inventory: {inventory != null}");
        for (int i = 0; i < inventory.slots.Length; i++)
        {
          
            if (i < itemIconSlots.Length) 
            {
                if (inventory.slots[i].item != null)
                {
                    itemIconSlots[i].SetActive(true);
                    itemIconSlots[i].GetComponent<UnityEngine.UI.Image>().sprite = inventory.slots[i].item.data.icon;
                }
                else if (itemIconSlots[i].activeSelf)

                {
                    itemIconSlots[i].SetActive(false);
                }
            }
        }
    }

}
