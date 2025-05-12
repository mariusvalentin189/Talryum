using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static int hp;
    public static int str;
    public static int dex;
    public static int def;

    public static void Load()
    {
        hp = 0;
        str = 0;
        dex = 0;
        def = 0;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerHP))
            hp = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerHP);
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerSTR))
            str = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerSTR);
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerDEX))
            dex = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerDEX);
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerDEF))
            def = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerDEF);
    }
    public static void Save()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerHP, hp);
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerSTR, str);
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerDEX, dex);
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerDEF, def);
    }
}
