using UnityEngine;
public class InventorySlot
{
 //Add item stuff here
 //

    public InventorySlot()
    {
        // set item to null
        
    }
    
}


public class Inventory : MonoBehaviour
{
    private int inventorySize = 28;
    public InventorySlot[] slots;

    private void Awake()
    {
        slots = new InventorySlot[inventorySize];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot();
        }
    }
    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            //implement count for items
        }
        return -1;
    }
    public bool RemoveItem(string itemName, int numOfItems = 1)
    {
        //TODO
        return false;
    }

}

