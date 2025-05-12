using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] CoinsUI coinUI;
    float speed;
    Transform target;
    bool isMoving;
    public int Value { get; set; }
    private void Start()
    {
        target = FindObjectOfType<PlayerCoins>().transform;
    }
    private void Update()
    {
        if (!isMoving)
        {
            float distance = Mathf.Abs(target.position.x - transform.position.x);
            speed = target.GetComponent<PlayerController>().MoveSpeed + 10f;
            if (distance <= target.GetComponent<PlayerCoins>().PickupRange)
                isMoving = true;
        }
        else
        {
            float distance = Mathf.Abs(target.position.x - transform.position.x);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, target.position.y + 1.5f), speed * Time.deltaTime);
            if (distance <= 0.2f)
            {
                PlayerCoins.Instance.AddCoins(Value);
                CoinsUI cUI = Instantiate(coinUI, transform.position, Quaternion.identity);
                cUI.ShowValue(Value);
                AudioManager.Instance.PlaySound(AudioManager.Instance.coinPickup);
                Destroy(gameObject);
            }
        }
    }
}
