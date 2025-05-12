using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPoint : MonoBehaviour
{
    [SerializeField] float knockbackForce;
    [SerializeField] AI ai;
    bool collided;
    int damage;
    private void Start()
    {
        damage = ai.damage;
    }

    private void Update()
    {
        collided = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collided)
            {
                HealthManager health = collision.GetComponent<HealthManager>();
                Vector2 direction = health.transform.position - ai.transform.position;
                direction = new Vector2(direction.x, 0f);
                health.TakeDamage(damage, direction * knockbackForce);
                collided = true;
            }
        }
    }
}
