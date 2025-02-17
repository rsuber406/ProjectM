using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell System/ Spells /Example Spell")]
public class ExampleSpell : SpellBase
{
    [Header("Spell Properties")]
    public float executionTime;
    
    public override bool CanActivate()
    {
       return spellSystem.HasEnoughMana(cost);
    }

    public override void Activate()
    {
        base.Activate();
        spellSystem.StartCoroutine(CastSpell());
    }

    IEnumerator CastSpell()
    {
        Debug.Log($"Casting {displayName}");
        yield return new WaitForSeconds(executionTime);
        Cancel();
    }

    public override void Cancel()
    {
        base.Cancel();
    }
}
