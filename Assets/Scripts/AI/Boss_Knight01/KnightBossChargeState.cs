using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossChargeState : StateMachineBehaviour
{
    KnightBoss boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<KnightBoss>();
        boss.normalAttackCounter = -1;
        boss.ChargeAttack();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("FinishCharge");
    }
}
