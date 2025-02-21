using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
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
    
    private static EquipmentSlotUI currentlyDraggedEquipment = null;

    private void Awake()
    {
        equipmentManager = FindAnyObjectByType<EquipmentManager>();
        imageComponent = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        oldImage = imageComponent;
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
                    button.onClick.AddListener(() => { UnequipItem(armorType); } );
                    break;
            }
        }
    }

    private void UnequipItem(ArmorType armorType)
    {
        ItemData unequippedItem = equipmentManager.GetItemData(armorType);
        Inventory inventory = FindAnyObjectByType<Inventory>();
            
        if (unequippedItem.itemType == ItemType.Armor)
        {
            imageComponent.sprite = imageComponent.sprite;
            canvasGroup.alpha = 0.2f;
            unequippedItem = equipmentManager.UnequipArmor(armorType, unequippedItem);
        }
        else
        {    
            unequippedItem = equipmentManager.UnequipWeapon();
        }
            
        if (unequippedItem != null)
        {
            inventory.AddItem(unequippedItemParent, 0);
        }

        if (activeContextMenu != null)
        {
            Destroy(activeContextMenu);
        }
    }
    
    private bool IsBagSlot()
    {
        return gameObject.CompareTag("BagSlot");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsBagSlot() && !isDraggingItem)
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
            currentlyDraggedEquipment = null; 
        }
    }
    
    public void ResetDragState()
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
        
        InventorySlotUI fromSlot = draggedObject.GetComponent<InventorySlotUI>();
        if (fromSlot != null) 
        {
            ItemData itemData = fromSlot.inventory.slots[fromSlot.slotIndex].item.data; 

            if (itemData.itemType == ItemType.Armor || itemData.itemType == ItemType.Weapon)
            {
                canvasGroup.alpha = 1f;
                equipmentManager.EquipItem(itemData); 
                fromSlot.inventory.RemoveItem(itemData.itemName, fromSlot.slotIndex);
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
                        currentlyDraggedEquipment.imageComponent.sprite = currentlyDraggedEquipment.imageComponent.sprite;
                        canvasGroup.alpha = 0.2f;
                        unequippedItem = equipmentManager.UnequipArmor(currentlyDraggedEquipment.armorType, unequippedItem);
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
                
                if (currentlyDraggedEquipment != null)
                {
                    currentlyDraggedEquipment.ResetDragState();
                    currentlyDraggedEquipment = null;
                }
        }
    }
}


    