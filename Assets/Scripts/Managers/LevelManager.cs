using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public LevelType levelType;
    public int levelNumber;
    public Key[] possibleKeys;
    [SerializeField] int villageSceneId;
    GameObject leaveVillageUI;
    int previousSceneId;
    int sceneToLoad;
    int spawnpointId;
    GameCamera gc;
    private void Awake()
    {
        gc = FindObjectOfType<GameCamera>();
        leaveVillageUI = GameUI.Instance.LeaveVillageUI;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PreviousScene))
            previousSceneId = PlayerPrefs.GetInt(PlayerPrefsKeys.PreviousScene);
    }
    public void HandleSceneLoad(int sceneId, int spawnpoint)
    {
        sceneToLoad = sceneId;
        spawnpointId = spawnpoint;
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentLevelCheckpoint, spawnpointId);
        SceneManager.LoadSceneAsync(sceneId);
    }
    public void LoadVillageScene ()
    {
        Time.timeScale = 1;
        gc.FadeIn();
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentLevelCheckpoint, 1);
        PlayerPrefs.SetInt(PlayerPrefsKeys.PreviousScene, SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync(villageSceneId);
    }
    public void LoadPreviousScene()
    {
        Time.timeScale = 1;
        gc.FadeIn();
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentLevelCheckpoint, 0);
        SceneManager.LoadSceneAsync(previousSceneId);
    }
    public void LoadSceneById()
    {
        Time.timeScale = 1;
        gc.FadeIn();
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentLevelCheckpoint, spawnpointId);
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
    public void UnlockLevel()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.LevelCompleted + levelNumber, 1);
    }
}
public enum LevelType
{
    forest,
    underground,
    castle,
    menu,
    village
}
