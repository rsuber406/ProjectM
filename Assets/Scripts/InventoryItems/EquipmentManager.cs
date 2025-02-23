using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EquipmentManager : MonoBehaviour
{
    public ItemData equippedHelmetData;
    public ItemData equippedChestplateData;
    public ItemData equippedBootsData;
    public ItemData equippedLegsData;
    public ItemData equippedRingData;
    public ItemData equippedAmuletData;
    public ItemData equippedGlovesData;
    public ItemData equippedWeaponData;


 
    public event Action<ItemData, ArmorType> OnArmorEquipped;
    public event Action<ItemData> OnWeaponEquipped;
    public event Action<ArmorType> OnArmorUnequipped;
    public event Action OnWeaponUnequipped;

    public event Action OnEquipmentNeedReset;
    
    
    private AttributesController attributesController;
    private EquipmentManager equipmentManager;
    
    

    
    
    private void Start()
    { 
         attributesController = GetComponent<AttributesController>();
         FindAnyObjectByType<Inventory>().OnEquipItem += EquipItem;

    }

    public ItemData GetItemData(ArmorType equipmentType)
    {
        switch (equipmentType)
        {
            case ArmorType.Helmet:
                return equippedHelmetData;
            case ArmorType.Chestplate:
                return equippedChestplateData;
            case ArmorType.Boots:
                return equippedBootsData;
            case ArmorType.Leggings:
                return equippedLegsData;
            case ArmorType.Ring:
                return equippedRingData;
            case ArmorType.Amulet:
                return equippedAmuletData;
            case ArmorType.Gloves:
                return equippedGlovesData;
            case ArmorType.None:
                //I know how it looks, but Unity won't allow me to disable enums based on a boolean 8^)
                return equippedWeaponData;
            default:
                return null;
        }
    }
    
    public ItemData[] GetEquippedItems()
    {
        ItemData[] equippedItems = new ItemData[8]; 

        equippedItems[0] = equippedHelmetData;
        equippedItems[1] = equippedChestplateData;
        equippedItems[2] = equippedBootsData;
        equippedItems[3] = equippedLegsData;
        equippedItems[4] = equippedRingData;
        equippedItems[5] = equippedAmuletData;
        equippedItems[6] = equippedGlovesData;
        equippedItems[7] = equippedWeaponData;

        return equippedItems; 
    }
    public ItemData UnequipArmor(ArmorType armorType, ItemData itemData)
    {
        ItemData unequippedItem = itemData;
    
    switch (armorType)
    {
                    case ArmorType.Helmet:
                       unequippedItem = equippedHelmetData;
                       RemoveAttributes(unequippedItem);
                       equippedHelmetData = null;
                       OnArmorUnequipped?.Invoke(ArmorType.Helmet);
            break;
                    case ArmorType.Chestplate:
                       unequippedItem = equippedChestplateData;
                       RemoveAttributes(unequippedItem);
                       equippedChestplateData = null;
                       OnArmorUnequipped?.Invoke(ArmorType.Chestplate);
            
            break;
                    case ArmorType.Boots:
                       unequippedItem = equippedBootsData;
                       RemoveAttributes(unequippedItem);
                       equippedBootsData = null;
                       OnArmorUnequipped?.Invoke(ArmorType.Boots);
            
            break;
                    case ArmorType.Gloves:
                      unequippedItem = equippedGlovesData;
                      RemoveAttributes(unequippedItem);
                      equippedGlovesData = null;
                      OnArmorUnequipped?.Invoke(ArmorType.Gloves);
            
            break;
                    case ArmorType.Ring:
                      unequippedItem = equippedRingData;
                      RemoveAttributes(unequippedItem);
                      equippedRingData = null;
                      OnArmorUnequipped?.Invoke(ArmorType.Ring);
            
            break;
                    case ArmorType.Amulet:
                      unequippedItem = equippedAmuletData;
                      RemoveAttributes(unequippedItem);
                      equippedAmuletData = null;
                      OnArmorUnequipped?.Invoke(ArmorType.Amulet);
            break;
                   case ArmorType.Leggings:
                    unequippedItem = equippedLegsData;
                    RemoveAttributes(unequippedItem);
                    equippedLegsData = null;
                    OnArmorUnequipped?.Invoke(ArmorType.Leggings);
            break;
    }
    OnEquipmentNeedReset?.Invoke();
    return unequippedItem;
}
    

    


    private void UpdateAttributes(ItemData item)
    {
        if (item != null)
        {
            attributesController.armor.AddValue((float)item.armor);
            attributesController.health.AddValue(item.healthModifier);
            attributesController.mana.AddValue(item.manaModifier);
            
        }
    }

    private void RemoveAttributes(ItemData item)
    {
        if (item != null)
        {
            attributesController.armor.ReduceValue((float)item.armor);
            attributesController.health.ReduceValue(item.healthModifier);
            attributesController.mana.ReduceValue(item.manaModifier);       
        }

    }
    public ItemData UnequipWeapon()
    {
        ItemData unequippedItem = equippedWeaponData;
    
        if (unequippedItem != null)
        {
            RemoveAttributes(unequippedItem);
            equippedWeaponData = null;
            OnWeaponUnequipped?.Invoke();
        }
    
        return unequippedItem;
    }

    public void EquipItem(ItemData itemData)
    {
        
        
        if (itemData.itemType == ItemType.Armor){
            
            switch (itemData.armor)
            {
                case ArmorType.Helmet:
                    equippedHelmetData = itemData; 
                    UpdateAttributes(equippedHelmetData);
                    OnArmorEquipped?.Invoke(itemData, itemData.armor);
                    break;
                case ArmorType.Chestplate:
                    equippedChestplateData = itemData;
                   
                    UpdateAttributes(equippedChestplateData);
                    OnArmorEquipped?.Invoke(itemData, itemData.armor);
                    break;
                case ArmorType.Boots:
                    equippedBootsData = itemData;
                    UpdateAttributes(equippedBootsData);
                    OnArmorEquipped?.Invoke(itemData, itemData.armor);
                    break;
                case ArmorType.Gloves:
                    equippedGlovesData = itemData;
                    UpdateAttributes(equippedGlovesData);
                    OnArmorEquipped?.Invoke(itemData, itemData.armor);
                    break;
                case ArmorType.Ring:
                    equippedRingData = itemData;
                    UpdateAttributes(equippedRingData);
                    OnArmorEquipped?.Invoke(itemData, itemData.armor);
                    break;
                case ArmorType.Amulet:
                    equippedAmuletData = itemData;
                    UpdateAttributes(equippedAmuletData);
                    OnArmorEquipped?.Invoke(itemData, itemData.armor);
                    break;
                case ArmorType.Leggings:
                    equippedLegsData = itemData;
                    UpdateAttributes(equippedLegsData);
                    OnArmorEquipped?.Invoke(itemData, itemData.armor);
                    break;
            }
        } else if (itemData.itemType == ItemType.Weapon)
        {
            equippedWeaponData = itemData;
            UpdateAttributes(equippedWeaponData);
            OnWeaponEquipped?.Invoke(itemData);

        }
        
    }
}
