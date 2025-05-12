using System.Collections;
using UnityEngine;
public class GolemAI : AI
{
    [SerializeField] AudioClip hitGrassSound;
    [SerializeField] AudioClip hitGroundSound;
    [SerializeField] AudioClip hitStoneSound;
    [SerializeField] AudioClip swingSound;
    AudioClip currentHitSound;
    protected override void FixedUpdate()
    {
        if (!healthManager.IsDead)
        {
            if (IsGrounded())
            {
                if (spd == 0)
                    spd = speed;
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
            else
            {
                if (anim.GetFloat("Speed") == 1)
                    anim.SetFloat("Speed", 0);
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }
    protected override void DetectWalkableLayerType()
    {
        Collider2D hitGrass;
        Collider2D hitGround;
        Collider2D hitStone;
        hitGrass = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x, 0.2f), 0f, grassLayer);
        hitGround = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x, 0.2f), 0f, groundLayer);
        hitStone = Physics2D.OverlapBox(groundDetector.position, new Vector2(GetComponent<BoxCollider2D>().size.x, 0.2f), 0f, stoneLayer);
        if (hitGrass && currentStepSound != grassStep)
        {
            currentStepSound = grassStep;
            currentHitSound = hitGrassSound;
            return;
        }
        if (hitGround && currentStepSound != groundStep)
        {
            currentStepSound = groundStep;
            currentHitSound = hitGroundSound;
            return;
        }
        if (hitStone && currentStepSound != stoneStep)
        {
            currentStepSound = stoneStep;
            currentHitSound = hitStoneSound;
            return;
        }
    }
    public override void EndAttack()
    {
        base.EndAttack();
        gc.PlayGotHitAnimation2();
    }
    public void HitGround()
    {
        sound.PlayOneShot(currentHitSound);
    }
    public void PlaySwingSound()
    {
        sound.PlayOneShot(swingSound);
    }
}
