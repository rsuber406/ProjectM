using System;
using System.Collections.Generic;
using UnityEngine;


public enum SpellSlotEnum
{
    Ability1,
    Ability2,
    Ability3,
}
public class PlayerSpellBarController : MonoBehaviour
{
    private SpellSystem playerSpellSystem;
    [SerializeField] public GameObject[] spellSlots;
    void Start()
    {
        GameObject go = GameManager.GetInstance().GetPlayer();
        playerSpellSystem = go.GetComponentInParent<SpellSystem>();

        InitializeSpellSlots();
    }

    void InitializeSpellSlots()
    {
        Dictionary<string, RuntimeSpell> boundSpells = playerSpellSystem.BoundSpells;
        
        foreach (GameObject slot in spellSlots)
        {
            SpellSlotUI slotUI = slot.GetComponent<SpellSlotUI>();
            if (slotUI == null) continue;

            string spellSlotKey = slotUI.spellslot.ToString();

            if (boundSpells.TryGetValue(spellSlotKey, out RuntimeSpell assignedSpell))
            {
                slotUI.Init(assignedSpell);
            }
            else
            {
                slotUI.ClearSlot();
            }
        }
    }
    
}
