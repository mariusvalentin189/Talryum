using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBoss : Boss
{
    public float minMoveTime;
    public float maxMoveTime;
    public float minAttackCooldown;
    public float maxAttackCooldown;
    public float minChargeRunCooldown;
    public float maxChargeRunCooldown;
    public float minSwordThrowCooldown;
    public float maxSwordThrowCooldown;
    public int minNormalAttacksCount;
    public int maxNormalAttacksCount;
    public bool AIEnabled { get; private set; }
    public int normalAttackCounter { get; set; } = -1;
    [SerializeField] float chargeSpeed;
    [SerializeField] float chargeRunTime;
    [SerializeField] Boss01Helmet helmet;
    [SerializeField] DialogueSystem dialogue;
    [SerializeField] Transform UI;
    [SerializeField] float moveSpeed;
    [SerializeField] float stage2SpeedMultiplier;
    [SerializeField] float stage2DeffenceDecrease;
    [SerializeField] Transform obstacleDetector;
    [SerializeField] float obstacleDetectDistance;
    [SerializeField] LayerMask obstacleLayers;
    [SerializeField] GameObject healthBar;
    [SerializeField] BossWeapon weapon;
    [SerializeField] int normalDamage;
    [SerializeField] int swordThrowDamage;

    [Header("Sounds")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip land;
    [SerializeField] AudioClip step;
    [SerializeField] AudioClip swordSwing;
    [SerializeField] AudioClip swordThrow;
    [SerializeField] AudioClip fall;
    Animator anim;
    BossHealthManager healthManager;
    Rigidbody2D rb;
    GameCamera gc;
    Transform target;
    float speed;
    bool levelStarted;
    bool movingRight;
    HealthManager playerHealth;
    void Awake()
    {
        healthBar.SetActive(false);
        UI.SetParent(null);
    }
    void Start()
    {
        playerHealth = FindObjectOfType<HealthManager>();
        healthManager = GetComponent<BossHealthManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gc = FindObjectOfType<GameCamera>();
        target = FindObjectOfType<PlayerController>().transform;
    }
    void Update()
    {
        if (dialogue.IsInDialogue && !levelStarted)
        {
            StartCoroutine(StartBossLevel());
        }
        if (AIEnabled && playerHealth.Dead)
        {
            StopMoving();
            AIEnabled = false;
            anim.SetTrigger("Victory");
        }
    }
    IEnumerator StartBossLevel()
    {
        levelStarted = true;
        anim.SetTrigger("BeginDialogue");
        gc.StartBossLevel = true;
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayBossMusic();
        dialogue.StartDialogue();
        yield return new WaitUntil(() => dialogue.FinishedDialogue == true);
        EnableBoss();
    }
    void EnableBoss()
    {
        dialogue.FinishedDialogue = false;
        BossLevelManager.Instance.DisableObjects();
        anim.SetTrigger("Entrance");
        anim.ResetTrigger("BeginDialogue");
        anim.speed = 1;
    }
    public void NormalAttack()
    {
        weapon.Damage = normalDamage;
        StopMoving();
    }
    public void SwordThrowAttack()
    {
        speed = 0;
        weapon.Damage = swordThrowDamage;
        if (anim.GetFloat("Speed") != 0)
            anim.SetFloat("Speed", 0);
        rb.velocity = new Vector2(0f, rb.velocity.y) * Time.fixedDeltaTime;
    }
    IEnumerator ChargeRun()
    {
        anim.SetFloat("Speed", 0);
        rb.velocity = new Vector2(0f, rb.velocity.y) * Time.fixedDeltaTime;
        speed = 0;
        float chargeRunCooldown = Random.Range(minChargeRunCooldown, maxChargeRunCooldown);
        yield return new WaitForSeconds(chargeRunCooldown);
        EnableHelmetCollider();
        anim.SetTrigger("ChargeRun");
        yield return new WaitForSeconds(chargeRunTime);
        anim.SetTrigger("FinishCharge");
        DisableHelmetCollider();
        StopMoving();
        FinishChargeRun();
    }
    public void EnableHelmetCollider()
    {
        helmet.ChangeColliderState(true);
    }
    public void DisableHelmetCollider()
    {
        helmet.ChangeColliderState(false);
    }
    public void EnableEntrance()
    {
        BossLevelManager.Instance.EnableEntrance();
        AudioManager.Instance.ResumeMusic();
        PlayerPrefs.SetInt(PlayerPrefsKeys.BossId + bossId, 1);
    }
    public void Move()
    {
        if (AIEnabled)
        {
            if (speed != moveSpeed)
                speed = moveSpeed;
            if (Mathf.Abs(transform.position.x - target.transform.position.x) > 1f)
                rb.velocity = speed * 100 * Time.fixedDeltaTime * -transform.right;
            else rb.velocity = new Vector2(0f, rb.velocity.y) * Time.fixedDeltaTime;
            HandleDetections();
        }
    }
    public void Charge()
    {
        if (AIEnabled)
        {
            if(healthManager.Health <= 0 && speed >= 0)
            {
                StopMoving();
                return;
            }
            if (speed != chargeSpeed)
                speed = chargeSpeed;
            rb.velocity = speed * 100 * Time.fixedDeltaTime * -transform.right;
            HandleDetections();
        }
    }
    public void FollowPlayer()
    {
        if (target.transform.position.x < transform.position.x && movingRight == true)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            movingRight = false;
        }
        else if (target.transform.position.x > transform.position.x && movingRight == false)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            movingRight = true;
        }
    }
    public void HandleDetections()
    {
        RaycastHit2D hitObstacle;
        hitObstacle = Physics2D.Raycast(obstacleDetector.position, transform.right, obstacleDetectDistance, obstacleLayers);
        if (hitObstacle)
        {
            if (!movingRight)
            {
                movingRight = true;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                obstacleDetector.gameObject.SetActive(false);
            }
            else
            {
                movingRight = false;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                obstacleDetector.gameObject.SetActive(false);
            }
        }
        else if (!obstacleDetector.gameObject.activeSelf)
        {
            obstacleDetector.gameObject.SetActive(true);
        }
    }
    public void ChargeAttack()
    {
        StartCoroutine(ChargeRun());
    }
    public void Vulnerable(bool vulnerable)
    {
        healthManager.Invulnerable = !vulnerable;
    }
    void FinishChargeRun()
    {
        if (target.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            movingRight = false;
        }
        else if (target.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            movingRight = true;
        }
    }
    public void StopMoving()
    {
        speed = 0;
        anim.SetFloat("Speed", 0);
        rb.velocity = new Vector2(0f, rb.velocity.y) * Time.fixedDeltaTime;
    }
    #region Animation Events

    public void FinishEntrance()
    {
        AIEnabled = true;
        healthBar.SetActive(true);
        gc.FollowPlayer();
    }
    public void ShakeCamera()
    {
        gc.PlayGotHitAnimation2();
    }
    public void ShakeCameraHit()
    {
        gc.PlayGotHitAnimation();
    }
    public void PlayLandSound()
    {
        soundSource.PlayOneShot(land);
    }
    public void PlayStepSound()
    {
        soundSource.PlayOneShot(step);
    }
    public void PlaySwingSound()
    {
        soundSource.PlayOneShot(swordSwing);
    }
    public void PlaySowrdThrowSound()
    {
        soundSource.PlayOneShot(swordThrow);
    }
    public void PlayFallSound()
    {
        soundSource.PlayOneShot(fall);
    }
    public void Idle()
    {
        if (speed != 0)
        {
            speed = 0;
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
    public void EnterStage2()
    {
        FollowPlayer();
        chargeSpeed *= stage2SpeedMultiplier;
        moveSpeed *= stage2SpeedMultiplier;
        anim.speed = stage2SpeedMultiplier;
        normalDamage = (int)((float)normalDamage * stage2SpeedMultiplier);
        swordThrowDamage = (int)((float)swordThrowDamage * stage2SpeedMultiplier);
        helmet.Damage = (int)((float)helmet.Damage * stage2SpeedMultiplier);
        healthManager.Deffence -= stage2DeffenceDecrease;
        GetComponent<BossHealthManager>().Invulnerable = false;
    }
    public void EnableSwordCollider()
    {
        weapon.ChangeColliderState(true);
    }
    public void DisableSwordCollider()
    {
        weapon.ChangeColliderState(false);
    }
    public void FinishAttack()
    {
        healthManager.Invulnerable = false;
    }

    #endregion

    #region Inherited

    public override void DisableBoss()
    {
        EnableEntrance();
        Destroy(gameObject);
    }
    public override void EndFight()
    {
        anim.ResetTrigger("ChargeRun");
        anim.ResetTrigger("Charge");
        anim.ResetTrigger("FinishCharge");
        anim.ResetTrigger("PrepareSwordThrow");
        anim.ResetTrigger("SwordThrow");
        anim.ResetTrigger("NormalAttack");
        anim.ResetTrigger("FinishCharge");
        anim.SetTrigger("Death");
        helmet.enabled = false;
        weapon.enabled = false;
        StopMoving();
        FindObjectOfType<DialogueTrigger>().SaveData();
    }
    public override void StartStage2()
    {
        StopMoving();
        anim.SetTrigger("Stage2");
        GetComponent<BossHealthManager>().Invulnerable = true;
    }
    
    #endregion
}
