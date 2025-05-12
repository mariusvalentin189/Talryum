using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    bool collided;
    [SerializeField] float knockbackForce;
    [SerializeField] float range;
    [SerializeField] LayerMask enemiesLayer;
    [SerializeField] PlayerController player;
    int minDamage;
    int maxDamage;
    public int MinDamage
    {
        get { return minDamage; }
        set { minDamage = value; }
    }
    public int MaxDamage
    {
        get { return maxDamage; }
        set { maxDamage = value; }
    }
    private void OnEnable()
    {
        collided = false;
    }
    private void OnDisable()
    {
        collided = false;
    }
    private void Update()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemiesLayer);
        if (!collided && enemies.Length > 0)
        {
            foreach (Collider2D enemy in enemies)
            {
                if (enemy.gameObject.CompareTag("Enemy"))
                {
                    EnemyHealthManager health = enemy.GetComponent<EnemyHealthManager>();
                    DamageEnemy(health);
                }
                else if (enemy.gameObject.CompareTag("BossEnemy"))
                {
                    BossHealthManager health = enemy.GetComponent<BossHealthManager>();
                    DamageEnemy(health);
                }
            }
            collided = true;
        }
    }
    void DamageEnemy(EnemyHealthManager health)
    {
        int damage = Random.Range(minDamage + player.Strength, maxDamage + player.Strength + 1);
        Vector2 direction = health.transform.position - player.transform.position;
        direction = new Vector2(direction.x, 0f);
        if (player.AirAttack)
            health.TakeDamage(2 * damage, direction * knockbackForce);
        else health.TakeDamage(damage, direction * knockbackForce);
    }

    void DamageEnemy(BossHealthManager health)
    {
        int damage = Random.Range(minDamage + player.Strength, maxDamage + player.Strength + 1);
        Vector2 direction = health.transform.position - player.transform.position;
        direction = new Vector2(direction.x, 0f);
        if (player.AirAttack)
            health.TakeDamage(2 * damage, direction * knockbackForce);
        else health.TakeDamage(damage, direction * knockbackForce);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
