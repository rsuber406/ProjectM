using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringSpellPair
{
    public string key;
    public SpellBase value;
}

[Serializable]
[CreateAssetMenu(fileName = "MasterSpellsList", menuName = "Spell System/MasterSpellsList")]
public class MasterSpellsList : ScriptableObject
{
    public List<StringSpellPair> SpellList = new List<StringSpellPair>();

    private Dictionary<string, SpellBase> dictionary;
    
    public void InitializeDictionary()
    {
        dictionary = new Dictionary<string, SpellBase>();
        foreach (var pair in SpellList)
        {
            if (!dictionary.ContainsKey(pair.key))
            {
                dictionary.Add(pair.key, pair.value);
            }
        }
    }

    public SpellBase GetValue(string key)
    {
        if (dictionary == null) InitializeDictionary();
        return dictionary.TryGetValue(key, out SpellBase value) ? value : null;
    }
}
