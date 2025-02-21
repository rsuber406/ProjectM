using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;


public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject[] itemIconSlots;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject contextMenuPrefab;
    private GameObject activeContextMenu;
    [SerializeField] public Inventory inventory;
    
    private int currentSelectedSlotIndex = -1; 
    
    void Start()
    {
        inventory = GetComponent<Inventory>();
        inventory.OnInventoryChanged += UpdateInventoryUI;
        inventoryPanel.SetActive(false);
        Cursor.visible = false;
        
     
    }

    private void DeleteItem(int slotIndex)
    {
        Item item = inventory.slots[slotIndex].item;
        if (item != null)
        {
            
            inventory.RemoveItem(item.itemName, slotIndex);
            
            
        }
        if(activeContextMenu != null)
        {
           CloseContextMenu();
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
        currentSelectedSlotIndex = slot.slotIndex;
        Debug.Log($"Showing context menu for {slot.slotIndex}");
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
                    button.onClick.RemoveAllListeners();
                    Debug.Log(button.name + " is clicked");
                    button.onClick.AddListener(() => { DeleteItem(currentSelectedSlotIndex);
                        
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
                    RectTransform itemIconRectTransform = itemIconSlots[i].GetComponentInChildren<Image>().rectTransform;
                    itemIconRectTransform.anchoredPosition = Vector2.zero;
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
