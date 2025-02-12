using System;
using UnityEngine;


// This is a template class not intended to be instanciated
[Serializable]
public abstract class SpellBase : ScriptableObject, ISpell
{
    [Header("Spell Info")]
    public string displayName;
    public string description;
    public string failedActivationMessage;

    [Header("Base Spell Properties")]
    public Sprite icon;
    public float cooldown;
    public float cost;

    protected SpellSystem spellSystem;
    public event Action OnSpellEnd;
    
    public void Init(SpellSystem spellSystem)
    {
        this.spellSystem = spellSystem;
        OnSpellEnd += this.spellSystem.ResetSpellActivationState;
    }

    public void Cleanup()
    {
        spellSystem = null;
        OnSpellEnd -= spellSystem.ResetSpellActivationState;
    }

    public abstract bool CanActivate();

    public virtual void Activate()
    {
        if (!spellSystem.UseMana(cost)) return;
        Debug.Log($"{displayName} activated!");
    }

    public virtual void Cancel()
    {
        Debug.Log($"{displayName} cancelled!");
        OnSpellEnd?.Invoke();
    }
}
