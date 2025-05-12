using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class DisableOnDistance : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public bool enabledAI=true;
    Transform player;
    SpriteRenderer[] sprites;
    SpriteSkin[] spriteSkins;
    Rigidbody2D[] rb;
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        sprites = transform.GetComponentsInChildren<SpriteRenderer>();
        spriteSkins = transform.GetComponentsInChildren<SpriteSkin>();
        rb = transform.GetComponentsInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DevTools.Instance.isDebugging == true)
            return;
        float distanceX = Mathf.Abs(transform.position.x - player.position.x);
        float distanceY = Mathf.Abs(transform.position.y - player.position.y);
        if (distanceX >= 25f || distanceY >= 15f)
        {
            if (enabledAI)
            {
                enabledAI = false;
                audioSource.enabled = false;
                GetComponent<EnemyHealthManager>().enabled = false;
                foreach (SpriteRenderer sp in sprites)
                    sp.enabled = false;
                foreach (SpriteSkin sp in spriteSkins)
                    sp.enabled = false;
                foreach (Rigidbody2D r in rb)
                    r.bodyType = RigidbodyType2D.Kinematic;
            }
        }
        else if (!enabledAI)
        {
            enabledAI = true;
            audioSource.enabled = true;
            GetComponent<EnemyHealthManager>().enabled = true;
            foreach (SpriteRenderer sp in sprites)
                sp.enabled = true;
            foreach (SpriteSkin sp in spriteSkins)
                sp.enabled = true;
            foreach (Rigidbody2D r in rb)
                r.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
