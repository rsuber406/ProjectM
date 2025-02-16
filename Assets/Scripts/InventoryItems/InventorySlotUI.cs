using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public int slotIndex;
    private InventoryUI inventoryUI;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    private Image itemImage;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryUI = GetComponentInParent<InventoryUI>();
        itemImage = GetComponent<Image>();
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
        if (itemImage.sprite != null)
        {
            originalPos = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemImage.sprite != null)
        {
            rectTransform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = originalPos;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI fromSlot = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (fromSlot != null && fromSlot != this)
        {
            inventoryUI.SwapItems(fromSlot.slotIndex, slotIndex);
        }
    }
}
