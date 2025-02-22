using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InstaKillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy") && !other.CompareTag("Player")) return;
        
        IDamage dmg = other.gameObject.GetComponentInChildren<IDamage>();
        if (dmg == null)
        {
            dmg = other.gameObject.GetComponentInParent<IDamage>();
        }

        if (dmg == null)
        {
            Debug.Log($"Entity {other.name} Entered InstaKillBox But did not die");
            return;
        }
        
        dmg.TakeDamage(9999999, DamageSourceType.World);
        Debug.Log("InstaKillBox Activated");
    }
}
