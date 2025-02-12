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
    private int inventorySize = 28;
    public InventorySlot[] slots;

    private void Awake()
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
    public bool RemoveItem(string itemName)
    {
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
        bool addedItem = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                addedItem = true;
                return addedItem;
            }

        }
        return addedItem;
    }

}

