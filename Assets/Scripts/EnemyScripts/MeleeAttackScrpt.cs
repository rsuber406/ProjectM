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
        if (other.CompareTag("Player"))
        {
            
        }
        Debug.Log(other.tag);
        
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        SphereCollider collider = this.GetComponent<SphereCollider>();
        collider.enabled = false;
    }
}
