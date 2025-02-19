using System;
using Unity.VisualScripting;
using UnityEngine;



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
    
    
    private AttributesController attributesController;
    private EquipmentManager equipmentManager;

    
    
    private void Start()
    { 
         attributesController = GetComponent<AttributesController>();
         attributesController.armor.currentValue = 1f;
         attributesController.health.currentValue = 100f;
         attributesController.mana.currentValue = 10f;
         FindAnyObjectByType<Inventory>().OnEquipItem += EquipItem;

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
