using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("Sounds")]
    public AudioSource sound;
    public AudioClip grassStep;
    public AudioClip groundStep;
    public AudioClip stoneStep;
    public bool FoundPlayer { get; set; }

    [Header("Stats")]
    public int damage;
    [SerializeField] protected float speed;
    [SerializeField] protected bool canMove;
    public float attackCooldown;

    [Header("Objects")]
    [SerializeField] protected GameObject attackPoint;
    [SerializeField] protected Transform middlePoint;
    public BoxCollider2D playerDetectCollider;

    [Header("Layers")]
    public LayerMask obstacleLayer;
    public LayerMask walkableLayers;
    public LayerMask grassLayer;
    public LayerMask groundLayer;
    public LayerMask stoneLayer;
    [SerializeField] protected Transform groundDetector;
    public Transform obstacleDetectPosition;
    public Transform groundDetectPosition;
    public Transform backObstacleDetectPosition;
    public float groundDetectDistance;
    public float obstacleDetectDistance;


    protected bool attacked;
    protected Transform player;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected GameCamera gc;
    protected float spd;
    protected float attackCld;
    protected Vector2 forceDirection;
    protected EnemyHealthManager healthManager;
    protected AudioClip currentStepSound;
    public bool GotHit { get; set; }
    public Animator Anim
    {
        get { return anim; }
        set { anim = value; }
    }
    public float Speed
    {
        get { return spd; }
        set { spd = value; }
    }
    protected DisableOnDistance disabler;
    protected virtual void Start()
    {
        disabler = GetComponent<DisableOnDistance>();
        spd = speed;
        attackCld = attackCooldown;
        attackPoint.SetActive(false);
        playerDetectCollider = middlePoint.GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerController>().transform;
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gc = FindObjectOfType<GameCamera>();
        healthManager = GetComponent<EnemyHealthManager>();
    }
    protected virtual void FixedUpdate()
    {
        if (!healthManager.IsDead)
        {
            if (IsGrounded())
            {
                if (!GotHit)
                {
                    if (canMove)
                    {
                        rb.velocity = transform.right * spd * Time.fixedDeltaTime;
                        if (anim.GetFloat("Speed") == 0)
                            anim.SetFloat("Speed", 1);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        if (anim.GetFloat("Speed") == 1)
                            anim.SetFloat("Speed", 0);
                    }
                }
                else
                {
                    rb.velocity = forceDirection * 10 * Time.fixedDeltaTime;
                }
            }
        }
        else rb.velocity = new Vector2(0f, rb.velocity.y);
    }
    public virtual void ApplyForce(Vector2 force)
    {
        forceDirection = force;
    }
    public virtual void ApplyDelay()
    {
        attackCld += 0.2f;
        attackCld = Mathf.Clamp(attackCld, 0f, attackCooldown);
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x, 0.2f), 0f, walkableLayers);
    }
    public virtual void EndAttack()
    {
        attacked = false;
        attackPoint.SetActive(false);
    }
    public void CanMove()
    {
        canMove = true;
    }
    public void SetSpeed()
    {
        speed = -speed;
        if (spd != 0)
            spd = speed;
    }
    public void StartAttack()
    {
        attackPoint.SetActive(true);
    }
    public void CanNotMove()
    {
        canMove = false;
    }
    public virtual void StopMoving()
    {
        canMove = false;
        anim.SetFloat("Speed", 0);
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
    public void EnableAI()
    {
        anim.enabled = true;
    }
    public void DisableAI()
    {
        anim.enabled = false;
    }
    public RaycastHit2D[] DetectObjects()
    {
        DetectWalkableLayerType();
        RaycastHit2D hitObstacle;
        RaycastHit2D hitBackObstacle;
        RaycastHit2D hitGround;
        hitGround = Physics2D.Raycast(groundDetectPosition.position, -transform.up, groundDetectDistance, walkableLayers);
        if (speed >= 0)
        {
            hitObstacle = Physics2D.Raycast(obstacleDetectPosition.position, transform.right, obstacleDetectDistance, obstacleLayer);
            hitBackObstacle = Physics2D.Raycast(backObstacleDetectPosition.position, -transform.right, obstacleDetectDistance, obstacleLayer);
        }
        else
        {
            hitObstacle = Physics2D.Raycast(obstacleDetectPosition.position, -transform.right, obstacleDetectDistance, obstacleLayer);
            hitBackObstacle = Physics2D.Raycast(obstacleDetectPosition.position, transform.right, obstacleDetectDistance, obstacleLayer);
        }
        RaycastHit2D[] ret = new RaycastHit2D[3];
        ret[0] = hitObstacle;
        ret[1] = hitBackObstacle;
        ret[2] = hitGround;
        return ret;

    }
    protected virtual void DetectWalkableLayerType()
    {
        Collider2D hitGrass;
        Collider2D hitGround;
        Collider2D hitStone;
        hitGrass = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x, 0.2f), 0f, grassLayer);
        hitGround = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x, 0.2f), 0f, groundLayer);
        hitStone = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x, 0.2f), 0f, stoneLayer);
        if(hitGrass && currentStepSound != grassStep)
        {
            currentStepSound = grassStep;
            return;
        }
        if (hitGround && currentStepSound != groundStep)
        {
            currentStepSound = groundStep;
            return;
        }
        if (hitStone && currentStepSound != stoneStep)
        {
            currentStepSound = stoneStep;
            return;
        }
    }
    public void ShakeCamera()
    {
        gc.PlayGotHitAnimation2();
    }
    public void Step()
    {
        if (!disabler.enabledAI)
            return;
        sound.PlayOneShot(currentStepSound);
    }
}
