using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
public class InventorySlot
{
    public Item item;
    

    public InventorySlot()
    {
        item = null;
        
    }
    
}


public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    public InventorySlot[] slots;
    public event Action<Item> OnItemAdded;
    public event Action<Item> OnItemRemoved;
    public event Action OnInventoryChanged;

    private void Awake()
    {
      InitializeInventory();
    }

    private void InitializeInventory()
    {
        slots = new InventorySlot[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            slots[i] = new InventorySlot();
        }
    }
    public int GetInventorySize()
    {
        int count = 0;
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].item != null)
            {
                count++;
            }

        }
        return count;
    }

    public int CountInventorySlotsLeft()
    {
        int count = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                count++;
            }
        }
        return count;

    }

    public bool HasItem(string itemName)
    {

        foreach (var slot in slots) 
        { 
            if (slot.item.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }
    public Item GetItem(string itemName)
    {
        foreach (var slot in slots)
        {
            if (slot.item.itemName == itemName)
            {
                return slot.item;
            }
        }
        return null;
    }
    public bool RemoveItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item.name == itemName)
            {
                slots[i].item = null;
            }
        }
        return false;
    }
    public bool AddItem(Item item)
    {
        if(item == null) return false;

        bool addedItem = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                addedItem = true;
                OnItemAdded?.Invoke(item);
                return addedItem;
            }

        }
        return addedItem;
    }

}

