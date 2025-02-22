using System.Collections;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float Lifetime;
    public float ForceToApply;
    private int damage;
    private ConstantForce constantForceRef;
    private Coroutine coroutineRef;
    private DamageSourceType damageSource;
    
    public void Init(Vector3 direction, int damageAmount, DamageSourceType source)
    {
        damage = damageAmount;
        damageSource = source;
        constantForceRef = gameObject.GetComponent<ConstantForce>();
        constantForceRef.force = direction * ForceToApply;
        coroutineRef = StartCoroutine(DelayDestroy());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponentInParent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, damageSource);
        }
        
        StopCoroutine(coroutineRef);
        Destroy(gameObject);
    }
    
    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }
}
