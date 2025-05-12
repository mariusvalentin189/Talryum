using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsManager
{
    public static float soundVolume, musicVolume;
    public static int screenWidth, screenHeight;
    public static bool fullScreen;
    public static void LoadSettings()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SoundVolume))
            soundVolume = PlayerPrefs.GetFloat(PlayerPrefsKeys.SoundVolume);
        else soundVolume = 1;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.MusicVolume))
            musicVolume = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicVolume);
        else musicVolume = 1;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.FullScreen))
        {
            int f = PlayerPrefs.GetInt(PlayerPrefsKeys.FullScreen);
            if (f == 0)
            {
                fullScreen = false;
                Screen.fullScreen = false;
            }
            else
            {
                fullScreen = true;
                Screen.fullScreen = true;
            }
        }
        else
        {
            Screen.fullScreen = true;
            fullScreen = true;
        }
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.Width))
            screenWidth = PlayerPrefs.GetInt(PlayerPrefsKeys.Width);
        else screenWidth = Screen.resolutions[Screen.resolutions.Length - 1].width;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.Height))
            screenHeight = PlayerPrefs.GetInt(PlayerPrefsKeys.Height);
        else screenHeight = Screen.resolutions[Screen.resolutions.Length - 1].height;
    }
    public static void SetFullScreen(bool _fullScreen)
    {
        fullScreen = _fullScreen;
        if (Screen.fullScreen != fullScreen)
            Screen.fullScreen = !Screen.fullScreen;
        if (fullScreen == false)
            PlayerPrefs.SetInt(PlayerPrefsKeys.FullScreen, 0);
        else PlayerPrefs.SetInt(PlayerPrefsKeys.FullScreen, 1);
    }
    public static void SetSoundVolume(float _soundVolume)
    {
        soundVolume = _soundVolume;
        PlayerPrefs.SetFloat(PlayerPrefsKeys.SoundVolume, soundVolume);
    }
    public static void SetMusicVolume(float _musicVolume)
    {
        musicVolume = _musicVolume;
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicVolume, musicVolume);
    }
    public static void SetResolution(int index)
    {
        if (Screen.currentResolution.width != Screen.resolutions[index].width || Screen.currentResolution.height != Screen.resolutions[index].height)
        {
            Screen.SetResolution(Screen.resolutions[index].width, Screen.resolutions[index].height, fullScreen);
            screenWidth = Screen.resolutions[index].width;
            screenHeight = Screen.resolutions[index].height;
        }
        PlayerPrefs.SetInt(PlayerPrefsKeys.Width, screenWidth);
        PlayerPrefs.SetInt(PlayerPrefsKeys.Height, screenHeight);
    }
    public static void SaveSettings()
    {
        if (fullScreen == false)
            PlayerPrefs.SetInt(PlayerPrefsKeys.FullScreen, 0);
        else PlayerPrefs.SetInt(PlayerPrefsKeys.FullScreen, 1);
        PlayerPrefs.SetFloat(PlayerPrefsKeys.SoundVolume, soundVolume);
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicVolume, musicVolume);
        PlayerPrefs.SetInt(PlayerPrefsKeys.Width, screenWidth);
        PlayerPrefs.SetInt(PlayerPrefsKeys.Height, screenHeight);
    }
}
