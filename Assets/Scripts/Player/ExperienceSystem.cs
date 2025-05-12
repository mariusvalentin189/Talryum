using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceSystem : Singleton<ExperienceSystem>
{
    [SerializeField] int maxLevel;
    [SerializeField] int maxUpgrades;
    [SerializeField] int maxExperience;
    [SerializeField] float nextLevelXpMultiplier;
    [SerializeField] int healthIncrease;
    [SerializeField] int attackIncrease;
    [SerializeField] float attackSpeedIncrease;
    [SerializeField] float moveSpeedIncrease;
    [SerializeField] int deffenceIncrease;
    [SerializeField] Image expBar;
    [SerializeField] TMP_Text expText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] GameObject nextLevelPanel;
    [SerializeField] HealthManager healthManager;
    [SerializeField] PlayerController playerController;
    [SerializeField] Sword weapon;
    [SerializeField] PlayerUpgrade[] upgrades;
    [SerializeField] TMP_Text statsText;
    [SerializeField] TMP_Text remainingPointsText;
    [SerializeField] int pointsPerLevel;
    [SerializeField] Button confirmButton;

    [Header("Sprites")]
    public SpriteRenderer playerWeapon;
    public SpriteRenderer playerBody;
    public SpriteRenderer playerHead;
    public SpriteRenderer playerLeftArm;
    public SpriteRenderer playerLeftLeg;
    public SpriteRenderer playerRightArm;
    public SpriteRenderer playerRightLeg;
    public ShopItem[] weapons;
    public ShopItem[] armors;
    public ShopItem defaultSword; 
    public ShopItem defaultArmor; 
    Sword playerSword;
    public int WeaponId { get; set; }
    public int ArmorId { get; set; }

    int experience;
    int level = 1;
    int currentAttackIncrease;
    public int MaxUpgrades => maxUpgrades;
    public bool MaxedUpgrades { get; set; }
    public int LevelPoints { get; set; }

    private void Awake()
    {
        WeaponId = 0;
        ArmorId = 0;
        playerSword = FindObjectOfType<Sword>();

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.WeaponID))
        {
            WeaponId = PlayerPrefs.GetInt(PlayerPrefsKeys.WeaponID);
            ChangeWeapon(weapons[WeaponId]);
        }
        else
        {
            playerSword.MinDamage += defaultSword.weapon.minDamage;
            playerSword.MaxDamage += defaultSword.weapon.maxDamage;
        }

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.ArmorID))
        {
            ArmorId = PlayerPrefs.GetInt(PlayerPrefsKeys.ArmorID);
            ChangeArmor(armors[ArmorId]);
        }

        LevelPoints = pointsPerLevel;
        confirmButton.interactable = false;

        LoadPlayerStats();
        CheckRemainingPoints();
    }
    public int Experience
    {
        get { return experience; }
        set 
        {
            if (experience < 0)
            {
                experience = 0;
                return;
            }
            experience = value;
            if (level == maxLevel)
            {
                experience = 0;
            }
            if (level <= maxLevel)
            {
                if (experience >= maxExperience && level < maxLevel)
                {
                    AudioManager.Instance.PlaySound(AudioManager.Instance.playerLevelUp);
                    healthManager.Health += healthManager.MaxHealth;
                    LevelPoints = pointsPerLevel;
                    nextLevelPanel.SetActive(true);
                    CheckRemainingPoints();
                    CheckRemainingUpgrades();
                    Time.timeScale = 0;
                    GameUI.Instance.CanBePaused = false;
                }
                if (level == maxLevel)
                {
                    expBar.fillAmount = 1;
                    expText.text = "exp: max";
                    levelText.text = maxLevel.ToString();
                    expBar.fillAmount = 1;
                }
                else
                {
                    expBar.fillAmount = (float)experience / maxExperience;
                    PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerExperience, experience);
                    expText.text = "exp: " + experience.ToString() + '/' + maxExperience.ToString();
                    levelText.text = level.ToString();
                }
            }
        }
    }
    public void AddExperience(int value)
    {
        Experience += value;
    }
    public void RemoveExperience(int value)
    {
        Experience -= value;
    }
    void LoadPlayerStats()
    { 
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerLevel))
        {
            level = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);
            maxExperience *= (int)Mathf.Pow(2, (level - 1));
        }

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerExperience))
            Experience = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerExperience);
        else Experience = 0;

        PlayerStats.Load();

        healthManager.MaxHealth += PlayerStats.hp * healthIncrease + level * 10;
        healthManager.LoadPlayerHealth();
        currentAttackIncrease = attackIncrease * PlayerStats.str;
        playerController.Strength = weapon.MinDamage * currentAttackIncrease / 100 + level;
        healthManager.Deffence += PlayerStats.def * deffenceIncrease + level * 2;
        playerController.MoveSpeed += PlayerStats.dex * moveSpeedIncrease;
        playerController.AttackSpeed += PlayerStats.dex * attackSpeedIncrease;
        playerController.DodgeSpeed += PlayerStats.dex * attackSpeedIncrease;
        UpdateStats();
    }


    public void ConfirmUpgrade()
    {
        AudioManager.Instance.PlayButtonClickSound();
        foreach (PlayerUpgrade up in upgrades)
            up.SaveUpgrade();
        GameUI.Instance.CanBePaused = true;
        Time.timeScale = 1;
        nextLevelPanel.SetActive(false);
        UpdateStats();
        if (level < maxLevel)
        {
            level += 1;
            healthManager.MaxHealth += 10;
            healthManager.Health += 10;
            healthManager.Deffence += 2;
        }
        if (level == maxLevel)
        {
            Experience = 0;
            expText.text = "exp: max";
            expBar.fillAmount = 1;
        }
        else
        {
            Experience -= maxExperience;
            float exp = (float)maxExperience * nextLevelXpMultiplier;
            maxExperience = (int)exp;
            expBar.fillAmount = (float)Experience / maxExperience;
            expText.text = "exp: " + experience.ToString() + '/' + maxExperience.ToString();
        }
        levelText.text = level.ToString();
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, level);
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerExperience, experience);
    }
    public void ChangeStat(UpgradeType ut, int sign)
    {
        switch (ut)
        {
            case UpgradeType.hp:
                {
                    healthManager.MaxHealth += healthIncrease * sign;
                    healthManager.Health += healthIncrease * sign;
                    PlayerStats.hp += 1 * sign;
                    break;
                }
            case UpgradeType.str:
                {
                    currentAttackIncrease += attackIncrease * sign;
                    playerController.Strength = weapon.MinDamage * currentAttackIncrease / 100 + level;
                    PlayerStats.str += 1 * sign;
                    break;
                }
            case UpgradeType.dex:
                {
                    playerController.AttackSpeed += attackSpeedIncrease * sign;
                    playerController.MoveSpeed += moveSpeedIncrease * sign;
                    playerController.DodgeSpeed += moveSpeedIncrease * sign;
                    PlayerStats.dex += 1 * sign;
                    break;
                }
            case UpgradeType.def:
                {
                    healthManager.Deffence += deffenceIncrease * sign;
                    PlayerStats.def += 1 * sign;
                    break;
                }
        }
        UpdateStats();
    }
    public string UpdateStatValue(UpgradeType ut)
    {
        switch (ut)
        {
            case UpgradeType.hp:
                {
                    return PlayerStats.hp.ToString();
                }
            case UpgradeType.str:
                {
                    return PlayerStats.str.ToString();
                }
            case UpgradeType.dex:
                {
                    return PlayerStats.dex.ToString();
                }
            case UpgradeType.def:
                {
                    return PlayerStats.def.ToString();
                }
        }
        return null;
    }

    void UpdateStats()
    {
        statsText.text = "max health: " + healthManager.MaxHealth + "\n" +
            "damage: " + (weapon.MinDamage + playerController.Strength) + "-" + (weapon.MaxDamage + playerController.Strength) + "\n" +
            "attack speed: " + playerController.AttackSpeed + "\n" +
            "move speed: " + playerController.MoveSpeed + "\n" +
            "deffence: " + healthManager.Deffence + "%" + "\n";
    }

    public void CheckRemainingPoints()
    {
        remainingPointsText.text = "Remaining points: " + LevelPoints;
    }
    public bool MustUpgrade()
    {
        return LevelPoints == pointsPerLevel;
    }
    public bool IsUpgrading()
    {
        return LevelPoints < pointsPerLevel && LevelPoints > 0;
    }
    public bool FinishedUpgrading()
    {
        return LevelPoints == 0;
    }
    public void CheckRemainingUpgrades()
    {
        foreach (PlayerUpgrade up in upgrades)
            up.CheckRemainingUpgrades();

        foreach (PlayerUpgrade up in upgrades)
        {
            MaxedUpgrades = up.IsMaxed;
            if (!MaxedUpgrades)
            {
                confirmButton.interactable = false;
                return;
            }
        }
        confirmButton.interactable = true;
    }
    public void ChangeArmor(ShopItem item)
    {
        if (ArmorId != item.id)
        {
            healthManager.Deffence -= armors[ArmorId].armor.damageReduction;
            healthManager.MaxHealth -= armors[ArmorId].armor.health;
        }
        ArmorId = item.id;
        healthManager.Deffence += armors[ArmorId].armor.damageReduction;
        healthManager.MaxHealth += armors[ArmorId].armor.health;
        if (healthManager.Health > healthManager.MaxHealth)
        {
            healthManager.Health = healthManager.MaxHealth;
        }
        playerBody.sprite= armors[ArmorId].armor.body;
        playerHead.sprite = armors[ArmorId].armor.head;
        playerLeftArm.sprite = armors[ArmorId].armor.leftArm;
        playerLeftLeg.sprite = armors[ArmorId].armor.leftLeg;
        playerRightArm.sprite = armors[ArmorId].armor.rightArm;
        playerRightLeg.sprite = armors[ArmorId].armor.rightLeg;
        UpdateStats();
    }
    public void ChangeWeapon(ShopItem item)
    {
        if (WeaponId != item.id)
        {
            playerSword.MinDamage -= weapons[WeaponId].weapon.minDamage;
            playerSword.MaxDamage -= weapons[WeaponId].weapon.maxDamage;
        }
        WeaponId = item.id;
        playerSword.MinDamage += item.weapon.minDamage;
        playerSword.MaxDamage += item.weapon.maxDamage;
        playerController.Strength = weapon.MinDamage * currentAttackIncrease / 100 + level;
        playerWeapon.sprite = weapons[WeaponId].weapon.weaponSprite;
        UpdateStats();
    }
}
