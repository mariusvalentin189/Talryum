using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] AudioClip jumpSound;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.up * force, ForceMode2D.Impulse);
            anim.SetTrigger("Launch");
            AudioManager.Instance.PlaySound(jumpSound);
        }
    }
}
