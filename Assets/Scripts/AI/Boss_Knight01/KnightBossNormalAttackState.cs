using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossNormalAttackState : StateMachineBehaviour
{
    KnightBoss boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<KnightBoss>();
        boss.normalAttackCounter--;
        boss.NormalAttack();
    }
}
