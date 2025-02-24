using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> possibleItems;
    [SerializeField] private int minItems;
    [SerializeField] private int maxItems;
    [SerializeField] Inventory inventory;
    private InventoryUI inventoryUI;
    

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        inventoryUI = GetComponent<InventoryUI>();
        PopulateChestWithRandomItems();
    }

    private void PopulateChestWithRandomItems()
    {
        int numberOfItems = UnityEngine.Random.Range(minItems, maxItems + 1);
        
        for(int i = 0; i < numberOfItems; i++)
        {
            ItemData randomItemData = possibleItems[UnityEngine.Random.Range(0, possibleItems.Count)];
            
            GameObject itemObject = new GameObject(randomItemData.itemName);
            Item newItem = itemObject.AddComponent<Item>();
            newItem.itemData = randomItemData;
            newItem.itemName = randomItemData.itemName;
            
            int randomSlot = GetRandomEmptySlot();
            if(randomSlot != -1)
            {
                inventory.AddItem(newItem, randomSlot);
                inventoryUI.UpdateInventoryUI();
            }
        }
    }

    private int GetRandomEmptySlot()
    {
        List<int> emptySlots = new List<int>();
        for(int i = 0; i < inventory.slots.Length; i++)
        {
            if(inventory.slots[i].item == null)
            {
                emptySlots.Add(i);
            }
        }
        
        if(inventory.IsFull()) return -1;
        return emptySlots[UnityEngine.Random.Range(0, emptySlots.Count)];
    }
}
