using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "Spell System/ Spells /ProjectileSpell")]
public class ProjectileSpell : SpellBase
{
    [Header("Spell Properties")]
    public GameObject ProjectilePrefab;
    public int damageAmount;
    public Vector3 SpawnOffset;
    public Vector3 SpawnOffsetCombat;
    public float spawnDelay;
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
        PlayerAnimation playerAnimRef = player.GetComponent<PlayerAnimation>();
        playerAnimRef.PlayAbilityByTriggerName(AbilityAnimationTriggerName);
        
        yield return new WaitForSeconds(spawnDelay);
        Transform cameraTransform = GameManager.GetInstance().GetPlayerCamera().transform;
        Vector3 direction = cameraTransform.forward.normalized;

        if (ProjectilePrefab)
        {

            Vector3 selectedOffset;

            if (player.GetComponent<PlayerController>().inCombat)
            {
                selectedOffset = SpawnOffsetCombat;
            }
            else
            {
                selectedOffset = SpawnOffset;
                player.transform.GetChild(0).rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            }
            
            Vector3 spawnPosition = player.transform.position + selectedOffset;
            
            GameObject projectile = Instantiate(ProjectilePrefab, spawnPosition, Quaternion.LookRotation(cameraTransform.forward));

            ProjectileBase projectileRef = projectile.GetComponent<ProjectileBase>();
            projectileRef.Init(direction, damageAmount, DamageSourceType.Player);
        }

        // Cast time , for some reason this is needed before we can call end
        yield return new WaitForSeconds(1);
        End();
        
        Cancel();
    }
}

