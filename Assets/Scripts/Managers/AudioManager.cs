using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource sound;
    public AudioSource music;

    [Header("Game Music")]
    [SerializeField] AudioClip forestLevelMusic;
    [SerializeField] AudioClip villageLevelMusic;
    [SerializeField] AudioClip castleLevelMusic;
    [SerializeField] AudioClip undergroundLevelMusic;
    [SerializeField] AudioClip bossLevelMusic;
    [SerializeField] AudioClip menuMusic;

    [Header("UI Sounds")]
    [SerializeField] AudioClip buttonClick;
    public AudioClip textPopupSound;

    [Header("Game Sounds")]
    public AudioClip playerSwordSwing;
    public AudioClip playerSwordHitFlesh;
    public AudioClip playerSwordHitStone;
    public AudioClip playerSwordHitSlime;
    public AudioClip playerJump;
    public AudioClip playerDodge;
    public AudioClip playerLevelUp;
    public AudioClip playerMoveGrass;
    public AudioClip playerMoveStone;
    public AudioClip playerMoveWood;
    public AudioClip playerMoveGround;
    public AudioClip coinPickup;
    public AudioClip potionUse;

    [HideInInspector] public AudioClip currentStepSound;

    AudioSource[] additionalAudioSources;
    private void Awake()
    {
        StartCoroutine(PlayMusic());
    }
    public void SetSoundVolume(float volume)
    {
        sound.volume = volume / 1.5f;
        foreach (AudioSource source in additionalAudioSources)
        {
            if(source != null)
                if (source != sound && source != music)
                    source.volume = volume;
        }
    }
    public void SetMusicVolume(float volume)
    {
        music.volume = volume / 1.5f;
    }
    public void PlayButtonClickSound()
    {
        sound.PlayOneShot(buttonClick);
    }
    public void PlaySound(AudioClip sound)
    {
        this.sound.PlayOneShot(sound);
    }
    IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(0.5f);
        music.Stop();
        if (LevelManager.Instance.levelType == LevelType.forest)
        {
            if (forestLevelMusic)
            {
                music.clip = forestLevelMusic;
                music.Play();
            }
        }
        else if (LevelManager.Instance.levelType == LevelType.castle)
        {
            if (castleLevelMusic)
            {
                music.clip = castleLevelMusic;
                music.Play();
            }
        }
        else if (LevelManager.Instance.levelType == LevelType.underground)
        {
            if (undergroundLevelMusic)
            {
                music.clip = undergroundLevelMusic;
                music.Play();
            }
        }
        else if (LevelManager.Instance.levelType == LevelType.menu)
        {
            if (menuMusic)
            {
                music.clip = menuMusic;
                music.Play();
            }
        }
        else if (LevelManager.Instance.levelType == LevelType.village)
        {
            if (villageLevelMusic)
            {
                music.clip = villageLevelMusic;
                music.Play();
            }
        }
        StartCoroutine(LerpMusicVolume());
    }

    public void PlayBossMusic()
    {
        if (bossLevelMusic)
        {
            music.Stop();
            music.clip = bossLevelMusic;
            music.Play();
            StopAllCoroutines();
            StartCoroutine(LerpMusicVolume());
        }
    }
    public void ResumeMusic()
    {
        StartCoroutine(PlayMusic());
    }
    IEnumerator LerpMusicVolume()
    {
        float increment = music.volume / 4f;
        music.volume = 0f;
        while (music.volume < SettingsManager.musicVolume)
        {
            yield return new WaitForSeconds(0.5f);
            music.volume += increment;
            music.volume = Mathf.Clamp(music.volume, 0, SettingsManager.musicVolume);
        }
    }
    public void LoadAdditionalAudioSources()
    {
        additionalAudioSources = FindObjectsOfType<AudioSource>();
    }
}
