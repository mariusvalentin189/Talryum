using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossSwordThrowState : StateMachineBehaviour
{
    KnightBoss boss;
    float cooldown;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<KnightBoss>();
        boss.normalAttackCounter = -1;
        cooldown = Random.Range(boss.minSwordThrowCooldown, boss.maxSwordThrowCooldown);
        boss.SwordThrowAttack();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0f)
        {
            animator.SetTrigger("SwordThrow");
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("SwordThrow");
    }
}
