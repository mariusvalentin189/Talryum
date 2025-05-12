using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Image healthBar;
    [SerializeField] DamageUI damageUI;
    [SerializeField] float invulnerabilityTime;
    [SerializeField] float knockbackTime;
    [SerializeField] SpriteRenderer[] playerSprites;
    [SerializeField] GameObject hitParticle;
    public bool Invulnerable { get; private set; }
    public bool Dead { get; private set; }
    public bool GodMode { get; set; }
    public int Deffence { get; set; }
    PlayerController player;
    GameCamera gc;
    int health;
    public int Health
    {
        get { return health; }
        set 
        { 
            health = value;
            health = Mathf.Clamp(value, 0, maxHealth);
            PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerHealth, health);
            healthText.text = health.ToString() + '/' + maxHealth.ToString();
            healthBar.fillAmount = (float)health / maxHealth;

        }
    }
    public int MaxHealth
    {
        get { return maxHealth; }
        set 
        { 
            maxHealth = value;
            healthBar.fillAmount = (float)health / maxHealth;
            healthText.text = health.ToString() + '/' + maxHealth.ToString();
            healthBar.fillAmount = (float)health / maxHealth;
        }
    }
    private void Start()
    {
        gc = FindObjectOfType<GameCamera>();
        player = GetComponent<PlayerController>();
    }
    public void TakeDamage(int damage, Vector2 direction)
    {
        if (!Invulnerable && !player.IsDodging && !player.AirAttack && !GodMode && !Dead)
        {
            foreach (SpriteRenderer sprite in playerSprites)
                sprite.color = Color.red;
            player.GotHit = true;
            gc.PlayGotHitAnimation();
            player.ApplyForce(direction);
            int finalDamage = 0;
            if ((damage - (int)((float)Deffence / 100 * damage)) > 0)
                finalDamage = damage - (int)((float)Deffence /100 * damage);
            Health -= finalDamage;
            Vector2 position = new Vector2(transform.position.x, transform.position.y + 1);
            DamageUI d = Instantiate(damageUI, position, Quaternion.identity);
            Instantiate(hitParticle, position, Quaternion.identity);
            d.ShowDamage(finalDamage);
            StartCoroutine(EndDamage());
            AudioManager.Instance.PlaySound(AudioManager.Instance.playerSwordHitFlesh);
            if (health <= 0)
            {
                Dead = true;
                player.Die();
                return;
            }
            Invulnerable = true;
            StartCoroutine(Vulnerable());
            StartCoroutine(EndKnockback());
        }
    }
    public void KillPlayer()
    {
        if (GodMode)
            return;
        Vector2 position = new Vector2(transform.position.x, transform.position.y + 1);
        DamageUI d = Instantiate(damageUI, position, Quaternion.identity);
        Instantiate(hitParticle, position, Quaternion.identity);
        AudioManager.Instance.PlaySound(AudioManager.Instance.playerSwordHitFlesh);
        d.ShowDamage(Health);
        Health -= Health;
        Dead = true;
        player.Die();
    }
    public void AddHealth(int value)
    {
        Health += value;
    }
    private void FixedUpdate()
    {
        if(Invulnerable)
        {
            foreach (SpriteRenderer sp in playerSprites)
                sp.enabled = !sp.enabled;
        }
    }
    IEnumerator Vulnerable()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        Invulnerable = false;
        foreach (SpriteRenderer sp in playerSprites)
            sp.enabled = true;
    }
    IEnumerator EndKnockback()
    {
        yield return new WaitForSeconds(knockbackTime);
        player.GotHit = false;
    }
    IEnumerator EndDamage()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer sprite in playerSprites)
            sprite.color = Color.white;
    }
    public void ResetScene()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerHealth, maxHealth);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadPlayerHealth()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerHealth))
            Health = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerHealth);
        else Health = maxHealth;
    }
}
