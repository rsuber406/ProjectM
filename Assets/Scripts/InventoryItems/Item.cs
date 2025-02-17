using UnityEngine;

public class Item : MonoBehaviour, IPickup
{
    [Header("Item Config")]

    [SerializeField] public ItemData itemData;

    public ItemData data => itemData;
    public string itemName;

    private void LoadItemData()
    {
        if (itemData != null)
        {
            itemName = itemData.name;
        }
    }
    public ItemType GetItemType()
    {
        return itemData.itemType;
    }
    public string GetDescription()
    {
        return itemData.description;
    }
    public void Pickup()
    {
        Inventory inventory = FindFirstObjectByType<Inventory>();
        if (inventory != null && inventory.AddItem(this))
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            Pickup();
        }
    }
}
