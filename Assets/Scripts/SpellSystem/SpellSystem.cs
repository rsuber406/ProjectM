using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttributesController))]
public class SpellSystem : MonoBehaviour
{
    private SpellSystemState state = SpellSystemState.Ready;
    
    [SerializeField] private SpellSlotMap spellSlotMapping;

    public Dictionary<string, RuntimeSpell> BoundSpells => boundSpells;
    private Dictionary<string, RuntimeSpell> boundSpells = new Dictionary<string, RuntimeSpell>();
    private HashSet<SpellBase> grantedSpells = new HashSet<SpellBase>();
    
    private AttributesController attributesController;
    private MasterSpellsList masterSpellsList;
    
    void Start()
    {
        masterSpellsList = GameManager.GetInstance().MasterSpellsList;
        attributesController = GetComponent<AttributesController>();
        InitializeSpells();
    }

    void GrantSpells(SpellBase spell)
    {
        // Here we give the player spells
        grantedSpells.Add(spell);
    }
    
    void RemoveSpell(string spellName)
    {
        // Here we remove the player spells buy name
    }

    void Update()
    {
        CheckForInput();
    }

    void InitializeSpells()
    {
        boundSpells.Clear();
        
        foreach (SpellSlot slot in spellSlotMapping.spellSlots)
        {
            if (slot.assignedSpell != null)
            {
                GrantSpells(slot.assignedSpell);
                RuntimeSpell targetSpell = new RuntimeSpell(slot.assignedSpell);
                targetSpell.spell.Init(this);
                
                boundSpells[slot.slotName] = targetSpell;
            }
        }
    }

    void CheckForInput()
    {
        // Block spell activation if system is not ready to recieve input
        if (state != SpellSystemState.Ready) return;
        
        foreach (var pair in boundSpells)
        {
            // If no spell assigned to slot then skipp slot
            if (pair.Value.spell == null)
            {
                Debug.LogError($"{pair.Key} is unassigned!");
                continue;
            }
            
            if (Input.GetButtonUp(pair.Key))
            {
                if (pair.Value.CanActivate())
                {
                    pair.Value.spell.Activate();
                    pair.Value.lastCastTime = Time.time;
                    state = SpellSystemState.Activated;
                    
                }
            }
        }
    }

    public bool HasEnoughMana(float cost)
    {
        return attributesController.mana.currentValue >= cost;
    }
    
    public bool UseMana(float cost)
    {
        if (HasEnoughMana(cost))
        {
            attributesController.mana.currentValue -= cost;
            return true;
        }

        return false;
    }

    public void ResetSpellActivationState()
    {
        state = SpellSystemState.Ready;
        Debug.Log("SpellSystem is Ready to activate new spell!");
    }
    
    
}
