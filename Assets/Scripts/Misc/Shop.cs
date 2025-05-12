using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : Singleton<Shop>
{
    [SerializeField] GameObject interactUI;
    public GameObject shopUI;
    [SerializeField] GameObject weaponsContent;
    [SerializeField] GameObject armorsContent;
    [SerializeField] GameObject potionsContent;
    ShopItem[] weapons;
    ShopItem[] armors;
    [SerializeField] ShopItem[] potions;
    PlayerController player;
    Sword playerHeldWeapon;
    PlayerCoins playerCoins;
    ExperienceSystem exp;
    List<ShopItem> shopItems = new List<ShopItem>();
    public GameObject descriptionPanel;
    public TMP_Text descriptionText;
    private void Awake()
    {
        exp = ExperienceSystem.Instance;
        weapons = exp.weapons;
        armors = exp.armors;
        player = FindObjectOfType<PlayerController>();
        playerHeldWeapon = FindObjectOfType<Sword>();
        playerCoins = FindObjectOfType<PlayerCoins>();
        foreach (ShopItem w in weapons)
        {
            ShopItem it = Instantiate(w, weaponsContent.transform);
            shopItems.Add(it);
        }
        foreach (ShopItem a in armors)
        {
            ShopItem it = Instantiate(a, armorsContent.transform);
            shopItems.Add(it);
        }
        foreach (ShopItem a in potions)
        {
            ShopItem it = Instantiate(a, potionsContent.transform);
            shopItems.Add(it);
        }
        foreach (ShopItem sh in shopItems)
            sh.Initialize();
    }
    private void Update()
    {
        if (interactUI.activeSelf)
        {
            if (Input.GetKeyDown(PlayerKeys.Instance.interactKey))
                EnterShop();
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
    public void EnterShop()
    {
        shopUI.SetActive(true);
        UpdateItems();
        FindObjectOfType<PlayerController>().EnterDialogue();
        GameUI.Instance.CanBePaused = false;
        InventoryPotion.Instance.CanUse = false;
    }
    public void ExitShop()
    {
        shopUI.SetActive(false);
        FindObjectOfType<PlayerController>().CanMove = true;
        GameUI.Instance.CanBePaused = true;
        InventoryPotion.Instance.CanUse = true;
    }
    public void BuyItem(ShopItem item)
    {
        if (item.Bought == 0)
        {
            playerCoins.RemoveCoins(item.Price);
            if (item.Type == ItemType.weapon)
            {
                exp.ChangeWeapon(item);
                PlayerPrefs.SetInt(PlayerPrefsKeys.WeaponID, item.id);
                PlayerPrefs.SetInt(PlayerPrefsKeys.BoughtWeapon + item.id, 1);
                item.Bought = 1;
            }
            else if (item.Type == ItemType.armor)
            {
                exp.ChangeArmor(item);
                PlayerPrefs.SetInt(PlayerPrefsKeys.ArmorID, item.id);
                PlayerPrefs.SetInt(PlayerPrefsKeys.BoughtArmor + item.id, 1);
                item.Bought = 1;
            }
            else
            {
                InventoryPotion.Instance.AddItem();
            }
        }
        else
        {
            if (item.Type == ItemType.weapon)
            {
                exp.ChangeWeapon(item);
                PlayerPrefs.SetInt(PlayerPrefsKeys.WeaponID, item.id);
            }
            else
            {
                exp.ChangeArmor(item);
                PlayerPrefs.SetInt(PlayerPrefsKeys.ArmorID, item.id);
            }
        }
        UpdateItems();


    }
    void UpdateItems()
    {
        foreach (ShopItem w in shopItems)
            if(w!=null)
                w.UpdateBuyButton();
    }
}
