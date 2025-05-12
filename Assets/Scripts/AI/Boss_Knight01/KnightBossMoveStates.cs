using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossMoveStates : StateMachineBehaviour
{
    KnightBoss boss;
    float moveTime;
    float attackCooldown;
    int randomAttack;
    BossPlayerDetector playerDetector;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<KnightBoss>();
        attackCooldown = Random.Range(boss.minAttackCooldown, boss.maxAttackCooldown);
        if (boss.normalAttackCounter == -1)
            boss.normalAttackCounter = Random.Range(boss.minNormalAttacksCount, boss.maxNormalAttacksCount + 1);
        playerDetector = FindObjectOfType<BossPlayerDetector>();
        moveTime = Random.Range(boss.minMoveTime, boss.maxMoveTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.AIEnabled)
        {
            boss.FollowPlayer();

            if (boss.normalAttackCounter == 0)
            {
                randomAttack = Random.Range(0, 3);
                boss.normalAttackCounter--;
                if(randomAttack == 0)
                    animator.SetTrigger("Charge");
                else animator.SetTrigger("PrepareSwordThrow");
            }

            if (moveTime > 0)
            {
                if (animator.GetFloat("Speed") != 1)
                    animator.SetFloat("Speed", 1);
                boss.Move();
                moveTime -= Time.deltaTime;
            }
            else if (moveTime <= 0)
            {
                boss.normalAttackCounter = -1;
                animator.SetTrigger("PrepareSwordThrow");
            }
            if (playerDetector.nearPlayer)
            {
                if (boss.normalAttackCounter > 0)
                {
                    if (attackCooldown <= 0)
                    {
                        animator.SetTrigger("NormalAttack");
                    }
                    else attackCooldown -= Time.deltaTime;
                }
                else boss.StopMoving();
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("NormalAttack");
        animator.ResetTrigger("Charge");
        animator.ResetTrigger("PrepareSwordThrow");
    }
}
