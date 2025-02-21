using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BlinkSpell", menuName = "Spell System/ Spells /BlinkSpell")]
public class BlinkSpell : SpellBase
{
    [Header("Spell Properties")]
    public float Distance;

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

        GameObject player = GameManager.GetInstance().GetPlayer();
        Rigidbody playerRigidBody = player.GetComponent<Rigidbody>();

        GameManager.GetInstance().GetSoundManager().BlinkSpell();

        // Blink in the cameras forward direction by default
        Transform cameraTransform = GameManager.GetInstance().GetPlayerCamera().transform;
        Vector3 direction = cameraTransform.forward;

        if (playerRigidBody.linearVelocity.magnitude > 0.13) 
            direction = playerRigidBody.linearVelocity.normalized;
        
        playerRigidBody.AddForce(direction * Distance, ForceMode.Impulse);
        player.transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        
        if (ParticleEffects)
        {
            effect = Instantiate(ParticleEffects, player.transform);
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