using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePropOnDistance : MonoBehaviour
{
    // Start is called before the first frame update
    Transform player;
    SpriteRenderer sprite;
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        sprite = GetComponent<SpriteRenderer>();
        if (sprite == null)
            sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DevTools.Instance.isDebugging)
            return;
        float distance = Mathf.Abs(transform.position.x - player.position.x);
        if (distance >= 30 && sprite.enabled==true)
        {
            sprite.enabled = false;
            for (int i = 0; i < transform.childCount;i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
        else if (distance < 30 && sprite.enabled == false)
        {
            sprite.enabled = true;
            for (int i = 0; i < transform.childCount;i++)
                transform.GetChild(i).gameObject.SetActive(true);
        }


    }
}
