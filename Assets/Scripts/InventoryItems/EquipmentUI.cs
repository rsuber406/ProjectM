using TMPro;
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
    
    [SerializeField] private GameObject equipmentUI;
    
    private Image[] equipmentSlots;
    
    

    private int numOfItems = 8;

    private EquipmentManager equipmentManager;
    private AttributesController attributesController;

    void Start()
    {
   
        equipmentUI.gameObject.SetActive(false);
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
        FindAnyObjectByType<EquipmentManager>().OnArmorEquipped += UpdateEquipmentSlot;
        equipmentManager.OnArmorEquipped += UpdateEquipmentSlot;
        equipmentManager.OnArmorUnequipped += ClearEquipmentSlot;
        equipmentManager.OnWeaponUnequipped += ClearEquipmentSlot;
        attributesController = FindAnyObjectByType<AttributesController>();
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
            equipmentUI.SetActive(!equipmentUI.activeSelf);
            Cursor.lockState = equipmentUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = equipmentUI.activeSelf;
        }
    }

    private void UpdateStatText()
    {
        healthText.text = $"$Health: {attributesController.health.currentValue.ToString()}";
        manaText.text = $"$Mana: {attributesController.mana.currentValue.ToString()}";
        armorText.text = $"$Armor: {attributesController.armor.currentValue.ToString()}";
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
                        break;
                    case ArmorType.Amulet:
                        amuletSlot.sprite = item.icon;
                        break;
                    case ArmorType.Gloves:
                        glovesSlot.sprite = item.icon;
                        break;
                    case ArmorType.Ring:
                        ringSlot.sprite = item.icon;
                        break;
                    case ArmorType.Boots:
                        bootsSlot.sprite = item.icon;
                        break;
                    case ArmorType.Leggings:
                        leggingsSlot.sprite = item.icon;
                        break;
                    case ArmorType.Chestplate:
                        chestSlot.sprite = item.icon;
                        break;
                }
            }
            else if (item.itemType == ItemType.Weapon)
            {
                weaponSlot.sprite = item.icon;
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
    private void ClearEquipmentSlot(ArmorType slotType)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i] == GetSlotImage(slotType))
            {
                equipmentSlots[i].gameObject.SetActive(false);
            }
        }
        
        
         
       
    }

    private void ClearEquipmentSlot()
    {
      
    }

   

}
