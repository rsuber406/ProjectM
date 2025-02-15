using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BlinkSpell", menuName = "Spell System/ Spells /BlinkSpell")]
public class BlinkSpell : SpellBase
{
    [Header("Spell Properties")]
    public float Distance;
    public Vector3 DefaultDirection = Vector3.forward;

    public GameObject ParticleEffects;
    private GameObject effect;

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

        Rigidbody playerRigidBody = GameManager.GetInstance().GetPlayer().GetComponentInChildren<Rigidbody>();

        Vector3 direction = DefaultDirection;

        if (playerRigidBody.linearVelocity.magnitude > 0.13) 
            direction = playerRigidBody.linearVelocity.normalized;
        
        playerRigidBody.AddForce(direction * Distance, ForceMode.Impulse);

        if (ParticleEffects)
        {
            effect = Instantiate(ParticleEffects, GameManager.GetInstance().GetPlayer().transform.GetChild(0).GetChild(0));
        }
        
        // Cast time , for som reason this is needed before we can call end
        yield return new WaitForSeconds(1);
        End();
        Cancel();
    }

    public override void Cancel()
    {
        Destroy(effect);
    }
}