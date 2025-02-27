using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler,
    IDropHandler
{
    public ArmorType armorType;
    private EquipmentManager equipmentManager;
    private Image imageComponent;
    private Image oldImage;
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    private bool isDraggingItem = false;
    private RectTransform rectTransform;
    Item unequippedItemParent;
    [SerializeField] private GameObject contextMenuPrefab;
    private GameObject activeContextMenu;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Inventory inventory;


    private static EquipmentSlotUI currentlyDraggedEquipment = null;
    private Sprite oldSprite;

    private void Awake()
    {
        equipmentManager = FindAnyObjectByType<EquipmentManager>();
        imageComponent = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        oldSprite = imageComponent.sprite;
        oldImage = imageComponent;
        inventory = FindFirstObjectByType<Inventory>().GetComponent<Inventory>();

        inventoryUI = FindAnyObjectByType<InventoryUI>().GetComponent<InventoryUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ShowContextMenu(eventData.position);
        }
    }

    public void ShowContextMenu(Vector2 position)
    {
        if (equipmentManager.GetItemData(armorType) == null)
        {
            return;
        }

        if (activeContextMenu != null)
        {
            Destroy(activeContextMenu);
        }

        activeContextMenu = Instantiate(contextMenuPrefab, position, Quaternion.identity, transform);
        RectTransform contextRect = activeContextMenu.GetComponent<RectTransform>();
        contextRect.position = position;

        var buttons = activeContextMenu.GetComponentsInChildren<Button>();

        foreach (var button in buttons)
        {
            switch (button.name)
            {
                case "UnequipButton":
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => { UnequipItem(armorType); });
                    break;
            }
        }
    }

    public void ResetEquipmentSlotImage()
    {
        imageComponent.sprite = oldSprite;
        canvasGroup.alpha = 0.2f;

    }

    private void UnequipItem(ArmorType armorType)
    {
        ItemData unequippedItem = equipmentManager.GetItemData(armorType);
        GameObject newItemObject = new GameObject(unequippedItem.name);
        Item newItem = newItemObject.AddComponent<Item>();
        newItem.itemData = unequippedItem;

        if (unequippedItem.itemType == ItemType.Armor)
        {
            unequippedItem = equipmentManager.UnequipArmor(unequippedItem.armor, unequippedItem);
            ResetEquipmentSlotImage();
        }
        else
        {
            unequippedItem = equipmentManager.UnequipWeapon();
            ResetEquipmentSlotImage();
        }

        if (unequippedItem != null)
        {
            inventory.AddItem(newItem, 1);
        }

        if (activeContextMenu != null)
        {
            Destroy(activeContextMenu);
        }
    }

    private ItemData GetItemDataFromSlot()
    {
        if (gameObject.CompareTag("BagSlot"))
        {
            return equipmentManager.GetItemData(armorType);
        }

        else return null;
    }



    private bool IsBagSlot()
    {
        return gameObject.CompareTag("BagSlot");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsBagSlot() && !isDraggingItem && HasItemToDrag())
        {
            isDraggingItem = true;
            currentlyDraggedEquipment = this;
            originalPos = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsBagSlot() && isDraggingItem && HasItemToDrag())
        {
            rectTransform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsBagSlot() && isDraggingItem)
        {
            ResetDragState();
            currentlyDraggedEquipment = null;
        }
    }

    public void ResetDragState()
    {
        isDraggingItem = false;
        if (canvasGroup != null)
        {
            if (equipmentManager.GetItemData(armorType) != null)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                canvasGroup.alpha = 0.2f;
            }

            canvasGroup.blocksRaycasts = true;
        }

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = originalPos;
        }
    }

    private bool HasItemToDrag()
    {
        if (IsBagSlot())
        {
            return false;
        }

        ItemData itemData = equipmentManager.GetItemData(armorType);
        return itemData != null;
        //return if not null.
    }

    private void OnDisable()
    {
        if (isDraggingItem)
        {
            ResetDragState();
            currentlyDraggedEquipment = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isDraggingItem)
        {
            return;
        }

        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject == null)
        {
            return;
        }

        // INVENTORY TO EQUIPMENT
        InventorySlotUI fromSlot = draggedObject.GetComponent<InventorySlotUI>();
        if (fromSlot != null)
        {
            ItemData itemData = fromSlot.inventory.slots[fromSlot.slotIndex].item.data;

            if (itemData.itemType == ItemType.Armor)
            {
                if (itemData.armor != armorType)
                {
                    return;
                }

                ItemData existingItem = equipmentManager.GetItemData(armorType);
                if (existingItem != null)
                {
                    UnequipItem(armorType);
                }

                equipmentManager.EquipItem(itemData);
                imageComponent.sprite = itemData.icon;
                canvasGroup.alpha = 1f;
                fromSlot.inventory.RemoveItem(itemData.itemName, fromSlot.slotIndex);
                return;
            }

            if (itemData.itemType == ItemType.Weapon)
            {
                ItemData existingItem = equipmentManager.GetItemData(armorType);
                if (existingItem != null)
                {
                    UnequipItem(armorType);
                }

                equipmentManager.EquipItem(itemData);
                imageComponent.sprite = itemData.icon;
                canvasGroup.alpha = 1f;
                fromSlot.inventory.RemoveItem(itemData.itemName, fromSlot.slotIndex);
                return;
            }
        }

        InventorySlotUI toInventorySlot = GetComponent<InventorySlotUI>();

        if (currentlyDraggedEquipment != null && toInventorySlot != null)
        {
            ItemData unequippedItem = equipmentManager.GetItemData(currentlyDraggedEquipment.armorType);

            if (unequippedItem != null)
            {
                if (unequippedItem.itemType == ItemType.Armor)
                {
                    unequippedItem = equipmentManager.UnequipArmor(currentlyDraggedEquipment.armorType, unequippedItem);
                }
                else
                {
                    unequippedItem = equipmentManager.UnequipWeapon();
                }

                if (unequippedItem != null)
                {
                    toInventorySlot.inventory.AddItem(unequippedItemParent, toInventorySlot.slotIndex);
                    currentlyDraggedEquipment.ResetEquipmentSlotImage();
                }
            }

            if (currentlyDraggedEquipment != null)
            {
                currentlyDraggedEquipment.ResetDragState();
                currentlyDraggedEquipment = null;
            }
        }

    }
}

    