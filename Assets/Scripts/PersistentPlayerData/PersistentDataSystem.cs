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
    public List<string> inventory;
    public List<string> equipment;
    
}

struct PlayerProgress
{
    public bool completeTutorial;
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
    public static List<Item> playerInventory;
    public static void SavePlayerData(int health, int mana, Item[] inventory, ItemData[] equipment)
    {
        PlayerData data = new PlayerData();
        data.SetData(health, mana);
        List<ItemDataConversion> convertedInventory = ChangeItemToItemData( ref inventory);
        List<ItemDataConversion> convertedEquipment = ChangeItemToItemData( ref equipment);
        List<string> inventoryJsonData = ItemToStringList(ref convertedInventory);
        List<string> equipmentJsonData = ItemToStringList(ref convertedEquipment);
        data.inventory = inventoryJsonData;
        data.equipment = equipmentJsonData;
        string playerJsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", playerJsonData);
    }

    public static PlayerLoadedData LoadPlayerData()
    {
        if (!PlayerPrefs.HasKey("PlayerData"))
        {
            PlayerLoadedData loadedData = new PlayerLoadedData();
            loadedData.health = 100;
            loadedData.mana = 100;
            loadedData.inventory = new List<Item>();
            loadedData.equipment = new List<ItemData>();
            return loadedData;
        }
        string playerData = PlayerPrefs.GetString("PlayerData");
        PlayerData data = JsonUtility.FromJson<PlayerData>(playerData);
        List<ItemDataConversion> convJsonInv = JsonItemToItemDataConversion(ref data.inventory);
        List<ItemDataConversion> convJsonEquip = JsonItemToItemDataConversion(ref data.equipment);
        List<Item> inventory = ItemDataConvToItem(ref convJsonInv);
        playerInventory = inventory;
        List<ItemData> equipment = ItemDataConvToItemData(ref convJsonEquip);
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
            if (items[i] == null) continue;
            ItemDataConversion conv = new ItemDataConversion();
            conv.itemName = items[i].name;
            conv.description = items[i].GetDescription();

            RenderTexture render = new RenderTexture(items[i].data.icon.texture.width, items[i].data.icon.texture.height, 0);
            Graphics.Blit(items[i].data.icon.texture, render);
            Texture2D convertToTexture = new Texture2D(render.width, render.height, TextureFormat.RGBA32, false);
            RenderTexture.active = render;
            convertToTexture.ReadPixels(new Rect(0, 0, render.width, render.height), 0, 0);
            convertToTexture.Apply();
            RenderTexture.active = null;
            byte[] pngToBytes = convertToTexture.EncodeToPNG();
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
            if(items[i] == null) continue;
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

    private static ItemData ConvertStringToItemData(ItemDataConversion loadedItems)
    {
            ItemData item = ScriptableObject.CreateInstance<ItemData>();
            item.name = loadedItems.itemName;
            item.description = loadedItems.description;
            byte[] iconLoader = System.Convert.FromBase64String(loadedItems.icon);
            Texture2D loadable = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            loadable.LoadImage(iconLoader, false);
            loadable.Apply(true);
            Sprite newSprite = Sprite.Create(loadable, new Rect(0, 0, loadable.width, loadable.height), Vector2.zero);
            item.icon = newSprite;
            item.itemType = (ItemType) loadedItems.itemType;
            item.rarity = (ItemRarity) loadedItems.rarity;
            item.armor = (ArmorType) loadedItems.armor;
            item.armorModifier = loadedItems.armorModifier;
            item.healthModifier = loadedItems.healthModifier;
            item.manaModifier = loadedItems.manaModifier;




            return item;
    }

    private static List<string> ItemToStringList(ref List<ItemDataConversion> items)
    {
        if(items.Count == 0) return new List<string>();
        List<string> convertedItem = new List<string>();
        for (int i = 0; i < items.Count; i++)
        {
            string item = JsonUtility.ToJson(items[i]);
            convertedItem.Add(item);
        }
        return convertedItem;
    }

    private static List<ItemDataConversion> JsonItemToItemDataConversion(ref List<string> itemData)
    {
        List<ItemDataConversion> convertedItems = new List<ItemDataConversion>();
        for (int i = 0; i < itemData.Count; i++)
        {
            ItemDataConversion conv = JsonUtility.FromJson<ItemDataConversion>(itemData[i]);
            convertedItems.Add(conv);
        }
        return convertedItems;
    }

    private static List<Item> ItemDataConvToItem(ref List<ItemDataConversion> convJsonData)
    {
        List<Item> convertedItems = new List<Item>();
        if (convJsonData.Count == 0) return convertedItems;
        for (int i = 0; i < convJsonData.Count; i++)
        {
            ItemData item = ConvertStringToItemData(convJsonData[i]);
            GameObject itemGo = new GameObject(convJsonData[i].itemName);
            Item loadedItem = itemGo.AddComponent<Item>();
            loadedItem.itemData = item;
            loadedItem.itemName = item.name;
            convertedItems.Add(loadedItem);
        }
        return convertedItems;
    }

    private static List<ItemData> ItemDataConvToItemData(ref List<ItemDataConversion> convJsonData)
    {
        List<ItemData> convertedItems = new List<ItemData>();
        for (int i = 0; i < convJsonData.Count; i++)
        {
            ItemData item = ConvertStringToItemData(convJsonData[i]);
            convertedItems.Add(item);
            
        }

        return convertedItems;
    }

    public static void SavePlayerProgress(bool completedTutorial)
    {
        PlayerProgress player = new PlayerProgress();
        player.completeTutorial = completedTutorial;
        string json = JsonUtility.ToJson(player);
        PlayerPrefs.SetString("CompleteTutorial", json);
    }

    public static bool LoadPlayerProgress()
    {
        if (PlayerPrefs.HasKey("CompleteTutorial"))
        {
            PlayerProgress player = new PlayerProgress();
            string json = PlayerPrefs.GetString("CompleteTutorial");
            PlayerProgress completed = JsonUtility.FromJson<PlayerProgress>(json);
            return completed.completeTutorial;
        }

        return false;
    }


}
