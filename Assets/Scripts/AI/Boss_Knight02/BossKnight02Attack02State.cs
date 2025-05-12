using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnight02Attack02State : StateMachineBehaviour
{
    BossKnight02 boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossKnight02>();
        boss.normalAttackCounter--;

    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.Attack02();
    }
}
