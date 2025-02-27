using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSpell", menuName = "Spell System/ Spells /ShieldSpell")]
public class ShieldSpell : SpellBase
{
    [Header("Spell Properties")]
    public float Duration;
    public GameObject ShieldPrefab;
    public float spawnDelay;
    public AudioClip CastAudioClip;
    [Range(0,1)] public float CastAudioPitch;
    
    private GameObject shieldObj;
    private AttributesController playerAttributesRef;

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

        playerAttributesRef = player.GetComponent<AttributesController>();
        PlayerAnimation playerAnimRef = player.GetComponent<PlayerAnimation>();
        AudioSource playerAudioSource = player.GetComponent<AudioSource>();

        playerAnimRef.PlayAbilityByTriggerName(AbilityAnimationTriggerName);
        float sfxVolume = GameManager.GetInstance().GetSoundManager().audSFX.volume;
        playerAudioSource.PlayOneShot(CastAudioClip, CastAudioPitch * sfxVolume);
        
        yield return new WaitForSeconds(spawnDelay);
        if (ShieldPrefab)
        {
            shieldObj = Instantiate(ShieldPrefab, player.transform);
        }

        // Cast time , for some reason this is needed before we can call end
        yield return new WaitForSeconds(1);
        playerAttributesRef.IsImmune = true;
        End();
        yield return new WaitForSeconds(Duration);
        
        Cancel();
    }

    public override void Cancel()
    {
        playerAttributesRef.IsImmune = false;
        Destroy(shieldObj);
    }
}
