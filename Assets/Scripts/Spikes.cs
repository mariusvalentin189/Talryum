using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            HealthManager h = collision.GetComponent<HealthManager>();
            if (!h.Dead)
            {
                h.KillPlayer();
            }
        }
        else if(collision.CompareTag("Enemy"))
        {
            EnemyHealthManager e = collision.GetComponent<EnemyHealthManager>();
            if (!e.IsDead)
            {
                e.Kill();
            }
        }
    }
}
