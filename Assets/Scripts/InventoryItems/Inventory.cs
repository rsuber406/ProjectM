using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
public class InventorySlot
{
    public Item item;
    public int index;

    public InventorySlot(int index)
    {
        item = null;
        this.index = index;
    }
    
}


public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    public InventorySlot[] slots;
    public event Action<Item> OnItemAdded;
    public event Action<Item> OnItemRemoved;
    public event Action OnInventoryChanged;

    public event Action<ItemData> OnEquipItem;

    private void Awake()
    {
      InitializeInventory();
    }

    private void InitializeInventory()
    {
        slots = new InventorySlot[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            slots[i] = new InventorySlot(i);
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

    
    public bool IsFull()
    {
        return CountInventorySlotsLeft() == 0;
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
            if ( slot.item != null && slot.item.itemName == itemName)
            {
                return slot.item;
            }
        }
        return null;
    }
    public bool RemoveItem(string itemName, int index)
    {
      if (string.IsNullOrEmpty(itemName)) return false;

    for (int i = 0; i < slots.Length; i++)
    {
        if (slots[i].item != null && slots[i].item.name == itemName && index == slots[i].index || index == slots[i].index)
        {
            var removedItem = slots[i].item;
            slots[i].item = null;
            OnItemRemoved?.Invoke(removedItem);
            OnInventoryChanged?.Invoke();
            return true;
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
                OnInventoryChanged?.Invoke();
                return addedItem;
            }

        }
        return addedItem;
    }

  

    public void SwapItemPositions(int fromSlot, int toSlot)
    {
        
            if (fromSlot == toSlot) return;
            if (fromSlot < 0 || fromSlot >= slots.Length || toSlot < 0 || toSlot >= slots.Length) return;

         
            var tempItem = slots[toSlot].item;  
            slots[toSlot].item = slots[fromSlot].item;  
            slots[fromSlot].item = tempItem;  

            OnInventoryChanged?.Invoke();
        
        
    }

    public void ClearInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                OnItemRemoved?.Invoke(slots[i].item);
            }
            slots[i].item = null;
        }
        OnInventoryChanged?.Invoke();
    }
    //should be used when player dies

}

