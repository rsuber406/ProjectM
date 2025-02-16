using System;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public ItemData equippedHelmet;
    public ItemData equippedChestplate;
    public ItemData equippedBoots;
    public ItemData equippedLegs;
    public ItemData equippedRing;
    public ItemData equippedAmulet;
    public ItemData equippedGloves;
    public ItemData equippedWeapon;

    public event Action<ItemData, ArmorType> OnArmorEquipped;
    public event Action<ItemData> OnWeaponEquipped;
    public event Action<ArmorType> OnArmorUnequipped;
    public event Action OnWeaponUnequipped;
    
    private AttributesController attributesController;
    private EquipmentManager equipmentManager;

    
    
    private void Start()
    {
        attributesController.armor.currentValue = 0f;
        attributesController.health.currentValue = 0f;
        attributesController.mana.currentValue = 0f;
        attributesController = GetComponent<AttributesController>();
        equipmentManager.EquipItem(equippedChestplate);
    }

    private void UpdateCharacterUI()
    {
        for (int i = 0; i < 7; i++)
        {
            
        }
    }

    private void UpdateAttributes(ItemData item)
    {
        if (item != null)
        {
            attributesController.armor.currentValue += item.armorModifier;
            attributesController.health.currentValue += item.healthModifier;
            attributesController.mana.currentValue += item.manaModifier;
        }
    }

    public void EquipItem(ItemData item)
    {
        
        
        if (item.itemType == ItemType.Armor){
            
            switch (item.armor)
            {
                case ArmorType.Helmet:
                    equippedHelmet = item;
                    UpdateAttributes(equippedHelmet);
                    OnArmorEquipped?.Invoke(item, item.armor);
                    break;
                case ArmorType.Chestplate:
                    equippedChestplate = item;
                    UpdateAttributes(equippedChestplate);
                    OnArmorEquipped?.Invoke(item, item.armor);
                    break;
                case ArmorType.Boots:
                    equippedBoots = item;
                    UpdateAttributes(equippedBoots);
                    OnArmorEquipped?.Invoke(item, item.armor);
                    break;
                case ArmorType.Gloves:
                    equippedGloves = item;
                    UpdateAttributes(equippedGloves);
                    OnArmorEquipped?.Invoke(item, item.armor);
                    break;
                case ArmorType.Ring:
                    equippedRing = item;
                    UpdateAttributes(equippedRing);
                    OnArmorEquipped?.Invoke(item, item.armor);
                    break;
                case ArmorType.Amulet:
                    equippedAmulet = item;
                    UpdateAttributes(equippedAmulet);
                    OnArmorEquipped?.Invoke(item, item.armor);
                    break;
                case ArmorType.Leggings:
                    equippedLegs = item;
                    UpdateAttributes(equippedLegs);
                    OnArmorEquipped?.Invoke(item, item.armor);
                    break;
            }
        } else if (item.itemType == ItemType.Weapon)
        {
            equippedWeapon = item;
            UpdateAttributes(equippedWeapon);
            OnWeaponEquipped?.Invoke(item);

        }
        
    }
}
