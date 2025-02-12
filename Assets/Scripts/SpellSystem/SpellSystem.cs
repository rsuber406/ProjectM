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
    private AttributesController attributesController;
    [SerializeField] private MasterSpellsList masterSpellsList;
    
    private SpellSystemState state = SpellSystemState.Ready;
    
    [SerializeField]
    private List<StringSpellPair> ActiveSpellMap = new List<StringSpellPair>();

    private List<string> grantedSpells = new List<string>();
    
    void Awake()
    {
        InitializeSpells();
    }

    void GrantSpells(SpellBase[] spells)
    {
        // Here we give the player spells
        
        InitializeSpells();
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
        foreach (StringSpellPair pair in ActiveSpellMap)
        {
            pair.value.Init(this);
        }
    }

    void CheckForInput()
    {
        // Block spell activation if system is not ready to recieve input
        if (state != SpellSystemState.Ready) return;
        
        foreach (StringSpellPair pair in ActiveSpellMap)
        {
            // If no spell assigned to slot then skipp slot
            if (pair.value == null) continue;
            
            if (Input.GetButtonUp(pair.key))
            {
                if (pair.value.CanActivate())
                {
                    pair.value.Activate();
                    state = SpellSystemState.Activated;
                    pair.value.OnSpellEnd += ResetSpellActivationState;
                }
            }
        }
    }

    private void ResetSpellActivationState()
    {
        state = SpellSystemState.Ready;
        Debug.Log("SpellSystem is Ready to activate new spell");
    }
}
