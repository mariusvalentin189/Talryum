using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;
    [SerializeField] int maxHealth;
    [SerializeField] TMP_Text healthText;
    [SerializeField] DamageUI damageUI;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject healthUI;
    [SerializeField] float knockbackTime;
    [SerializeField] int expValue;
    SpriteRenderer[] sprites;
    [SerializeField] int coinsDrop;
    [SerializeField] GameObject hitParticle;
    [SerializeField] float hitParticleOffset;
    [SerializeField] float deffence;
    [SerializeField] EnemyAttackPoint attackPoint;
    public bool IsDead { get; private set; }
    AI ai;
    int health;
    GameCamera gc;
    float healthUITime = 5f;
    bool healthUIVisible;
    bool invulnerable;
    AudioManager am;
    Vector2 pos;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            health = Mathf.Clamp(value, 0, maxHealth);
            healthText.text = health.ToString() + '/' + maxHealth.ToString();
            healthBar.fillAmount = (float)health / maxHealth;
        }
    }
    void Start()
    {
        sprites = transform.GetComponentsInChildren<SpriteRenderer>();
        Health = maxHealth;
        gc = FindObjectOfType<GameCamera>();
        ai = GetComponent<AI>();
        HideHealthBar();
        am = AudioManager.Instance;
    }
    private void Update()
    {
        if(healthUIVisible)
        {
            if(healthUITime >= 0)
            {
                healthUITime -= Time.deltaTime;
            }
            else
            {
                HideHealthBar();
            }
        }
    }

    public void TakeDamage(int damage, Vector2 direction)
    {
        if (!invulnerable)
        {
            foreach (SpriteRenderer sprite in sprites)
                sprite.color = Color.red;
            ai.GotHit = true;
            gc.PlayHitEnemyAnimation();
            int finalDamage = 0;
            if ((damage - (int)(deffence / 100 * damage)) > 0)
                finalDamage = damage - (int)(deffence / 100 * damage);
            Health -= finalDamage;
            ai.FoundPlayer = true;
            DamageUI d = Instantiate(damageUI, healthBar.transform.position, Quaternion.identity);
            pos = new Vector2(transform.position.x, transform.position.y + hitParticleOffset);
            Instantiate(hitParticle, pos, Quaternion.identity);
            ShowHealthBar();
            d.ShowDamage(finalDamage);
            gc.PlayHitEnemyAnimation();
            ai.ApplyDelay();
            if (enemyType == EnemyType.flesh)
                am.PlaySound(am.playerSwordHitFlesh);
            else if (enemyType == EnemyType.stone)
                am.PlaySound(am.playerSwordHitStone);
            else if(enemyType == EnemyType.slime)
                am.PlaySound(am.playerSwordHitSlime);
            StartCoroutine(EndDamage());
            if (health <= 0)
            {
                IsDead = true;
                attackPoint.gameObject.SetActive(false);
                ai.Speed = 0;
                ai.StopMoving();
                ai.Anim.SetTrigger("Death");
                HideHealthBar();
                invulnerable = true;
            }
            else ai.ApplyForce(direction);
            StartCoroutine(EndKnockback());
        }
    }
    IEnumerator EndKnockback()
    {
        yield return new WaitForSeconds(knockbackTime);
        ai.GotHit = false;
    }
    void HideHealthBar()
    {
        healthUI.SetActive(false);
        healthUIVisible = false;
    }
    void ShowHealthBar()
    {
        healthUI.SetActive(true);
        healthUIVisible = true;
        healthUITime = 5f;
    }
    public void DesableEnemy()
    {
        foreach (SpriteRenderer sp in sprites)
        {
            Destroy(sp.gameObject);
        }
        FindObjectOfType<ExperienceSystem>().AddExperience(expValue);
        PlayerCoins.Instance.SpawnCoins(coinsDrop, transform.position);
        Destroy(gameObject);
    }
    IEnumerator EndDamage()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer sprite in sprites)
            sprite.color = Color.white;
    }
    public void Kill()
    {
        TakeDamage(Health, Vector2.zero);
    }

    public void DeathAnimation()
    {
        foreach (SpriteRenderer sp in sprites)
        {
            //sp.transform.SetParent(null,true);
            //sp.transform.position = new Vector3(sp.transform.position.x, sp.transform.position.y, 0f);
            sp.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            sp.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }
}
public enum EnemyType
{
    flesh,
    stone,
    slime
}
