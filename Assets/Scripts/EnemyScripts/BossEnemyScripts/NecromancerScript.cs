using System.Collections;
using UnityEngine;

public class NecromancerScript : EnemyAI
{
    [SerializeField] private Animator animationController;
    [Range(1, 8)] [SerializeField] private int animationChangeRate;
    [SerializeField] private float spellCooldown;
    [SerializeField] private Transform spellCastPosition;
    [SerializeField] private SphereCollider weaponCollider;

    private float cooldownTimer;

    void Start()
    {
        cooldownTimer = spellCooldown;
    }

    void Update()
    {
        if (playerDetected)
        {
            float animSpeed = animationController.GetFloat("Speed");
            float speed = agent.velocity.magnitude;
            speed = speed * 0.5f;
            animationController.SetFloat("Speed",
                Mathf.MoveTowards(animSpeed, speed, Time.deltaTime * animationChangeRate));
        }

        base.Update();

        cooldownTimer += Time.deltaTime;
    }

    protected override void AttackPlayer()
    {
        if (isAttacking) return;
        float distance = (playerPos - transform.position).magnitude;

        if (distance < agent.stoppingDistance)
        {
            int randomAttack = Random.Range(0, 99);
            if (randomAttack < 70)
            {
                StartCoroutine(FirstMelee());
            }
            else StartCoroutine(SecondMelee());
        }
        else if (distance > range * 0.25f && cooldownTimer >= spellCooldown)
        {
            StartCoroutine(CastSpell());
            cooldownTimer = 0f;
        }
    }

    private IEnumerator FirstMelee()
    {
        isAttacking = true;
        weaponCollider.enabled = true;
        animationController.SetTrigger("Attack1");
        yield return new WaitForSeconds(1.2f);
        isAttacking = false;
    }

    private IEnumerator SecondMelee()
    {
        isAttacking = true;
        
        animationController.SetTrigger("Attack2");
        yield return new WaitForSeconds(1.25f);
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(2.05f);
        isAttacking = false;
    }

    private IEnumerator CastSpell()
    {
        // More logic needed for proper implementation
        isAttacking = true;
        animationController.SetTrigger("CastSpell");
        agent.isStopped = true;
        yield return new WaitForSeconds(3.3f);
        agent.isStopped = false;
        isAttacking = false;
    }
}

enum NecromancerAttack
{
    FirstMelee = 0,
    SecondMelee,
    CastSpell
}