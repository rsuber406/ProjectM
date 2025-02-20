using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler,
    IDropHandler
{
    public int slotIndex;
    private InventoryUI inventoryUI;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    private Image itemImage;
    private bool isDraggingItem = false;
    [NonSerialized] 
    public Inventory inventory;
    Item unequippedItemParent;
    private EquipmentManager equipmentManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryUI = GetComponentInParent<InventoryUI>();
        itemImage = GetComponent<Image>();
        inventory = FindAnyObjectByType<Inventory>();
        equipmentManager = FindAnyObjectByType<EquipmentManager>();
    }

    private void OnDisable()
    {
        if (isDraggingItem)
        {
            ResetDragState();
        }

    }

    private void ResetDragState()
    {
        isDraggingItem = false;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = originalPos;
        }
    }

    private bool IsBagSlot()
    {
        return gameObject.CompareTag("BagSlot");
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"clicked on {gameObject.name}");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            inventoryUI.ShowContextMenu(this, eventData.position);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsBagSlot() && !isDraggingItem)
        {
            isDraggingItem = true;
            originalPos = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsBagSlot() && isDraggingItem)
        {
            rectTransform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsBagSlot() && isDraggingItem)
        {
            ResetDragState();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isDraggingItem)
        {
            return;
        }
        
        InventorySlotUI fromSlot = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (fromSlot != null && fromSlot != this)
        {
            inventoryUI.SwapItems(fromSlot.slotIndex, slotIndex);
            return;
        }

        EquipmentSlotUI fromEquipmentSlot = eventData.pointerDrag?.GetComponent<EquipmentSlotUI>();
        if (fromEquipmentSlot != null)
        {
            ArmorType equipmentType = fromEquipmentSlot.armorType;
            ItemData unequippedItemData = equipmentManager.GetItemData(equipmentType);

            if (unequippedItemData != null)
            {
                GameObject itemObject = new GameObject(unequippedItemData.itemName);
                Item newItem = itemObject.AddComponent<Item>();
                newItem.itemData = unequippedItemData;
                newItem.itemName = unequippedItemData.itemName;
                
            
                if (unequippedItemData.itemType == ItemType.Armor)
                {
                    unequippedItemData = equipmentManager.UnequipArmor(fromEquipmentSlot.armorType, unequippedItemData);
                }
                else
                {
                    unequippedItemData = equipmentManager.UnequipWeapon();
                }
            
                if (unequippedItemData != null)
                {
                    
                    newItem.itemData = unequippedItemData;
                    inventory.AddItem(newItem, this.slotIndex);
                    
                }
                else
                {
                    Destroy(itemObject);
                }
            }
        }

    }
}
