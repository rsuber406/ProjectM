using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public ArmorType armorType;
    private EquipmentManager equipmentManager;
    private Image imageComponent;
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    private bool isDraggingItem = false;
    private RectTransform rectTransform;
    Item unequippedItemParent;

    private void Awake()
    {
        equipmentManager = FindAnyObjectByType<EquipmentManager>();
        imageComponent = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"clicked on {gameObject.name}");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
        }
    }

    private bool IsBagSlot()
    {
        return gameObject.CompareTag("BagSlot");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsBagSlot() &&!isDraggingItem)
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

    public void OnDrop(PointerEventData eventData)
    {
        if (isDraggingItem)
        {
            return;
        }
        
        InventorySlotUI fromSlot = eventData.pointerDrag?.GetComponent<InventorySlotUI>();

        if (fromSlot != null) 
        {
            ItemData itemData = fromSlot.inventory.slots[fromSlot.slotIndex].item.data; 

            if (itemData.itemType == ItemType.Armor || itemData.itemType == ItemType.Weapon)
            {
                equipmentManager.EquipItem(itemData); 
                

                fromSlot.inventory.RemoveItem(itemData.itemName, fromSlot.slotIndex);
            }
        }
        EquipmentSlotUI fromEquipmentSlot = eventData.pointerDrag?.GetComponent<EquipmentSlotUI>();
        InventorySlotUI toInventorySlot = GetComponent<InventorySlotUI>();
    
        if (fromEquipmentSlot != null && toInventorySlot != null)
        {
          
            ItemData unequippedItem = fromSlot.inventory.slots[fromSlot.slotIndex].item.data;
            
            unequippedItemParent.itemData = unequippedItem;
           
        
            if (unequippedItem.itemType == ItemType.Armor)
            {
                fromEquipmentSlot.imageComponent.sprite = null;
                unequippedItem = equipmentManager.UnequipArmor(fromEquipmentSlot.armorType, unequippedItem);
            }
            else
            {
                
                unequippedItem = equipmentManager.UnequipWeapon();
            }
            
            
            if (unequippedItem != null)
            {
                toInventorySlot.inventory.AddItem(unequippedItemParent, toInventorySlot.slotIndex);
            }
        }
        isDraggingItem = false;
    }
}
