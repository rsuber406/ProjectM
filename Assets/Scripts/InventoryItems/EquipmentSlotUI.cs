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


    private void Awake()
    {
        equipmentManager = FindAnyObjectByType<EquipmentManager>();
        imageComponent = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (imageComponent.sprite != null && !isDraggingItem)
        {
            isDraggingItem = true;
            originalPos = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggingItem)
        {
            rectTransform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDraggingItem)
        {
            ResetDragState();
        }
    }
    private void ResetDragState()
    {
        isDraggingItem = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = originalPos;
    }

    public void OnDrop(PointerEventData eventData)
    {
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
    }
}
