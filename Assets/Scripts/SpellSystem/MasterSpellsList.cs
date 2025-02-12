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
    public List<StringSpellPair> keyValuePairs = new List<StringSpellPair>();

    private Dictionary<string, SpellBase> _dictionary;
    
    public void InitializeDictionary()
    {
        _dictionary = new Dictionary<string, SpellBase>();
        foreach (var pair in keyValuePairs)
        {
            if (!_dictionary.ContainsKey(pair.key))
            {
                _dictionary.Add(pair.key, pair.value);
            }
        }
    }

    public SpellBase GetValue(string key)
    {
        if (_dictionary == null) InitializeDictionary();
        return _dictionary.TryGetValue(key, out SpellBase value) ? value : null;
    }
}
