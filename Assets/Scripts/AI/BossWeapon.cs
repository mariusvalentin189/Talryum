using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    [SerializeField] float knockbackForce;
    BoxCollider2D coll;
    public int Damage { get; set; }
    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;
    }
    public void ChangeColliderState(bool enabled)
    {
        coll.enabled = enabled;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            coll.enabled = false;
            HealthManager health = collision.GetComponent<HealthManager>();
            Vector2 direction = health.transform.position - transform.position;
            direction = new Vector2(direction.x, 0f);
            health.TakeDamage(Damage, direction * knockbackForce);
        }
    }
}
