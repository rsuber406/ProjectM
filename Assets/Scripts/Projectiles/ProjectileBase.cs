using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileBase : MonoBehaviour
{
    public float Lifetime;
    public float ForceToApply;
    private int damage;
    private ConstantForce constantForceRef;
    private Coroutine coroutineRef;
    private DamageSourceType damageSource;
    private AudioSource audioSource;
    
    public AudioClip CastAudioClip;
    public AudioClip ImpactAudioClip;
    [Range(0,1)] public float CastAudioPitch;

    private bool hasImpact;
    
    public void Init(Vector3 direction, int damageAmount, DamageSourceType source)
    {
        damage = damageAmount;
        damageSource = source;
        constantForceRef = gameObject.GetComponent<ConstantForce>();
        constantForceRef.force = direction * ForceToApply;
        
        audioSource = gameObject.GetComponent<AudioSource>();
        if(CastAudioClip) PlaySfxClip(CastAudioClip);
        
        coroutineRef = StartCoroutine(DelayDestroy(Lifetime));
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasImpact) return;

        if (other.isTrigger)
            return;

        hasImpact = true;

        IDamage dmg = other.GetComponentInParent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, damageSource);
        }
        
        StopCoroutine(coroutineRef);
        if(ImpactAudioClip) PlaySfxClip(ImpactAudioClip);

        StartCoroutine(DelayDestroy(ImpactAudioClip.length));
    }
    
    IEnumerator DelayDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    void PlaySfxClip(AudioClip clip)
    {
        float sfxVolume = GameManager.GetInstance().GetSoundManager().audSFX.volume;
        audioSource.PlayOneShot(clip, CastAudioPitch * sfxVolume);
    }
}
