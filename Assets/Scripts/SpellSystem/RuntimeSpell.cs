using System;
using UnityEngine;

[Serializable]
public class RuntimeSpell
{
    public float lastCastTime;
    public SpellBase spell;

    public RuntimeSpell(SpellBase inSpell)
    {
        lastCastTime = Time.time - inSpell.cooldown;
        spell = inSpell;
    }

    public bool IsCooldownActive()
    {
        return GetRemainingCooldown() > 0;
    }
    
    public float GetRemainingCooldown()
    {
        return Mathf.Max(0, (lastCastTime + spell.cooldown) - Time.time);
    }

    public bool CanActivate()
    {
        float cooldown = GetRemainingCooldown();
        if (cooldown > 0)
        {
            Debug.Log($"{spell.displayName} is on cooldown for {cooldown:F2} more seconds.");
            return false;
        }
        
        return spell.CanActivate() && cooldown <= 0;
    }
}