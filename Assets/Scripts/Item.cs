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


}
