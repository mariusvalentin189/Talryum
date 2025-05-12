using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnight02NormalAttackState : StateMachineBehaviour
{
    BossKnight02 boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossKnight02>();
        boss.normalAttackCounter--;
        boss.NormalAttack();
    }
}
