using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryPotion : Singleton<InventoryPotion>
{
    [SerializeField] int maxCount;
    public int healthincrease;
    [SerializeField] TMP_Text countText;
    [SerializeField] TMP_Text useText;
    [SerializeField] HealthManager playerHealth;
    [SerializeField] Image potionImage;
    [SerializeField] Sprite potionSpriteFull;
    [SerializeField] Sprite potionSpriteEmpty;
    [SerializeField] Transform particleSpawnPoint;
    [SerializeField] GameObject useEffect;
    public bool CanUse { get; set; } = true;
    int count;
    void Start()
    {
        useText.text = PlayerKeys.Instance.usePotionKey.ToString().ToLower();
        count = maxCount;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.Potions))
            count = PlayerPrefs.GetInt(PlayerPrefsKeys.Potions);
        countText.text = count.ToString();
        if(count==0)
            potionImage.sprite = potionSpriteEmpty;
        else potionImage.sprite = potionSpriteFull;
    }
    void UseItem()
    {
        if (count > 0 && CanUse)
        {
            if (playerHealth.Health == playerHealth.MaxHealth) return;
            count--;
            count = Mathf.Clamp(count, 0, maxCount);
            PlayerPrefs.SetInt(PlayerPrefsKeys.Potions, count);
            countText.text = count.ToString();
            playerHealth.AddHealth(healthincrease);
            GameObject ef = Instantiate(useEffect, particleSpawnPoint);
            ef.transform.localPosition = Vector3.zero;
            Destroy(ef, 1.5f);
            if(count==0)
            {
                potionImage.sprite = potionSpriteEmpty;
            }
            AudioManager.Instance.PlaySound(AudioManager.Instance.potionUse);
        }
        
    }
    public void AddItem()
    {
        if(count==0)
        {
            potionImage.sprite = potionSpriteFull;
        }
        count++;
        count = Mathf.Clamp(count, 0, maxCount);
        countText.text = count.ToString();
        PlayerPrefs.SetInt(PlayerPrefsKeys.Potions, count);
    }
    private void Update()
    {
        if (Input.GetKeyDown(PlayerKeys.Instance.usePotionKey))
            UseItem();
    }
    public bool MaxCount()
    {
        return count == maxCount;
    }
}
