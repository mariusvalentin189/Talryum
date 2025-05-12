using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public int id;
    public Weapon weapon;
    public Armor armor;
    [SerializeField] ItemType itemType;
    [SerializeField] int price;
    [SerializeField] Sprite iconSprite;
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text buyText;
    [SerializeField] Button buyButton;
    [SerializeField] Image priceImage;
    Shop shop;
    ExperienceSystem exp;
    public int Bought { get; set; }
    public ItemType Type => itemType;
    public int Price => price;
    public void Initialize()
    {
        exp = ExperienceSystem.Instance;
        shop = Shop.Instance;
        LoadData();
        UpdateDescription();
    }
    public void BuyItem()
    {
        Settings.Instance.PlayButtonSound();
        shop.BuyItem(this);
    }

    public void LoadData()
    {
        Bought = 0;
        if (itemType == ItemType.weapon)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.BoughtWeapon + id))
                Bought = PlayerPrefs.GetInt(PlayerPrefsKeys.BoughtWeapon + id);
        }
        else if (itemType == ItemType.armor)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.BoughtArmor + id))
                Bought = PlayerPrefs.GetInt(PlayerPrefsKeys.BoughtArmor + id);
        }
        UpdateBuyButton();
        priceImage.sprite = iconSprite;
    }
    public void UpdateBuyButton()
    {
        if (Bought == 1)
        {
            if ((itemType == ItemType.weapon && id == exp.WeaponId) || (itemType == ItemType.armor && id == exp.ArmorId))
            {
                buyText.text = "equiped";
                buyButton.interactable = false;
            }
            else
            {
                buyText.text = "equip";
                buyButton.interactable = true;
            }
            priceText.text = "owned";
        }
        else
        {
            if (itemType == ItemType.potion)
            {
                if (InventoryPotion.Instance.MaxCount() || price > PlayerCoins.Instance.Coins)
                    buyButton.interactable = false;
                else buyButton.interactable = true;
            }
            else
            {
                if (price > PlayerCoins.Instance.Coins)
                    buyButton.interactable = false;
                else buyButton.interactable = true;
            }
            priceText.text = price.ToString();
            buyText.text = "buy";
        }
    }
    void UpdateDescription()
    {
        if(itemType == ItemType.armor)
        {
            shop.descriptionText.text = $"deffence +" + armor.damageReduction + "% \n"
                + "health +" + armor.health; 
        }
        else if (itemType == ItemType.weapon)
        {
            shop.descriptionText.text = "damage: " + weapon.minDamage + "-" + weapon.maxDamage;
        }
        else if (itemType == ItemType.potion)
        {
            shop.descriptionText.text = "restores " + InventoryPotion.Instance.healthincrease + " hp when used";
        }
    }
    public void ShowDescription(bool active)
    {
        if (active)
            UpdateDescription();
        shop.descriptionPanel.SetActive(active);
    }
}
public enum ItemType
{
    weapon = 1,
    armor = 2,
    potion = 3
}
[System.Serializable]
public class Armor
{
    public Sprite body;
    public Sprite head;
    public Sprite leftArm;
    public Sprite leftLeg;
    public Sprite rightArm;
    public Sprite rightLeg;
    public int damageReduction;
    public int health;
}
[System.Serializable]
public class Weapon
{
    public Sprite weaponSprite;
    public int minDamage;
    public int maxDamage;
}
