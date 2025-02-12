using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell System/Example Spell")]
public class ExampleSpell : SpellBase
{
    [Header("Spell Properties")]
    public float executionTime;
    
    public override bool CanActivate()
    {
       return true;
    }

    public override void Activate()
    {
        spellSystem.StartCoroutine(CastSpell());
    }

    IEnumerator CastSpell()
    {
        Debug.Log("Casting Spell");
        yield return new WaitForSeconds(executionTime);
        Cancel();
    }

    public override void Cancel()
    {
        base.Cancel();
    }
}
