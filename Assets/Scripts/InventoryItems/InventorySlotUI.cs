using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler,
    IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int slotIndex;
    private InventoryUI inventoryUI;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    private Image itemImage;
    private bool isDraggingItem = false;
    public Inventory inventory;


    [SerializeField] private GameObject itemStatsMenu;
    public TMP_Text itemName;
    public TMP_Text itemArmor;
    public TMP_Text itemMana;
    public TMP_Text itemHealth;
    public TMP_Text itemDescription;

    [NonSerialized] private GameObject currentHoveredItem;
   
    Item unequippedItemParent;
    private EquipmentManager equipmentManager;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryUI = GetComponentInParent<InventoryUI>();
        itemImage = GetComponent<Image>();
        inventory = inventoryUI.GetComponent<Inventory>();
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

    private ItemData GetItemDataFromSlot()
    {
        return inventory.slots[slotIndex].item?.data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemData itemData = GetItemDataFromSlot();

        if (itemData != null)
        {
            ShowItemStats(itemData, eventData);
        }
    }

    private void ShowItemStats(ItemData itemData, PointerEventData eventData)
    {
        if (itemData != null)
        {
            itemName.text = itemData.itemName;
            itemArmor.text = "Armor: " + itemData.armor.ToString();
            itemHealth.text = "Health: " + itemData.healthModifier.ToString();
            itemMana.text = "Mana: " + itemData.manaModifier.ToString();
            itemDescription.text = itemData.description;
            currentHoveredItem = Instantiate(itemStatsMenu, transform);

            Canvas menuCanvas = currentHoveredItem.GetComponent<Canvas>();
            menuCanvas.overrideSorting = true;
            menuCanvas.sortingOrder = 450;

            RectTransform rectTransform = currentHoveredItem.GetComponent<RectTransform>();
            rectTransform.position = eventData.position + new Vector2(150, -150);
        }
    }

    private void HideItemStats()
    {
        if (currentHoveredItem != null)
        {
            currentHoveredItem.SetActive(false);
            Destroy(currentHoveredItem);
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        HideItemStats();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsBagSlot() && !isDraggingItem)
        {
            isDraggingItem = true;
            originalPos = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.5f;
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
            if (fromSlot.inventory != this.inventory)
            {
                Item itemToTransfer = fromSlot.inventory.slots[fromSlot.slotIndex].item;
                if (itemToTransfer != null)
                {
                    if (this.inventory.AddItem(itemToTransfer, this.slotIndex))
                    {
                        fromSlot.inventory.RemoveItem(itemToTransfer.itemName, fromSlot.slotIndex);
                    }
                }
            }
            else
            {
                inventoryUI.SwapItems(fromSlot.slotIndex, this.slotIndex);
            }
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
