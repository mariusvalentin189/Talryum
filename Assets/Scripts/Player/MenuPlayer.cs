using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] int minAnimations;
    [SerializeField] int maxAnimations;
    [SerializeField] Animator anim;
    int animationsCount;
    void Start()
    {
        animationsCount = Random.Range(minAnimations, maxAnimations + 1);
    }
    public void FinishAnimation()
    {
        animationsCount -= 1;
        if(animationsCount==0)
        {
            anim.SetTrigger("ShowAtEnemy");
            animationsCount = Random.Range(minAnimations, maxAnimations + 1);
        }
    }
}
