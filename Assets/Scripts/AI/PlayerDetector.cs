using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField]AI enemyAi;
    HealthManager playerHealth;
    private void Start()
    {
        playerHealth = FindObjectOfType<HealthManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!playerHealth.Dead)
                enemyAi.FoundPlayer = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerHealth.Dead && enemyAi.FoundPlayer == true)
                enemyAi.FoundPlayer = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
                enemyAi.FoundPlayer = false;
    }
}
