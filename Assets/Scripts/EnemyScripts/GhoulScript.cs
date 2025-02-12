using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class GhoulScript : EnemyAI
{
    [SerializeField] private Animator animationController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override IEnumerator RangeAttackPlayer()
    {
        animationController.SetBool("Cast Spell", true);
        yield return new WaitForSeconds(1f);
        animationController.SetBool("Cast Spell", false);
        isAttacking = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter is called in derived object");
        base.OnTriggerEnter(other);
    }

    protected override void Update()
    {
        if (playerDetected)
        {
            float animSpeed = animationController.GetFloat("Speed");
            float speed = agent.velocity.magnitude;
            animationController.SetFloat("Speed", animSpeed, speed, Time.deltaTime * 2);
        }
        base.Update();
        
    }

    // Update is called once per frame
   
}
