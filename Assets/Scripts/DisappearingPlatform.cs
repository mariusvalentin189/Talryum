using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] float disappearTime;
    [SerializeField] float appearTime;
    [SerializeField] GameObject platform;
    BoxCollider2D boxCollider;
    Animator anim;
    bool triggered;
    private void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (!triggered)
            {
                triggered = true;
                anim.SetTrigger("Shake");
                StartCoroutine(Disappear());
            }
        }
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearTime);
        anim.SetTrigger("Appear");
        boxCollider.enabled = false;
        platform.SetActive(false);
        StartCoroutine(Appear());
    }
    IEnumerator Appear()
    {
        yield return new WaitForSeconds(appearTime);
        triggered = false;
        platform.SetActive(true);
        boxCollider.enabled = true;

    }
}
