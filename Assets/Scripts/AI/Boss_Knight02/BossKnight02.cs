using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnight02 : Boss
{
    public float minMoveTime;
    public float maxMoveTime;
    public float minAttackCooldown;
    public float maxAttackCooldown;
    public int minNormalAttacksCount;
    public int maxNormalAttacksCount;
    public bool AIEnabled { get; private set; }
    public int normalAttackCounter { get; set; } = -1;
    [SerializeField] DialogueSystem dialogue;
    [SerializeField] Transform UI;
    [SerializeField] float moveSpeed;
    [SerializeField] float stage2SpeedMultiplier;
    [SerializeField] float stage2DeffenceDecrease;
    [SerializeField] float attack02RunSpeed;
    [SerializeField] float attack03RunSpeed;
    [SerializeField] Transform obstacleDetector;
    [SerializeField] float obstacleDetectDistance;
    [SerializeField] LayerMask obstacleLayers;
    [SerializeField] GameObject healthBar;
    [SerializeField] BossWeapon weapon;
    [SerializeField] int normalDamage;
    [SerializeField] int attack02Damage;
    [SerializeField] int attack03Damage;
    [SerializeField] BoxCollider2D throneRoomDoor;
    [SerializeField] GameObject throneRoomCheckpoint;
    [SerializeField] GameObject king;

    [Header("Sounds")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip step;
    [SerializeField] AudioClip swordSwing;
    [SerializeField] AudioClip fall01;
    [SerializeField] AudioClip fall02;

    Animator anim;
    BossHealthManager healthManager;
    Rigidbody2D rb;
    GameCamera gc;
    Transform target;
    float speed;
    bool levelStarted;
    bool movingRight;
    bool attack02;
    bool attack03;
    HealthManager playerHealth;
    void Awake()
    {
        playerHealth = FindObjectOfType<HealthManager>();
        healthBar.SetActive(false);
        UI.SetParent(null);
        throneRoomDoor.enabled = false;
        throneRoomCheckpoint.SetActive(false);
    }
    void Start()
    {
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
        if(AIEnabled && playerHealth.Dead)
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
        BossLevelManager.Instance.EnableObjectsOnEntrance();
        anim.SetTrigger("Entrance");
        anim.ResetTrigger("BeginDialogue");
        anim.speed = 1;
    }
    public void NormalAttack()
    {
        weapon.Damage = normalDamage;
        StopMoving();
    }
    public void EnableEntrance()
    {
        throneRoomDoor.enabled = true;
        throneRoomCheckpoint.SetActive(true);
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
            else StopMoving();
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
    public void Vulnerable(bool vulnerable)
    {
        healthManager.Invulnerable = !vulnerable;
    }
    public void StopMoving()
    {
        speed = 0;
        anim.SetFloat("Speed", 0);
        rb.velocity = new Vector2(0f, rb.velocity.y) * Time.fixedDeltaTime;
    }
    #region Animation Events

    public void PlayStepSound()
    {
        soundSource.PlayOneShot(step);
    }
    public void PlaySwingSound()
    {
        soundSource.PlayOneShot(swordSwing);
    }
    public void PlayFall01Sound()
    {
        soundSource.PlayOneShot(fall01); 
    }
    public void PlayFall02Sound()
    {
        soundSource.PlayOneShot(fall02); 
    }

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
        anim.speed = stage2SpeedMultiplier;
        moveSpeed *= stage2SpeedMultiplier;
        attack02RunSpeed *= stage2SpeedMultiplier;
        attack02RunSpeed *= stage2SpeedMultiplier;
        normalDamage = (int)((float)normalDamage * stage2SpeedMultiplier);
        attack02Damage = (int)((float)attack02Damage * stage2SpeedMultiplier);
        attack03Damage = (int)((float)attack03Damage * stage2SpeedMultiplier);
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
    public void Attack02()
    {
        if (AIEnabled)
        {
            if (!attack02)
                StopMoving();
            else
            {
                weapon.Damage = attack02Damage;
                if (speed != attack02RunSpeed)
                    speed = attack02RunSpeed;
                rb.velocity = speed * 100 * Time.fixedDeltaTime * -transform.right;
            }
        }
    }
    public void BeginAttack02()
    {
        attack02 = true;
        EnableSwordCollider();
    }
    public void EndAttack02()
    {
        attack02 = false;
    }

    public void BeginAttack03()
    {
        attack03 = true;
    }
    public void EndAttack03()
    {
        attack03 = false;
    }
    public void Attack03()
    {
        if (AIEnabled)
        {
            if (!attack03)
                StopMoving();
            else
            {
                weapon.Damage = attack03Damage;
                if (speed != attack03RunSpeed)
                    speed = attack03RunSpeed;
                rb.velocity = speed * 100 * Time.fixedDeltaTime * -transform.right;
            }
        }
    }
    void EndAttacks()
    {
        attack02 = false;
        attack03 = false;
        StopMoving();
    }
    #endregion

    #region Inherited

    public override void DisableBoss()
    {
        EnableEntrance();
        Destroy(king);
        Destroy(gameObject);
    }
    public override void EndFight()
    {
        healthBar.SetActive(false);
        FollowPlayer();
        EndAttacks();
        FindObjectOfType<DialogueTrigger>().SaveData();
        weapon.enabled = false;
        anim.ResetTrigger("NormalAttack");
        anim.ResetTrigger("Attack02");
        anim.ResetTrigger("Attack03");
        anim.SetTrigger("Death");
    }
    public override void StartStage2()
    {
        StopMoving();
        anim.SetTrigger("Stage2");
        GetComponent<BossHealthManager>().Invulnerable = true;
    }
    #endregion
}
