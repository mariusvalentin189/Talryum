using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Settings : Singleton<Settings>
{
    [Header("Volume")]
    public Slider soundVolumeSlider;
    public Slider musicVolumeSlider;
    [Header("Resolution")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    [Header("Controls")]
    Resolution[] resolutions;
    int selectedResolutionIndex;
    public float SoundVolume { get; private set; }
    public float MusicVolume { get; private set; }
    bool fullScreen;
    int savedIndex;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = AudioManager.Instance;
        audioManager.LoadAdditionalAudioSources();
        SettingsManager.LoadSettings();
        SoundVolume = SettingsManager.soundVolume;
        MusicVolume = SettingsManager.musicVolume;
        audioManager.SetSoundVolume(SoundVolume);
        audioManager.SetMusicVolume(MusicVolume);
        soundVolumeSlider.value = SettingsManager.soundVolume;
        musicVolumeSlider.value = SettingsManager.musicVolume;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int maxResolutionHz = 0;
        for (int i = 0; i < resolutions.Length / 2; i++)
        {
            if (resolutions[i].refreshRate > maxResolutionHz)
                maxResolutionHz = resolutions[i].refreshRate;
        }
        List<string> resolutionsList = new List<string>();
        int currentResolutionIndex = 0;
        for(int i=0;i<resolutions.Length;i++)
        {
            if (resolutions[i].width == SettingsManager.screenWidth && resolutions[i].height == SettingsManager.screenHeight)
                currentResolutionIndex = i;
            if (resolutions[i].refreshRate == maxResolutionHz)
            {
                string res = resolutions[i].width + " x " + resolutions[i].height;
                resolutionsList.Add(res);
            }
        }
        resolutionDropdown.AddOptions(resolutionsList);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        fullScreenToggle.isOn = SettingsManager.fullScreen;
        savedIndex = currentResolutionIndex;
    }
    public void SetSoundVolume(float value)
    {
        SoundVolume = value;
        audioManager.SetSoundVolume(SoundVolume);
        SettingsManager.SetSoundVolume(SoundVolume);
    }
    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        audioManager.SetMusicVolume(MusicVolume);
        SettingsManager.SetMusicVolume(MusicVolume);
    }
    public void SetFullScreen(bool value)
    {
        fullScreen = value;
        SettingsManager.SetFullScreen(fullScreen);
    }
    public void SetResolution(int index)
    {
        selectedResolutionIndex = index;
        SettingsManager.SetResolution(selectedResolutionIndex);
    }
    public void ChangeMusicVolume(bool increase)
    {
        if (increase)
            MusicVolume += 0.1f;
        else MusicVolume -= 0.1f;
        audioManager.SetMusicVolume(MusicVolume);
        SettingsManager.SetMusicVolume(MusicVolume);
        musicVolumeSlider.value = MusicVolume;
        audioManager.PlayButtonClickSound();
        MusicVolume = Mathf.Clamp(MusicVolume, 0, 1);
    }
    public void ChangeSoundVolume(bool increase)
    {
        if (increase)
            SoundVolume += 0.1f;
        else SoundVolume -= 0.1f;
        audioManager.SetSoundVolume(SoundVolume);
        SettingsManager.SetSoundVolume(SoundVolume);
        soundVolumeSlider.value = SoundVolume;
        audioManager.PlayButtonClickSound();
        SoundVolume = Mathf.Clamp(SoundVolume, 0, 1);
    }
    public void PlayButtonSound()
    {
        audioManager.PlayButtonClickSound();
    }
}
