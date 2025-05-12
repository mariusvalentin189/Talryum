using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationEvents : MonoBehaviour
{
    [SerializeField]AI ai;
    public void StartAttack()
    {
        ai.StartAttack();
    }
    public void EndAttack()
    {
        ai.EndAttack();
    }
    public void CanMove()
    {
        ai.CanMove();
    }
    public void CanNotMove()
    {
        ai.CanNotMove();
    }
}
