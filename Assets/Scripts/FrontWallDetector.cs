using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWallDetector : MonoBehaviour
{
    [SerializeField] int[] layersIndex;
    public PlayerController player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.AirAttack || player.IsGrounded())
        {
            player.NearWallFront = false;
            return;
        }
        if (HasElement(collision.gameObject.layer))
        {
            player.NearWallFront = true;
            player.WallJumped = false;
            player.ResetYMovement();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (HasElement(collision.gameObject.layer))
        {
            player.NearWallFront = false;
            player.ResetGravityScale();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (HasElement(collision.gameObject.layer))
        {
            if(player.IsGrounded())
            {
                player.NearWallFront = false;
                player.ResetGravityScale();
            }
        }
    }
    bool HasElement(int value)
    {
        foreach (int el in layersIndex)
        {
            if (el == value)
                return true;
        }
        return false;
    }
}
