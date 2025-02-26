using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class GhoulScript : EnemyAI
{
    [SerializeField] private Animator animationController;

    [SerializeField] private float castTime;

    [SerializeField] private float castCooldown;

    [SerializeField] private BoxCollider rightWeapon;

    [SerializeField] private BoxCollider leftWeapon;

    [Range(1, 8)] [SerializeField] private int animationChangeRate;
    private float cooldownCompare;

    private Coroutine attackCo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected IEnumerator RangeAttackPlayer()
    {
        isAttacking = true;
        animationController.SetBool("Cast Spell", true);
        yield return new WaitForSeconds(castTime);
        animationController.SetBool("Cast Spell", false);
        AIController.GetAIController().RemoveFromAttackQue();
        isAttacking = false;
    }

    protected override void Start()
    {
        cooldownCompare = castCooldown;
        base.Start();
    }

    protected override void Update()
    {
        if (playerDetected)
        {
            float animSpeed = animationController.GetFloat("Speed");
            float speed = agent.velocity.magnitude;
            animationController.SetFloat("Speed",
                Mathf.MoveTowards(speed, animSpeed, animationChangeRate * Time.deltaTime));
        }
        else
        {
            if (agent.remainingDistance < agentStoppingDistanceOrig)
            {
                animationController.SetFloat("Speed", 0);
            }
        }

        base.Update();
    }

    protected override void AttackPlayer()
    {
        if (AIController.GetAIController().CanAttackPlayer())
        {
            float distance = (playerPos - transform.position).magnitude;

            if (!isAttacking && distance <= agent.stoppingDistance)
            {
                Debug.Log("Can attack");
                attackCo = StartCoroutine((MeleeAttack()));
            }

            AIController.GetAIController().RemoveFromAttackQue();
        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        rightWeapon.enabled = true;
        leftWeapon.enabled = true;
        animationController.SetTrigger("Attack");

        yield return new WaitForSeconds(2f);

        isAttacking = false;
        rightWeapon.enabled = false;
        leftWeapon.enabled = false;
    }

    protected override IEnumerator OnDeath()
    {
        if(attackCo != null)
        StopCoroutine(attackCo);
        leftWeapon.enabled = false;
        rightWeapon.enabled = false;
        animationController.SetTrigger("Death");
        agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    // Update is called once per frame
}