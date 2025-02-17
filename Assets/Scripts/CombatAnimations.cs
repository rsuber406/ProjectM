using UnityEngine;
using UnityEngine.Playables;

public class CombatAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;

    bool inCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inCombat = false;
    }

    // Update is called once per frame
    void Update()
    {
        InCombatCheck();
        CombatAnimation();
    }

    void InCombatCheck()
    {
        if (Input.GetButtonDown("Combat"))
           inCombat = !inCombat;

        if (inCombat)
            anim.SetBool("inCombat", true);
        else
            anim.SetBool("inCombat", false);

    }

    void CombatAnimation()
    {
        //PlayerController.instance.playerState = PlayerController.State.idle;

        if (inCombat)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                anim.SetBool("Forward", true);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("Forward", false);
            }
        }
    }
}
