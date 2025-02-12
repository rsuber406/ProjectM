using UnityEngine;

public class Item : MonoBehaviour
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                if (inventory.AddItem(this))
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
