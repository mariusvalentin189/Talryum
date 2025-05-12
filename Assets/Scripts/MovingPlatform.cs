using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform[] platformPoints;
    Transform currentPoint;
    private void Start()
    {
        currentPoint = platformPoints[0];
        foreach (Transform p in platformPoints)
            p.SetParent(null);
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, moveSpeed * Time.deltaTime);
        if (transform.position == currentPoint.position)
            SwitchPoint();
    }
    void SwitchPoint()
    {
        if (currentPoint == platformPoints[0])
            currentPoint = platformPoints[1];
        else currentPoint = platformPoints[0];
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
