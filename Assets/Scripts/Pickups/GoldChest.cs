using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldChest : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] int coinsCount;
    [SerializeField] CoinsUI coinUI;
    [SerializeField] TMP_Text interactText;
    [SerializeField] GameObject interactUI;
    void Start()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.ChestCollected + id))
        {
            int collected = PlayerPrefs.GetInt(PlayerPrefsKeys.ChestCollected + id);
            if (collected == 1)
                Destroy(gameObject);
        }
        string keyText = PlayerKeys.Instance.interactKey.ToString().ToLower();
        interactText.text = $"press {keyText} to open";
    }
    private void Update()
    {
        if(interactUI.activeSelf)
        {
            if (Input.GetKeyDown(PlayerKeys.Instance.interactKey))
            {
                PlayerCoins.Instance.AddCoins(coinsCount);
                PlayerPrefs.SetInt(PlayerPrefsKeys.ChestCollected + id, 1);
                CoinsUI cUI = Instantiate(coinUI, transform.position, Quaternion.identity);
                cUI.ShowValue(coinsCount);
                AudioManager.Instance.PlaySound(AudioManager.Instance.coinPickup);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactUI.SetActive(false);
        }
    }
}
