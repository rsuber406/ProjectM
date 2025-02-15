using System.Collections;
using UnityEditor;
using UnityEngine;

public class DemonBossScript : EnemyAI
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Animator animationController;
    [Range(1, 8)] [SerializeField] private int animationChangeRate;

    [SerializeField] private SphereCollider leftHandMeleeCollider;
    [SerializeField] private SphereCollider rightHandMeleeCollider;
    // Update is called once per frame
     protected override void Update()
    {
        if (playerDetected)
        {
            float animSpeed = animationController.GetFloat("Speed");
            float speed = agent.velocity.magnitude;
            animationController.SetFloat("Speed", Mathf.MoveTowards(animSpeed, speed, Time.deltaTime * animationChangeRate));
        }
        else if (agent.remainingDistance < agentStoppingDistanceOrig)
        {
           // animationController.SetFloat("Speed", 0f);
        }
        
        base.Update();
    }

    protected override void AttackPlayer()
    {
        if (isAttacking) return;
        float distance = (playerPos - transform.position).magnitude;
        if (distance < agent.stoppingDistance)
        {
           // animationController.SetFloat("Speed", 0);
            
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animationController.SetTrigger("Rage");
        yield return new WaitForSeconds(3.767f);
        int randomAttack = Random.Range(0,2);
        switch (randomAttack)
        {
            case (int) AttackOptions.FirstAttack:

                StartCoroutine(AttackOne());
                break;
            case (int) AttackOptions.SecondAttack:
                StartCoroutine(AttackTwo());
                break;
            case (int) AttackOptions.ThirdAttack:
               
                break;
            case (int) AttackOptions.FourthAttack:
               
                break;
                
        }
        isAttacking = false;
    }

    private IEnumerator AttackOne()
    {
        animationController.SetTrigger("Attack1");
        // These are the times needed for animation to line up with attack
        yield return new WaitForSeconds(1.98f);
        leftHandMeleeCollider.enabled = true;
        yield return new WaitForSeconds(0.27f);

    }

    private IEnumerator AttackTwo()
    {
        animationController.SetTrigger("Attack2");
        yield return new WaitForSeconds(1.35f);
        rightHandMeleeCollider.enabled = true;
        leftHandMeleeCollider.enabled = true;
        yield return new WaitForSeconds(1.35f);

    }

    private void AttackThree()
    {
        
    }

    private void AttackFour()
    {
        
    }
    protected override void OnDeath()
    {
        animationController.SetTrigger("Death");
        agent.isStopped = true;
        Destroy(gameObject);
    }
}

enum AttackOptions : int
{
    FirstAttack = 0,
    SecondAttack,
    ThirdAttack,
    FourthAttack
}
