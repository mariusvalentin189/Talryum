using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveStates : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    AI ai;
    PlayerController player;
    float attackCooldown;
    RaycastHit2D[] rays;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        player = FindObjectOfType<PlayerController>();
        attackCooldown = ai.attackCooldown;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai.IsGrounded())
        {
            rays = ai.DetectObjects();
            if (rays[0] || !rays[2])
            {
                ai.transform.localScale = new Vector3(-ai.transform.localScale.x, ai.transform.localScale.y, ai.transform.localScale.z);
                ai.SetSpeed();
            }
            if (!rays[0] && !rays[1] && rays[2])
            {
                if (ai.FoundPlayer)
                {
                    if (player.transform.position.x < ai.transform.position.x)
                    {
                        if (ai.transform.localScale.x > 0)
                        {
                            ai.transform.localScale = new Vector3(-ai.transform.localScale.x, ai.transform.localScale.y, ai.transform.localScale.z);
                            ai.SetSpeed();
                        }
                        if (ai.transform.position.x - player.transform.position.x <= ai.playerDetectCollider.size.x / 2)
                        {
                            ai.StopMoving();
                            Attack(animator);
                        }
                    }
                    else if (player.transform.position.x > ai.transform.position.x)
                    {
                        if (ai.transform.localScale.x < 0)
                        {
                            ai.transform.localScale = new Vector3(-ai.transform.localScale.x, ai.transform.localScale.y, ai.transform.localScale.z);
                            ai.SetSpeed();
                        }
                        if (player.transform.position.x - ai.transform.position.x <= ai.playerDetectCollider.size.x / 2)
                        {
                            ai.StopMoving();
                            Attack(animator);
                        }
                    }
                }
                else ai.CanMove();
            }
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("NormalAttack");
    }
    void Attack(Animator anim)
    {
        if (attackCooldown <= 0)
        {
            anim.SetTrigger("NormalAttack");
        }
        else attackCooldown -= Time.deltaTime;
    }
}
