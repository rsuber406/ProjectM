using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    enum DamageType
    {
        moving
        ,stationary
    }
    [SerializeField] DamageType type;

    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == DamageType.moving)
        {
            rb.linearVelocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponentInParent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damageAmount);
        }

        if (type == DamageType.moving)
        {
            Destroy(gameObject);
        }
    }
}
