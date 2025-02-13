using System.Collections;
using UnityEditor;
using UnityEngine;

public class DemonBossScript : EnemyAI
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Animator animationController;
    [SerializeField] private int animationChangeRate;
    
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
        float distance = (playerPos - transform.position).magnitude;
        if (distance < agent.stoppingDistance)
        {
           // animationController.SetFloat("Speed", 0);
            animationController.SetTrigger("Rage");
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.3f);
        int randomAttack = Random.Range(0, 3);
        switch (randomAttack)
        {
            case (int) AttackOptions.FirstAttack:
                
                break;
            case (int) AttackOptions.SecondAttack:

                break;
            case (int) AttackOptions.ThirdAttack:

                break;
            case (int) AttackOptions.FourthAttack:

                break;
                
        }
        isAttacking = false;
    }

    private void AttackOne()
    {
        
    }

    private void AttackTwo()
    {
        
    }

    private void AttackThree()
    {
        
    }

    private void AttackFour()
    {
        
    }
}

enum AttackOptions : int
{
    FirstAttack = 0,
    SecondAttack,
    ThirdAttack,
    FourthAttack
}
