using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerScript : EnemyAI
{
    [SerializeField] private Animator animationController;
    [Range(1, 8)] [SerializeField] private int animationChangeRate;
    [SerializeField] private float spellCooldown;
    [SerializeField] private Transform spellCastPosition;
    [SerializeField] private SphereCollider weaponCollider;
    [SerializeField] private List<GameObject> spells;
    

    private float cooldownTimer;

    void Start()
    {
        cooldownTimer = spellCooldown;
    }

    protected override void Update()
    {
        if (playerDetected)
        {
            
            float animSpeed = animationController.GetFloat("Speed");
            float speed = agent.velocity.magnitude;
            speed = speed * 0.5f;
            animationController.SetFloat("Speed",
                Mathf.MoveTowards(animSpeed, speed, Time.deltaTime * animationChangeRate));
        }
      
        Debug.DrawRay(spellCastPosition.position, AIController.GetAIController().GetPlayerPosition() - spellCastPosition.position, Color.green);
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
        int randomSpell = Random.Range(0, 99);
        yield return new WaitForSeconds(1.3f);
        Vector3 directionToPlayer = (AIController.GetAIController().GetPlayerPosition() - spellCastPosition.position);
        Quaternion rotationToApply = Quaternion.LookRotation(-directionToPlayer);
        spellCastPosition.rotation = rotationToApply;
      
        if (randomSpell > 10)
        {
          GameObject spell =  Instantiate(spells[0], spellCastPosition.position, spellCastPosition.rotation);
          
        }
        else AttackSpell(spells[1]);
        Debug.Log(spellCastPosition.rotation);
        yield return new WaitForSeconds(2.0f);
        agent.isStopped = false;
        isAttacking = false;
    }
    protected override IEnumerator OnDeath()
    {
        animationController.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        agent.isStopped = true;
        Destroy(gameObject);
    }
private void AttackSpell(GameObject spell)
{
    float distance = Vector3.Distance(transform.position, AIController.GetAIController().GetPlayerPosition());
    spell.transform.localScale = new Vector3(1f, 1f, distance);
   Instantiate(spell, spellCastPosition.position, spellCastPosition.rotation);
 
}

}


enum NecromancerAttack
{
    FirstMelee = 0,
    SecondMelee,
    CastSpell
}