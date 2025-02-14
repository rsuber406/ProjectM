using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public Item testItem; 
    public Inventory playerInventory;

    // Test adding item with a keypress
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool wasAdded = playerInventory.AddItem(testItem);
            Debug.Log(wasAdded ? "Item added successfully!" : "Failed to add item - inventory might be full");

            Debug.Log($"Inventory slots filled: {playerInventory.GetInventorySize()}");
            Debug.Log($"Slots remaining: {playerInventory.CountInventorySlotsLeft()}");
            Debug.Log($" in bag: {playerInventory.GetItem("Sword")}");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            bool wasRemoved = playerInventory.RemoveItem(("Sword"));
            
            Debug.Log($"Inventory slots removed: {playerInventory.GetItem("Sword")}");

        }
    }
}
