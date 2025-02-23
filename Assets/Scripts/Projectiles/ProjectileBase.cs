using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileBase : MonoBehaviour
{
    public float Lifetime;
    public float ForceToApply;
    public int damage;

    // private ConstantForce constantForceRef;
    private Coroutine coroutineRef;
    public DamageSourceType damageSource;
    private AudioSource audioSource;

    public AudioClip CastAudioClip;
    public AudioClip ImpactAudioClip;
    [Range(0, 1)] public float CastAudioPitch;

    private bool hasImpact;

    private void Start()
    {
        RaycastHit hit;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Transform cameraDirection = GameManager.GetInstance().GetPlayerCamera().transform;

        Vector3 direction = this.transform.rotation * GameManager.GetInstance().GetPlayer().transform.forward;
        rb.linearVelocity = this.transform.forward * ForceToApply;


        audioSource = gameObject.GetComponent<AudioSource>();
        if (CastAudioClip) PlaySfxClip(CastAudioClip);
        coroutineRef = StartCoroutine(DelayDestroy(Lifetime));
    }

    public void Init(Vector3 direction, int damageAmount, DamageSourceType source)
    {
        // damage = damageAmount;
        // damageSource = source;
        // constantForceRef = gameObject.GetComponent<ConstantForce>();
        // constantForceRef.force = direction * ForceToApply;
        //
        // audioSource = gameObject.GetComponent<AudioSource>();
        // if(CastAudioClip) PlaySfxClip(CastAudioClip);
        //
        // coroutineRef = StartCoroutine(DelayDestroy(Lifetime));
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
        if (ImpactAudioClip) PlaySfxClip(ImpactAudioClip);

        StartCoroutine(DelayDestroy(ImpactAudioClip.length));
    }

    IEnumerator DelayDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    void PlaySfxClip(AudioClip clip)
    {
        float sfxVolume = GameManager.GetInstance().GetSoundManager().SFXVol;
        audioSource.PlayOneShot(clip, CastAudioPitch * sfxVolume);
    }
}