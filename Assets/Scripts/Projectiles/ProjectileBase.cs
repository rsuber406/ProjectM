using System.Collections;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float Lifetime;
    public float ForceToApply;
    private ConstantForce constantForceRef;
    private Coroutine coroutineRef;
    
    public void Init(Vector3 direction)
    {
        constantForceRef = gameObject.GetComponent<ConstantForce>();
        constantForceRef.force = direction * ForceToApply;
        coroutineRef = StartCoroutine(DelayDestroy());
    }

    void OnTriggerEnter(Collider other)
    {
        StopCoroutine(coroutineRef);
        Destroy(gameObject);
    }
    
    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }
}
