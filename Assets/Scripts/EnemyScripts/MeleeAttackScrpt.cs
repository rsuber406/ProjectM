using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackScrpt : MonoBehaviour
{
    [SerializeField] private int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        // Remove after debug


        IDamage dmg = other.GetComponentInParent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
            Debug.Log("Damage has been taken");
        }

        SphereCollider collider = this.GetComponent<SphereCollider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        else
        {
            BoxCollider boxCollider = this.GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }
    }
}