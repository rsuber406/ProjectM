using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private Image helmetSlot;
    [SerializeField] private Image chestSlot;
    [SerializeField] private Image leggingsSlot;
    [SerializeField] private Image bootsSlot;
    [SerializeField] private Image glovesSlot;
    [SerializeField] private Image ringSlot;
    [SerializeField] private Image amuletSlot;
    [SerializeField] private Image weaponSlot;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text manaText;
    [SerializeField] private TMP_Text armorText;
    [SerializeField] private GameObject equipmentUIGameObject;
    [SerializeField] private GameObject contextMenu;

    public Button unequipButton;
    private Image[] equipmentSlots;

    private EquipmentManager equipmentManager;
    private AttributesController attributesController;

    void Start()
    {
        equipmentUIGameObject.gameObject.SetActive(false);
        equipmentSlots = new Image[] {
            helmetSlot,
            chestSlot,
            leggingsSlot, 
            bootsSlot, 
            glovesSlot, 
            ringSlot, 
            amuletSlot, 
            weaponSlot    };
        
        equipmentManager = FindAnyObjectByType<EquipmentManager>();
       equipmentManager.OnArmorEquipped += UpdateEquipmentSlot;
        equipmentManager.OnArmorEquipped += UpdateEquipmentSlot;
        equipmentManager.OnEquipmentNeedReset += ResetEmptyEquipmentSlots;
        attributesController = FindAnyObjectByType<AttributesController>();
        ResetEmptyEquipmentSlots();

    }
    
    private void Update()
    {
        UpdateStatText();
        ToggleCharacterScreen();
    }
    private void ToggleCharacterScreen()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            equipmentUIGameObject.SetActive(!equipmentUIGameObject.activeSelf);
            Cursor.visible = equipmentUIGameObject.activeSelf;
        }
    }
    public void ResetEmptyEquipmentSlots()
    {
        foreach (ArmorType armorType in Enum.GetValues(typeof(ArmorType)))
        {
            if (equipmentManager.GetItemData(armorType) == null)
            {
                Image slotImage = GetSlotImage(armorType);
                if (slotImage != null)
                {
                    slotImage.GetComponent<EquipmentSlotUI>().ResetEquipmentSlotImage(); 
                }
            }
        }
    }
   

    private void UpdateStatText()
    {
        healthText.text = $"$Health: {attributesController.health.currentValue.ToString()}";
        manaText.text = $"$Mana: {attributesController.mana.currentValue.ToString()}";
        armorText.text = $"$Armor: {attributesController.armor.currentValue.ToString()}";
    }
    private void AnchorToZero(Image image)
    {
        if (image.rectTransform != null)
        {
            image.rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    private void UpdateEquipmentSlot(ItemData item, ArmorType slotType)
    {
        if (item != null && item.icon != null)
        {
            if (item.itemType == ItemType.Armor)
            {
                
                switch (slotType)
                {
                    case ArmorType.Helmet:
                        helmetSlot.sprite = item.icon;
                        AnchorToZero(helmetSlot);
                        break;
                    case ArmorType.Amulet:
                        amuletSlot.sprite = item.icon;
                        AnchorToZero(amuletSlot);
                        break;
                    case ArmorType.Gloves:
                        glovesSlot.sprite = item.icon;
                        AnchorToZero(glovesSlot);
                        break;
                    case ArmorType.Ring:
                        ringSlot.sprite = item.icon;
                        AnchorToZero(ringSlot);
                        break;
                    case ArmorType.Boots:
                        bootsSlot.sprite = item.icon;
                        AnchorToZero(bootsSlot);
                        break;
                    case ArmorType.Leggings:
                        leggingsSlot.sprite = item.icon;
                        AnchorToZero(leggingsSlot);
                        break;
                    case ArmorType.Chestplate:
                        chestSlot.sprite = item.icon;
                        AnchorToZero(chestSlot);
                        break;
                }
            }
            else if (item.itemType == ItemType.Weapon)
            {
                weaponSlot.sprite = item.icon;
                AnchorToZero(weaponSlot);
            }
        }

        ActivateImage();
    }
    

    private void ActivateImage()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].sprite != null)
            {
                equipmentSlots[i].gameObject.SetActive(true);
            }
        }
    }
    
    
    
   



    

    private Image GetSlotImage(ArmorType armorType)
    {
        switch (armorType)
        {
            case ArmorType.Helmet: return helmetSlot.GetComponent<Image>();
            case ArmorType.Amulet: return amuletSlot.GetComponent<Image>();
            case ArmorType.Gloves: return glovesSlot.GetComponent<Image>();
            case ArmorType.Ring: return ringSlot.GetComponent<Image>();
            case ArmorType.Boots: return bootsSlot.GetComponent<Image>();
            case ArmorType.Chestplate: return chestSlot.GetComponent<Image>();
            case ArmorType.Leggings: return leggingsSlot.GetComponent<Image>();
            default: return null;
        }
    } 
    
        
        
         
       
    

   

}
