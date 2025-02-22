using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    [SerializeField] ParticleSystem lightningStrike;
    [SerializeField] private int damage;

    private void Start()
    {
        StartCoroutine(CastLighting(AIController.GetAIController().GetPlayerPosition()));
    }

    public IEnumerator CastLighting(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;
        RaycastHit hit;
        Debug.DrawRay(transform.position, direction, Color.blue);
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.point.magnitude < distance)
            {
              
                
                IDamage dmg = hit.collider.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.TakeDamage(damage, DamageSourceType.Enemy);
                }

                yield return new WaitForSeconds(0.5f);
                Destroy(gameObject);
            }
        }
    }
}
