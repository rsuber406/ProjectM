using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "Spell System/ Spells /ProjectileSpell")]
public class ProjectileSpell : SpellBase
{
    [Header("Spell Properties")] public GameObject ProjectilePrefab;
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
        PlayerController playerControllerRef = player.GetComponent<PlayerController>();

        playerAnimRef.PlayAbilityByTriggerName(AbilityAnimationTriggerName);

        Transform cameraTransform = GameManager.GetInstance().GetPlayerCamera().transform;
        Vector3 direction = cameraTransform.forward.normalized;

        if (ProjectilePrefab)
        {
            Vector3 selectedOffset;

            if (playerControllerRef.inCombat)
            {
                selectedOffset = SpawnOffsetCombat;
            }
            else
            {
                selectedOffset = SpawnOffset;
                player.transform.GetChild(0).rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            }

            yield return new WaitForSeconds(spawnDelay);

            cameraTransform = GameManager.GetInstance().GetPlayerCamera().transform;
            Debug.Log(cameraTransform.rotation);
            Vector3 spawnPosition = playerControllerRef.HandSocket.position;
            if (GameManager.GetInstance().IsInCombatMode())
            {
                Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
                Vector3 worldTarget = (playerControllerRef.HandSocket.position) - Camera.main.ScreenToWorldPoint(screenCenter);
                Quaternion spawnRotation = Quaternion.LookRotation(worldTarget);
                spawnRotation *= Quaternion.Euler(-2.25f, 17.25f, 0);
                Instantiate(ProjectilePrefab, spawnPosition, spawnRotation);
            }
            else
            {
                Instantiate(ProjectilePrefab, spawnPosition, playerControllerRef.HandSocket.rotation);
            }
        }

        // Cast time , for some reason this is needed before we can call end
        yield return new WaitForSeconds(1);
        End();

        Cancel();
    }
}