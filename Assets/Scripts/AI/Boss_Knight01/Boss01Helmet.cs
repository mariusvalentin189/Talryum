using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01Helmet : MonoBehaviour
{
    [SerializeField] float knockbackForce;
    [SerializeField] int damage;
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }
    BoxCollider2D coll;
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
        if (collision.CompareTag("Player"))
        {
            HealthManager health = collision.GetComponent<HealthManager>();
            Vector2 direction = health.transform.position - transform.position;
            direction = new Vector2(direction.x, 0f);
            health.TakeDamage(damage, direction * knockbackForce);
        }
    }
}
