using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnight02MoveStates : StateMachineBehaviour
{
    BossKnight02 boss;
    float moveTime;
    float attackCooldown;
    int randomAttack;
    BossPlayerDetector playerDetector;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossKnight02>();
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
            if (moveTime > 0)
            {
                if (animator.GetFloat("Speed") != 1)
                    animator.SetFloat("Speed", 1);
                boss.Move();
                moveTime -= Time.deltaTime;
            }
            else if (moveTime <= 0)
            {
                randomAttack = Random.Range(0, 2);
                animator.SetTrigger("Attack03");
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
                else
                {
                    animator.SetTrigger("Attack02");
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.StopMoving();
        animator.ResetTrigger("NormalAttack");
        animator.ResetTrigger("Attack02");
        animator.ResetTrigger("Attack03");
    }
}
