using UnityEngine;
using UnityEngine.UI;
public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private GameObject helmetSlot;
    [SerializeField] private GameObject chestSlot;
    [SerializeField] private GameObject leggingsSlot;
    [SerializeField] private GameObject bootsSlot;
    [SerializeField] private GameObject glovesSlot;
    [SerializeField] private GameObject ringSlot;
    [SerializeField] private GameObject amuletSlot;
    [SerializeField] private GameObject weaponSlot;
    
    private EquipmentManager equipmentManager;

    void Start()
    {
        equipmentManager = FindAnyObjectByType<EquipmentManager>();
        FindAnyObjectByType<EquipmentManager>().OnArmorEquipped += UpdateEquipmentSlot;
        FindAnyObjectByType<EquipmentManager>().OnWeaponEquipped += UpdateEquipmentSlot;
        equipmentManager.OnArmorEquipped += UpdateEquipmentSlot;
        equipmentManager.OnWeaponEquipped += UpdateEquipmentSlot;
        equipmentManager.OnArmorUnequipped += ClearEquipmentSlot;
        equipmentManager.OnWeaponUnequipped += ClearEquipmentSlot;
    }

    private void UpdateEquipmentSlot(ItemData item, ArmorType slotType)
    {
        
    }

    private void UpdateEquipmentSlot(ItemData item)
    {
       
    }

    private GameObject GetSlotImage(ArmorType armorType)
    {
        switch (armorType)
        {
            case ArmorType.Helmet: return helmetSlot;
            case ArmorType.Amulet: return amuletSlot;
            case ArmorType.Gloves: return glovesSlot;
            case ArmorType.Ring: return ringSlot;
            case ArmorType.Boots: return bootsSlot;
            case ArmorType.Chestplate: return chestSlot;
            case ArmorType.Leggings: return leggingsSlot;
            default: return null;
        }
    } 
    private void ClearEquipmentSlot(ArmorType slotType)
    {
        GameObject targetSlot = GetSlotImage(slotType);
         
       
    }

    private void ClearEquipmentSlot()
    {
       
    }

   

}
