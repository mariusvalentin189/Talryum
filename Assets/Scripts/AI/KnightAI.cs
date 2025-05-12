using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAI : AI
{
    [SerializeField] AudioClip swingSound;
    [SerializeField] AudioClip fallGrassSound;
    [SerializeField] AudioClip fallGroundSound;
    [SerializeField] AudioClip fallStoneSound;
    AudioClip currentFallSound;
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
            currentFallSound = fallGrassSound;
            return;
        }
        if (hitGround && currentStepSound != groundStep)
        {
            currentStepSound = groundStep;
            currentFallSound = fallGroundSound;
            return;
        }
        if (hitStone && currentStepSound != stoneStep)
        {
            currentStepSound = stoneStep;
            currentFallSound = fallStoneSound;
            return;
        }
    }
    public void PlayFallSound()
    {
        sound.PlayOneShot(currentFallSound);
    }
    public void PlaySwingSound()
    {
        sound.PlayOneShot(swingSound);
    }
}
