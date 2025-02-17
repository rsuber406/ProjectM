using UnityEngine;

public class EquipmentTest : MonoBehaviour
{
    public Inventory inventory;
    public EquipmentManager equipmentManager;
    public Item testItem;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
             equipmentManager.EquipItem(testItem.itemData);
             
         
        }
    }





    private void EquipItemFromSlot(int slotIndex)
    {
        inventory = FindAnyObjectByType<Inventory>();

        equipmentManager.EquipItem(inventory.slots[slotIndex].item.data);
    }

}
