using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class GhoulScript : EnemyAI
{
    [SerializeField] private Animator animationController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected IEnumerator RangeAttackPlayer()
    {
        animationController.SetBool("Cast Spell", true);
        yield return new WaitForSeconds(1f);
        animationController.SetBool("Cast Spell", false);
        AIController.GetAIController().RemoveFromAttackQue();
        isAttacking = false;
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
                animationController.SetFloat("Speed",0);
            }
        }
        base.Update();
        
    }

    protected override void AttackPlayer()
    {
        if (AIController.GetAIController().CanAttackPlayer())
        {
           
            Debug.Log("Ghoul Attack was called");
            if (!isAttacking)
            {
                StartCoroutine((MeleeAttack()));
            }

        }
    }

    private IEnumerator MeleeAttack()
    {
        Debug.Log("Melee was called");
        isAttacking = true;
        animationController.SetTrigger("Attack");
        yield return new WaitForSeconds(2f);
        animationController.SetTrigger("Attack");
        AIController.GetAIController().RemoveFromAttackQue();
        isAttacking = false;
        
    }

    // Update is called once per frame
   
}
