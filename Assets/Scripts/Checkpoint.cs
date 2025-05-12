using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Sprite reachedSprite;
    [SerializeField] Sprite unReachedSprite;
    [SerializeField] SpriteRenderer spriteRenderer;
    public bool FaceLeft;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerSpawnSystem.Instance.AssignCheckpoint(transform);
            spriteRenderer.sprite = reachedSprite;
        }
    }
    public void ResetCheckpoint()
    {
        spriteRenderer.sprite = unReachedSprite;
    }
    public void EnableFlag()
    {
        spriteRenderer.sprite = reachedSprite;
    }
}
