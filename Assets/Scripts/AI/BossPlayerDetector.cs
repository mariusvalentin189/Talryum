using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayerDetector : MonoBehaviour
{
    public bool nearPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            nearPlayer = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            nearPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            nearPlayer = false;
    }

}
