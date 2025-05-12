using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCoins : Singleton<PlayerCoins>
{
    [SerializeField] int coins;
    [SerializeField] Coin coinPrefab;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] float pickupRange;

    public float PickupRange => pickupRange;
    public int Coins => coins;
    private void Start()
    {
        LoadCoins();
    }
    public void LoadCoins()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.Coins))
            coins = PlayerPrefs.GetInt(PlayerPrefsKeys.Coins);
        else coins = 0;
        UpdateText();
    }
    public void AddCoins(int value)
    {
        coins += value;
        UpdateText();
        PlayerPrefs.SetInt(PlayerPrefsKeys.Coins, coins);
    }
    public void RemoveCoins(int value)
    {
        coins -= value;
        UpdateText();
        PlayerPrefs.SetInt(PlayerPrefsKeys.Coins, coins);
    }
    public void SpawnCoins(int value, Vector2 position)
    {
        Vector2 newPos = new Vector2(position.x, position.y + 2f);
        Coin coin = Instantiate(coinPrefab, newPos, Quaternion.identity);
        coin.Value = value;
    }
    void UpdateText()
    {
        coinsText.text = coins.ToString();
    }
    public void ExitShop()
    {
        FindObjectOfType<Shop>().ExitShop();
    }
}
