using System.Collections;
using UnityEngine;

public class SkeletonScript : EnemyAI
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Animator animationController;
    [Range(1, 8)] [SerializeField] private int animationChangeRate;
    [SerializeField] private BoxCollider weaponCollider;

    private Coroutine attackCo;

    // Update is called once per frame
    protected override void Update()
    {
        if (playerDetected)
        {
            float animSpeed = animationController.GetFloat("Speed");
            float speed = agent.velocity.magnitude;
            animationController.SetFloat("Speed",
                Mathf.MoveTowards(animSpeed, speed, Time.deltaTime * animationChangeRate));
        }
        else
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                animationController.SetFloat("Speed", 0);
            }
        }

        base.Update();
    }


    protected override void AttackPlayer()
    {
        if (isAttacking) return;
        if (AIController.GetAIController().CanAttackPlayer())
        {
            float distance = (playerPos - transform.position).magnitude;
            if (distance < agent.stoppingDistance)
            {
                attackCo = StartCoroutine(Attack());
            }

            AIController.GetAIController().RemoveFromAttackQue();
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animationController.SetTrigger("Attack");

        yield return new WaitForSeconds(0.75f);
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        weaponCollider.enabled = false;
        yield return null;
    }

    protected override IEnumerator OnDeath()
    {
        if(attackCo != null)
        StopCoroutine(attackCo);
        weaponCollider.enabled = false;
        animationController.SetTrigger("Death");
        agent.isStopped = true;
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}