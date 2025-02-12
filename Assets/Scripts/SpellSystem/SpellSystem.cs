using System.Collections.Generic;
using UnityEngine;

enum SpellSystemState
{
    Ready, // Can a spell be activated now?
    Activated, // Is a spell in progress blocking other spells from activating
    Offline,
}


[RequireComponent(typeof(AttributesController))]
public class SpellSystem : MonoBehaviour
{
    private SpellSystemState state = SpellSystemState.Ready;
    
    private AttributesController attributesController;
    [SerializeField] private MasterSpellsList masterSpellsList;
    
    [SerializeField] private SpellSlotMap spellSlotMapping;
    private Dictionary<string, SpellBase> boundSpells = new Dictionary<string, SpellBase>();
    private HashSet<SpellBase> grantedSpells = new HashSet<SpellBase>();
    
    void Awake()
    {
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
                slot.assignedSpell.Init(this);
                boundSpells[slot.slotName] = slot.assignedSpell;
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
            if (pair.Value == null)
            {
                Debug.LogError($"{pair.Key} is unassigned!");
                continue;
            }
            
            if (Input.GetButtonUp(pair.Key))
            {
                if (pair.Value.CanActivate())
                {
                    pair.Value.Activate();
                    state = SpellSystemState.Activated;
                    pair.Value.OnSpellEnd += ResetSpellActivationState;
                }
            }
        }
    }

    private void ResetSpellActivationState()
    {
        state = SpellSystemState.Ready;
        Debug.Log("SpellSystem is Ready to activate new spell!");
    }
}
