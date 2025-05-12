using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int bossId;
    public virtual void EndFight() { }
    public virtual void StartStage2() { }
    public virtual void DisableBoss() { }
}
