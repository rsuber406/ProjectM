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

    private float cooldownCompare;

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

    private void Start()
    {
        cooldownCompare = castCooldown;
    }

    protected override void Update()
    {
        if (playerDetected)
        {
            float animSpeed = animationController.GetFloat("Speed");
            float speed = agent.velocity.magnitude;
            animationController.SetFloat("Speed", Mathf.MoveTowards(speed, animSpeed, 2 * Time.deltaTime));
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
                StartCoroutine((MeleeAttack()));
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
       
        rightWeapon.enabled = false;
        leftWeapon.enabled = false;
        isAttacking = false;
    }

    // Update is called once per frame
}