using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSpell", menuName = "Spell System/ Spells /ShieldSpell")]
public class ShieldSpell : SpellBase
{
    [Header("Spell Properties")]
    public float Duration;

    public GameObject ShieldPrefab;
    private GameObject shieldObj;
    
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
        if (ShieldPrefab)
        {
            shieldObj = Instantiate(ShieldPrefab, GameManager.GetInstance().GetPlayer().transform.GetChild(0).GetChild(0));
        }
        // Cast time , for som reason this is needed before we can call end
        yield return new WaitForSeconds(1);
        End();
        yield return new WaitForSeconds(Duration);
        
        Cancel();
    }

    public override void Cancel()
    {
        Destroy(shieldObj);
    }
}
