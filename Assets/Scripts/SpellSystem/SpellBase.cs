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

    [Header("Spell Properties")]
    public Sprite icon;
    public float cooldown;
    public float cost;

    private SpellSystem spellSystem;
    
    public void Init(SpellSystem spellSystem)
    {
        this.spellSystem = spellSystem;
    }

    public abstract bool CanActivate();
    public abstract void Activate();
    public abstract void Cancel();
}
