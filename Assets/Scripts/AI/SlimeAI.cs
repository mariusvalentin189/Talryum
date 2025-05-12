using UnityEngine;
public class SlimeAI : AI
{
    bool disabledConstraints;
    bool moved;
    protected override void FixedUpdate()
    {
        if (IsGrounded())
        {
            if (!GotHit)
            {
                if (canMove)
                {
                    rb.velocity = transform.right * spd * Time.fixedDeltaTime;
                    if (!moved)
                    {
                        moved = true;
                    }
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
                if (disabledConstraints)
                    EnableConstraints();
                DetectWalkableLayerType();
                rb.velocity = forceDirection * 50 * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (disabledConstraints && (moved || GotHit))
            {
                EnableConstraints();
            }
            if (anim.GetFloat("Speed") == 1)
                anim.SetFloat("Speed", 0);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    #region Animation Events
    public void DisableConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        disabledConstraints = true;
    }
    public void EnableConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        disabledConstraints = false;
    }
    public override void StopMoving()
    {
        spd = 0;
    }
    public void StartMoving()
    {
        spd = speed;
    }
    #endregion
}
