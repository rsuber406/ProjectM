using System;
using System.Collections.Generic;
using UnityEngine;

struct PlayerData
{
    public void SetData(int health, int mana)
    {
        this.health = health;
        this.mana = mana;
    }

    public int health;
    public int mana;
    public string inventory;
    public string equipment;
    
}


struct ItemDataConversion
{
    public string itemName;
    public string description;
    public string icon;
    public int itemType;
    public int rarity;
    public int armor;
    
    //new stat modifiers
    public int armorModifier;
    public int healthModifier;
    public int manaModifier;
}

public struct PlayerLoadedData
{
    public int health;
    public int mana;
    public List<Item> inventory;
    public List<ItemData> equipment;
}



public static class PersistentDataSystem
{
    
    public static void SavePlayerData(int health, int mana, Item[] inventory, ItemData[] equipment)
    {
        PlayerData data = new PlayerData();
        data.SetData(health, mana);
        List<ItemDataConversion> convertedInventory = ChangeItemToItemData( ref inventory);
        List<ItemDataConversion> convertedEquipment = ChangeItemToItemData( ref equipment);
        string inventoryJsonData = JsonUtility.ToJson(convertedInventory);
        string equipmentJsonData = JsonUtility.ToJson(convertedEquipment);
        data.inventory = inventoryJsonData;
        data.equipment = equipmentJsonData;
        string playerJsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", playerJsonData);
    }

    public static PlayerLoadedData LoadPlayerData()
    {
        string playerData = PlayerPrefs.GetString("PlayerData");
        PlayerData data = JsonUtility.FromJson<PlayerData>(playerData);
        List<ItemData> equipment = ConvertStringToItemData(data.equipment);
        List<Item> inventory = ConvertStringToItem(data.inventory);
        PlayerLoadedData player = new PlayerLoadedData();
        player.health = data.health;
        player.mana = data.mana;
        player.inventory = inventory;
        player.equipment = equipment;
        return player;
    }
    

    private static List<ItemDataConversion> ChangeItemToItemData(ref Item[] items)
    {
        List<ItemDataConversion> itemData = new List<ItemDataConversion>();
        for (int i = 0; i < items.Length; i++)
        {
            ItemDataConversion conv = new ItemDataConversion();
            conv.itemName = items[i].name;
            conv.description = items[i].GetDescription();
            byte[] pngToBytes = items[i].data.icon.texture.EncodeToPNG();
            conv.icon = Convert.ToBase64String(pngToBytes);
            conv.itemType = (int) items[i].data.itemType;
            conv.rarity = (int) items[i].data.rarity;
            conv.armor = (int) items[i].data.armor;
            conv.armorModifier = items[i].data.armorModifier;
            conv.healthModifier = items[i].data.healthModifier;
            conv.manaModifier = items[i].data.manaModifier;
            itemData.Add(conv);
        }

        return itemData;
    }

    private static List<ItemDataConversion> ChangeItemToItemData(ref ItemData[] items)
    {
        List<ItemDataConversion> itemData = new List<ItemDataConversion>();
        for (int i = 0; i < items.Length; i++)
        {
            ItemDataConversion conv = new ItemDataConversion();
            conv.itemName = items[i].name;
            conv.description = items[i].description;
            byte[] pngToBytes = items[i].icon.texture.EncodeToPNG();
            conv.icon = Convert.ToBase64String(pngToBytes);
            conv.itemType = (int) items[i].itemType;
            conv.rarity = (int) items[i].rarity;
            conv.armor = (int) items[i].armor;
            conv.armorModifier = items[i].armorModifier;
            conv.healthModifier = items[i].healthModifier;
            conv.manaModifier = items[i].manaModifier;
            itemData.Add(conv);
        }

        return itemData;
    }

    private static List<ItemData> ConvertStringToItemData(string itemData)
    {
        List<ItemDataConversion> loadedItems = JsonUtility.FromJson<List<ItemDataConversion>>(itemData);
        List<ItemData> convertedItems = new List<ItemData>();
        for (int i = 0; i < loadedItems.Count; i++)
        {
            ItemData item = ScriptableObject.CreateInstance<ItemData>();
            item.name = loadedItems[i].itemName;
            item.description = loadedItems[i].description;
            byte[] iconLoader = System.Convert.FromBase64String(loadedItems[i].icon);
            item.icon.texture.LoadImage(iconLoader);
            item.itemType = (ItemType) loadedItems[i].itemType;
            item.rarity = (ItemRarity) loadedItems[i].rarity;
            item.armor = (ArmorType) loadedItems[i].armor;
            item.armorModifier = loadedItems[i].armorModifier;
            item.healthModifier = loadedItems[i].healthModifier;
            item.manaModifier = loadedItems[i].manaModifier;
            convertedItems.Add(item);

        }
        
        return convertedItems;
    }
    private static List<Item> ConvertStringToItem(string itemData)
    {
        List<ItemDataConversion> loadedItems = JsonUtility.FromJson<List<ItemDataConversion>>(itemData);
        List<Item> convertedItems = new List<Item>();
        for (int i = 0; i < loadedItems.Count; i++)
        {
            ItemData item = ScriptableObject.CreateInstance<ItemData>();
            item.name = loadedItems[i].itemName;
            item.description = loadedItems[i].description;
            byte[] iconLoader = System.Convert.FromBase64String(loadedItems[i].icon);
            item.icon.texture.LoadImage(iconLoader);
            item.itemType = (ItemType) loadedItems[i].itemType;
            item.rarity = (ItemRarity) loadedItems[i].rarity;
            item.armor = (ArmorType) loadedItems[i].armor;
            item.armorModifier = loadedItems[i].armorModifier;
            item.healthModifier = loadedItems[i].healthModifier;
            item.manaModifier = loadedItems[i].manaModifier;
            Item loadedItem = new Item();
            loadedItem.itemData = item;
            convertedItems.Add(loadedItem);

        }
        
        return convertedItems;
    }


}
