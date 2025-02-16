using System.ComponentModel;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "ItemData")]
public class ItemData: ScriptableObject
{

    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public ItemRarity rarity;
    public ArmorType armor;
    
    //new stat modifiers
    public int armorModifier;
    public int healthModifier;
    public int manaModifier;


}


public enum ItemType
{
    Weapon,
    Armor

        //add more as needed
}
public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
        //don't plan on using legendary but it's here if needed.
}

public enum ArmorType
{
    Helmet,
    Chestplate,
    Leggings,
    Boots,
    Gloves,
    Ring,
    Amulet
}