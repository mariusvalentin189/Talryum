using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUpgrade : MonoBehaviour
{
    public bool IsMaxed { get; private set; }
    [SerializeField] UpgradeType upgradeType;
    [SerializeField] TMP_Text valueText;
    [SerializeField] Button confirmButton;
    [SerializeField] ExperienceSystem experienceSystem;
    [SerializeField] Button upgradeButton;
    [SerializeField] Button downgradeButton;
    [SerializeField] TMP_Text leveltext;
    int currentUpgrade;
    AudioManager am;
    int upgradeCount;
    int maxUpgrades;
    private void Awake()
    {
        PlayerStats.Load();
        maxUpgrades = experienceSystem.MaxUpgrades;
        am = AudioManager.Instance;
        switch (upgradeType)
        {
            case UpgradeType.hp:
                {
                    currentUpgrade = PlayerStats.hp;
                    break;
                }
            case UpgradeType.str:
                {
                    currentUpgrade = PlayerStats.str;
                    break;
                }
            case UpgradeType.dex:
                {
                    currentUpgrade = PlayerStats.dex;
                    break;
                }
            case UpgradeType.def:
                {
                    currentUpgrade = PlayerStats.def;
                    break;
                }
            default:
                {
                    break;
                }
        }
        valueText.text = experienceSystem.UpdateStatValue(upgradeType);
        CheckRemainingUpgrades();
    }

    public void AddUpgrade()
    {
        upgradeCount++;
        currentUpgrade++;
        leveltext.text = currentUpgrade + "/" + maxUpgrades;
        experienceSystem.LevelPoints--;
        experienceSystem.ChangeStat(upgradeType, 1);
        downgradeButton.interactable = true;
        experienceSystem.CheckRemainingUpgrades();
        if (experienceSystem.LevelPoints <= 0)
            confirmButton.interactable = true;
        experienceSystem.CheckRemainingPoints();
        am.PlayButtonClickSound();
        valueText.text = experienceSystem.UpdateStatValue(upgradeType);
    }
    public void RemoveUpgrade()
    {
        upgradeCount--;
        currentUpgrade--;
        leveltext.text = currentUpgrade + "/" + maxUpgrades;
        experienceSystem.LevelPoints++;
        experienceSystem.ChangeStat(upgradeType, -1);
        upgradeButton.interactable = true;
        experienceSystem.CheckRemainingPoints();
        am.PlayButtonClickSound();
        valueText.text = experienceSystem.UpdateStatValue(upgradeType);
        experienceSystem.CheckRemainingUpgrades();
    }

    public void SaveUpgrade()
    {
        switch (upgradeType)
        {
            case UpgradeType.hp:
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerHP, currentUpgrade);
                    break;
                }
            case UpgradeType.str:
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerSTR, currentUpgrade);
                    break;
                }
            case UpgradeType.dex:
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerDEX, currentUpgrade);
                    break;
                }
            case UpgradeType.def:
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerDEF, currentUpgrade);
                    break;
                }
            default:
                {
                    break;
                }
        }
        upgradeCount = 0;
    }
    public void CheckRemainingUpgrades()
    {
        if (currentUpgrade == maxUpgrades)
        {
            upgradeButton.interactable = false;
            if (upgradeCount > 0)
                downgradeButton.interactable = true;
            else
                downgradeButton.interactable = false;
            IsMaxed = true;
        }
        else
        {
            IsMaxed = false;
            if (experienceSystem.MustUpgrade())
            {
                upgradeButton.interactable = true;
                downgradeButton.interactable = false;
            }
            else if (experienceSystem.IsUpgrading())
            {
                upgradeButton.interactable = true;
                if (upgradeCount > 0)
                    downgradeButton.interactable = true;
                else downgradeButton.interactable = false;
            }
            else if (experienceSystem.FinishedUpgrading())
            {
                upgradeButton.interactable = false;
                if (upgradeCount > 0)
                    downgradeButton.interactable = true;
                else downgradeButton.interactable = false;
            }
        }
        leveltext.text = currentUpgrade + "/" + maxUpgrades;
    }
}
public enum UpgradeType
{
    hp,
    str,
    dex,
    def
}
