using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpellSlotMap", menuName = "Spell System /SpellSlotMap")]
public class SpellSlotMap : ScriptableObject
{
    public List<SpellSlot> spellSlots;
}

[Serializable]
public class SpellSlot
{
    public string slotName; // "Ability1", "Ability2"
    public SpellBase assignedSpell; // Assigned spell (may be null if no spell assigned)
}
