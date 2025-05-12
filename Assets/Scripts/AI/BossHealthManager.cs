using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthManager : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Image healthBar;
    [SerializeField] DamageUI damageUI;
    [SerializeField] float knockbackTime;
    [SerializeField] int expValue;
    SpriteRenderer[] sprites;
    [SerializeField] int coinsDrop;
    [SerializeField] GameObject hitParticle;
    [SerializeField] float deffence;
    public bool Invulnerable { get; set; }
    Boss ai;
    GameCamera gc;
    bool dead;
    int health;
    bool isInStage2;
    AudioManager am;
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
    public float Deffence
    {
        get { return deffence; }
        set { deffence = value; }
    }
    void Start()
    {
        sprites = transform.GetComponentsInChildren<SpriteRenderer>();
        gc = FindObjectOfType<GameCamera>();
        Health = maxHealth;
        ai = GetComponent<Boss>();
        am = AudioManager.Instance;
    }
    public void TakeDamage(int damage, Vector2 direction)
    {
        if (!dead && !Invulnerable)
        {
            gc.PlayHitEnemyAnimation();
            int finalDamage = 0;
            if ((damage - (int)(Deffence / 100 * damage)) > 0)
                finalDamage = damage - (int)(Deffence / 100 * damage);
            Health -= finalDamage;
            Vector2 position = new Vector2(transform.position.x, transform.position.y + 2);
            DamageUI d = Instantiate(damageUI, position, Quaternion.identity);
            Instantiate(hitParticle, position, Quaternion.identity);
            d.ShowDamage(finalDamage);
            am.PlaySound(am.playerSwordHitFlesh);
            StartCoroutine(EndDamage());
            if (health <= 0)
            {
                dead = true;
                ai.EndFight();
            }
            else if (health <= maxHealth / 2 && !isInStage2)
            {
                ai.StartStage2();
                isInStage2 = true;
            }
        }
    }
    IEnumerator EndDamage()
    {
        foreach (SpriteRenderer sp in sprites)
            sp.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        foreach (SpriteRenderer sp in sprites)
            sp.color = Color.white;
    }


    public void ChangeSpritesToRed()
    {
        foreach (SpriteRenderer sp in sprites)
            sp.color = Color.red;
    }
    public void ChangeSpritesToWhite()
    {
        foreach (SpriteRenderer sp in sprites)
            sp.color = Color.white;
    }
    public void AddExperience()
    {
        FindObjectOfType<ExperienceSystem>().AddExperience(expValue);
        PlayerCoins.Instance.SpawnCoins(coinsDrop, transform.position);
    }
}
