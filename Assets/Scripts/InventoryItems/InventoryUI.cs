using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject[] itemIconSlots;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject contextMenuPrefab;
    private GameObject activeContextMenu;
    [SerializeField] public Inventory inventory;
    
    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
        inventory.OnInventoryChanged += UpdateInventoryUI;
        inventoryPanel.SetActive(false);
        Cursor.visible = false;
        
        for (int i = 0; i < itemIconSlots.Length; i++)
        {
            var slotUI = itemIconSlots[i].AddComponent<InventorySlotUI>();
            slotUI.slotIndex = i;
            
            if (!itemIconSlots[i].GetComponent<CanvasGroup>())
            {
                itemIconSlots[i].AddComponent<CanvasGroup>();
            }
        }
    }

    private void DeleteItem(int slotIndex)
    {
        Item item = inventory.slots[slotIndex].item;
        if (item != null)
        {
            
            inventory.RemoveItem(item.itemName);
            
            CloseContextMenu();
        }
        else
        {
            Debug.Log("No item to delete");
        }
    }

    private void CloseContextMenu()
    {
        if (activeContextMenu != null)
        {
            Destroy(activeContextMenu);
        }
    }
    void Update()
    {
        ToggleInventory();
    }

    public void ShowContextMenu(InventorySlotUI slot, Vector2 position)
    {
    
        if (inventory.slots[slot.slotIndex].item == null) 
        {
            return;
        }

        if (activeContextMenu != null)
        {
            Destroy(activeContextMenu);
        }

        activeContextMenu = Instantiate(contextMenuPrefab, position, Quaternion.identity, transform);
        RectTransform contextRect = activeContextMenu.GetComponent<RectTransform>();
        contextRect.position = position;

        var buttons = activeContextMenu.GetComponentsInChildren<Button>();
    
        foreach (var button in buttons)
        {
            switch (button.name)
            {
                case "DeleteButton":
                    button.onClick.AddListener(() => {
                        DeleteItem(slot.slotIndex);
                    }
                        );
                    break;
            }
        }
    }

    private void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            Cursor.lockState = inventoryPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = inventoryPanel.activeSelf;
        }
    }

    private void EquipItem(int slotIndex)
    {
        Item item = inventory.slots[slotIndex].item;
        //equip logic
        
        CloseContextMenu();
    }

    public void SwapItems(int fromIndex, int toIndex)
    {
        inventory.SwapItemPositions(fromIndex, toIndex);
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (i < itemIconSlots.Length)
            {
                Image slotImage = itemIconSlots[i].GetComponent<Image>();
                
                if (inventory.slots[i].item != null)
                {
                    slotImage.sprite = inventory.slots[i].item.data.icon;
                    itemIconSlots[i].SetActive(true);
                }
                else
                {
                    slotImage.sprite = null;
                    itemIconSlots[i].SetActive(false);
                }
            }
        }
    }
}
