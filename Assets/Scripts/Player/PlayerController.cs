using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region variables
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float dodgeSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float wallJumpForce;
    [SerializeField] Transform groundDetector;
    [SerializeField] float canWallJumpTime;
    [SerializeField] bool movingRight;
    [SerializeField] bool movingLeft;
    [SerializeField] float groundDetectorWidth;
    public Animator anim;

    [Header("Layers")]
    [SerializeField] LayerMask whatIsWalkable;
    [SerializeField] LayerMask whatIsGrass;
    [SerializeField] LayerMask whatIsStone;
    [SerializeField] LayerMask whatIsWood;
    [SerializeField] LayerMask whatIsGround;

    [Header("Player Objects")]
    [SerializeField] GameObject lineObject;
    [SerializeField] GameObject playerSprite;
    [SerializeField] Transform feet;
    [SerializeField] Sword weapon;

    [Header("Effects")]
    [SerializeField] ParticleSystem[] moveEffects;
    [SerializeField] ParticleSystem[] jumpEffects;

    [Header("UI")]
    [SerializeField] Transform playerUI;

    [Header("Public variables")]
    public bool kneelOnDialogue;

    //private variables
    DialogueSystem dialogue;
    Vector2 moveDirection;
    Vector2 forceDirection;
    Rigidbody2D rb;
    float canWallTime;
    bool jump, wallJumpRight, wallJumpLeft;
    bool normalAttack;
    int numberOfAttacks;
    float timeToSpawnMoveParticle;
    AudioManager am;
    float attackSpeed = 1;
    HealthManager health;
    ParticleSystem moveEffect;
    ParticleSystem jumpEffect;
    bool canCombo;
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            attackSpeed = value;
            if (attackSpeed < 1f)
                attackSpeed = 1;
            anim.SetFloat("AttackSpeed", attackSpeed);
        }
    }
    public bool WallJumped { get; set; }
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }
    public float DodgeSpeed
    {
        get { return dodgeSpeed; }
        set { dodgeSpeed = value; }
    }
    float gravityScale;
    GameCamera gc;
    public bool NearWallFront { get; set; }
    public bool GotHit { get; set; }
    public bool CanMove { get; set; } = true;
    public bool IsDodging { get; private set; }
    public bool AirAttack { get; private set; }
    public int Strength { get; set; }
    #endregion


    private void Awake()
    {
        anim.SetLayerWeight(1, 0);
        health = GetComponent<HealthManager>();
        am = AudioManager.Instance;
        dialogue = GetComponent<DialogueSystem>();
        playerUI.SetParent(null);
    }

    void Start()
    {
        gc = FindObjectOfType<GameCamera>();
        DisableRenderer();
        rb = GetComponent<Rigidbody2D>();
        canWallTime = canWallJumpTime;
        gravityScale = rb.gravityScale;
    }

    //Physics movement
    void FixedUpdate()
    {
        if (!health.Dead)
        {
            if (!GotHit)
            {
                if (CanMove)
                {
                    if (!WallJumped)
                    {
                        if (Input.GetAxis("Horizontal") != 0)
                            moveDirection = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
                        else
                        {
                            if (!IsGrounded())
                                moveDirection = rb.velocity;
                            else
                            {
                                moveDirection = new Vector2(0, rb.velocity.y);
                            }
                        }
                        rb.velocity = moveDirection * 50 * Time.fixedDeltaTime;
                    }
                    if (jump)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 50 * Time.fixedDeltaTime);
                        jump = false;
                    }
                    else if (wallJumpRight && canWallTime > 0)
                    {
                        rb.velocity = new Vector2(wallJumpForce, jumpForce) * 50 * Time.fixedDeltaTime;
                        WallJumped = true;
                        wallJumpRight = false;
                    }
                    else if (wallJumpLeft && canWallTime > 0)
                    {
                        rb.velocity = new Vector2(-wallJumpForce, jumpForce) * 50 * Time.fixedDeltaTime;
                        WallJumped = true;
                        NearWallFront = false;
                        wallJumpLeft = false;
                    }
                }
                else
                {
                    if (AirAttack)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        rb.gravityScale = gravityScale + 2;
                    }
                    else if (IsDodging)
                    {
                        if (movingRight)
                        {
                            rb.velocity = new Vector2(DodgeSpeed, rb.velocity.y) * 50 * Time.fixedDeltaTime;
                        }
                        else
                        {
                            rb.velocity = new Vector2(-DodgeSpeed, rb.velocity.y) * 50 * Time.fixedDeltaTime;
                        }
                    }
                }
            }
            else
            {
                rb.velocity = forceDirection * 50 * Time.fixedDeltaTime;
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
    
    //Input Handling
    void Update()
    {
        anim.SetBool("Grounded", IsGrounded());
        if (!health.Dead)
        {
            if (CanMove)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    anim.SetLayerWeight(1, 1);
                    Attack();
                }
                HandleMovement();
            }
            //Dialogue handle
            else if (dialogue.FinishedDialogue == true)
            {
                CanMove = true;
                dialogue.FinishedDialogue = false;
            }
            else if (dialogue.StartedDialogue)
            {
                dialogue.StartedDialogue = false;
                dialogue.StartDialogue();
            }
        }
    }

    #region Class Functions
    public bool IsGrounded()
    {
        bool grounded=Physics2D.OverlapBox(groundDetector.position, new Vector2(groundDetectorWidth, 0.1f), 0f, whatIsWalkable);
        if(grounded)
        {
            CheckGroundLayer();
            return true;
        }
        return false;
    }
    public void CheckGroundLayer()
    {
        bool grass = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x - 0.5f, 0.2f), 0f, whatIsGrass);
        bool stone = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x - 0.5f, 0.2f), 0f, whatIsStone);
        bool wood = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x - 0.5f, 0.2f), 0f, whatIsWood);
        bool ground = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x - 0.5f, 0.2f), 0f, whatIsGround);
        if(grass && am.currentStepSound != am.playerMoveGrass)
        {
            am.currentStepSound = am.playerMoveGrass;
            moveEffect = moveEffects[0];
            jumpEffect = jumpEffects[0];
            return;
        }
        if (ground && am.currentStepSound != am.playerMoveGround)
        {
            am.currentStepSound = am.playerMoveGround;
            moveEffect = moveEffects[1];
            jumpEffect = jumpEffects[1];
            return;
        }
        if (stone && am.currentStepSound != am.playerMoveStone)
        {
            am.currentStepSound = am.playerMoveStone;
            moveEffect = moveEffects[2];
            jumpEffect = jumpEffects[2];
            return;
        }
        if (wood && am.currentStepSound != am.playerMoveWood)
        {
            am.currentStepSound = am.playerMoveWood;
            moveEffect = moveEffects[3];
            jumpEffect = jumpEffects[3];
            return;
        }

    }
    public void SetGravity(float value)
    {
        if (moveDirection.y < 0f && canWallTime > 0f)
        {
            rb.gravityScale = value;
            anim.SetBool("SlideWall", true);
        }
        else ResetGravityScale();
    }
    public void ResetGravityScale()
    {
        rb.gravityScale = gravityScale;
        anim.SetBool("SlideWall", false);
        canWallTime = canWallJumpTime;
    }
    public void ResetYMovement()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
    }
    public void ApplyForce(Vector2 force)
    {
        forceDirection = force;
        gc.PlayGotHitAnimation();
    }
    void SpawnJumpEffect()
    {
        ParticleSystem je = Instantiate(jumpEffect, feet.transform.position, feet.rotation);
        je.Play();
        Destroy(je.gameObject, 0.5f);
    }
    void SpawnJumpEffectWall()
    {
        ParticleSystem effect = jumpEffects[1];
        if (LevelManager.Instance.levelType == LevelType.castle)
            effect = jumpEffects[2];
        ParticleSystem je = Instantiate(effect, feet.transform.position, feet.rotation);
        je.Play();
        Destroy(je.gameObject, 0.5f);
    }
    void SpawnMoveEffect()
    {
        if (timeToSpawnMoveParticle <= 0)
        {
            ParticleSystem moveParticle = Instantiate(moveEffect, feet.transform.position, feet.rotation);
            moveParticle.Play();
            Destroy(moveParticle.gameObject, 1f);
            timeToSpawnMoveParticle = 0.2f;
        }
        else timeToSpawnMoveParticle -= Time.deltaTime;
    }
    public void EnterDialogue()
    {
        EnterCutscene();
        CanMove = false;
    }
    public void Die()
    {
        PlayerCoins.Instance.RemoveCoins(PlayerCoins.Instance.Coins / 4);
        ExperienceSystem.Instance.RemoveExperience(ExperienceSystem.Instance.Experience / 4);
        GameUI.Instance.CanBePaused = false;
        DisableRenderer();
        GotHit = false;
        EnterDialogue();
        anim.ResetTrigger("SlideWall");
        anim.SetTrigger("Death");
    }
    public void SpinPlayer()
    {
        playerSprite.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        movingLeft = true;
        movingRight = false;
    }
    void Attack()
    {
        if (AirAttack || IsDodging)
            return;
        if (!IsGrounded() && AirAttack == false && Input.GetAxis("Vertical") < 0 && !IsDodging)
        {
            anim.SetBool("AirAttack", true);
            AirAttack = true;
            anim.SetFloat("Speed", 0);
            CanMove = false;
            am.PlaySound(am.playerSwordSwing);
        }
        else if (numberOfAttacks == 0 && normalAttack == false)
        {
            anim.SetTrigger("NormalAttack");
            canCombo = false;
            numberOfAttacks++;
            normalAttack = true;
            am.PlaySound(am.playerSwordSwing);
        }
        else if (numberOfAttacks == 1 && canCombo && anim.GetCurrentAnimatorStateInfo(1).IsName("NormalAttack"))
        {
            anim.SetTrigger("Attack2");
            DisableRenderer();
            canCombo = false;
            numberOfAttacks++;
            am.PlaySound(am.playerSwordSwing);
        }
        else if (numberOfAttacks == 2 && canCombo && anim.GetCurrentAnimatorStateInfo(1).IsName("Attack2"))
        {
            anim.SetTrigger("ChargeAttack");
            canCombo = false;
            DisableRenderer();
            numberOfAttacks++;
            am.PlaySound(am.playerSwordSwing);
        }
    }    
    void HandleMovement()
    {
        if (IsGrounded())
        {
            NearWallFront = false;
            if (anim.GetLayerWeight(1) != 1f)
                anim.SetLayerWeight(1, 1f);
            if (WallJumped)
            {
                WallJumped = false;
            }
            if (Input.GetAxis("Horizontal") != 0 && anim.GetFloat("Speed") == 0)
            {
                anim.SetFloat("Speed", 1);
            }
            else if (anim.GetFloat("Speed") == 1 && Input.GetAxis("Horizontal") == 0)
            {
                anim.SetFloat("Speed", 0);
                timeToSpawnMoveParticle = 0;
            }
            if (anim.GetFloat("Speed") == 1)
                SpawnMoveEffect();
            if (anim.GetBool("AirAttack") == true)
            {
                anim.SetBool("AirAttack", false);
                CanMove = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) && !NearWallFront)
            {
                jump = true;
                anim.SetTrigger("Jump");
                SpawnJumpEffect();
                timeToSpawnMoveParticle = 0;
                if (rb.gravityScale == 1f)
                    ResetGravityScale();
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift) && !IsDodging)
            {
                anim.SetTrigger("Dodge");
                DisableRenderer();
                FinishGroundAttack();
                timeToSpawnMoveParticle = 0;
                SpawnJumpEffect();
            }
        }
        else if (NearWallFront)
        {
            if (canWallTime > 0)
            {
                if (movingLeft)
                {
                    if (Input.GetKey(KeyCode.Space) && Input.GetAxis("Horizontal") > 0)
                    {
                        anim.SetTrigger("Jump");
                        SpawnJumpEffectWall();
                        timeToSpawnMoveParticle = 0;
                        wallJumpRight = true;
                        movingLeft = false;
                        movingRight = true;
                        canWallTime = canWallJumpTime;
                        playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    }
                }
                else if (movingRight)
                {
                    if (Input.GetAxis("Horizontal") < 0 && Input.GetKey(KeyCode.Space))
                    {
                        anim.SetTrigger("Jump");
                        SpawnJumpEffectWall();
                        timeToSpawnMoveParticle = 0;
                        wallJumpLeft = true;
                        movingRight = false;
                        movingLeft = true;
                        canWallTime = canWallJumpTime;
                        playerSprite.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    }
                }
                if (!IsGrounded())
                {
                    canWallTime -= Time.deltaTime;
                    if (rb.gravityScale == gravityScale)
                        SetGravity(1f);
                    else if (IsGrounded())
                        ResetGravityScale();
                }
                else
                {
                    NearWallFront = false;
                    if (rb.gravityScale == 1f)
                        ResetGravityScale();
                    if (canWallTime < canWallJumpTime)
                        canWallTime = canWallJumpTime;
                }
            }
            else
            {
                if (rb.gravityScale == 1f)
                    ResetGravityScale();
                canWallTime = 0f;
                NearWallFront = false;
            }
        }

        //Rotate player
        if (!NearWallFront)
        {
            if (Input.GetAxis("Horizontal") > 0 && movingLeft)
            {
                movingRight = true;
                movingLeft = false;
                playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (Input.GetAxis("Horizontal") < 0 && movingRight)
            {
                movingLeft = true;
                movingRight = false;
                playerSprite.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }
    #endregion


    #region Animation Events
    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }
    public void CanCombo()
    {
        canCombo = true;
    }
    public void CanNotCombo()
    {
        canCombo = false;
    }
    public void SlowTime()
    {
        gc.CloseUp();
        Time.timeScale = 0.5f;
    }
    public void ResetScene()
    {
        health.ResetScene();
    }
    public void FinishAirAttack()
    {      
        CanMove = true;
        AirAttack = false;
        anim.SetBool("AirAttack", false);
        numberOfAttacks = 0;
        ResetGravityScale();
    }
    public void FinishGroundAttack()
    {        
        normalAttack = false;
        canCombo = false;
        anim.ResetTrigger("NormalAttack");
        anim.ResetTrigger("Attack2");
        anim.ResetTrigger("ChargeAttack");
        numberOfAttacks = 0;
    }
    public void ShakeCamera()
    {
        gc.PlayGotHitAnimation();
    }
    public void EnableRenderer()
    {
        if (IsDodging) return;
        weapon.gameObject.SetActive(true);
        lineObject.SetActive(true);
    }
    public void DisableRenderer()
    {
        weapon.gameObject.SetActive(false);
        lineObject.SetActive(false);
    }
    public void StartDodging()
    {
        anim.ResetTrigger("NormalAttack");
        anim.ResetTrigger("Attack2");
        anim.ResetTrigger("ChargeAttack");
        anim.SetLayerWeight(1, 0);
        normalAttack = false;
        numberOfAttacks = 0;
        AirAttack = false;
        am.PlaySound(am.playerDodge);
        CanMove = false;
        IsDodging = true;
    }
    public void EndDodging()
    {
        CanMove = true;
        IsDodging = false;
    }
    public void EnterCutscene()
    {
        anim.SetFloat("Speed", 0f);
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
    public void PlayJump()
    {
        am.PlaySound(am.playerJump);
    }
    public void PlayStepSound()
    {
        am.PlaySound(am.currentStepSound);
    }
    public void AbleToMove()
    {
        CanMove = true;
    }
    public void StopMoving()
    {
        CanMove = false;
    }
    public void StartAirAttack()
    {
        anim.SetLayerWeight(1, 0f);
    }
    public void ResetSceneAfterDeath()
    {
        ResetScene();
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundDetector.position, new Vector2(groundDetectorWidth, 0.1f));
    }
}
