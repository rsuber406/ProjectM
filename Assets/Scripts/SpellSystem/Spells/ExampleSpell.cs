using UnityEngine;

[CreateAssetMenu(menuName = "Spell System/Example Spell")]
public class ExampleSpell : SpellBase
{
    public override bool CanActivate()
    {
       return true;
    }

    public override void Activate()
    {
        Debug.Log("Spell Activated");
    }

    public override void Cancel()
    {
        Debug.Log("Spell Cancelled");
    }
}
