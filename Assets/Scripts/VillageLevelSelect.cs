using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageLevelSelect : MonoBehaviour
{
    [SerializeField] GameObject[] levelSelectButtons;
    private void Awake()
    {
        for (int i=0; i<levelSelectButtons.Length; i++)
        {
            int unlocked = 0;
            int id = i + 1;
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.LevelCompleted + id))
                unlocked = PlayerPrefs.GetInt(PlayerPrefsKeys.LevelCompleted + id);
            if (unlocked == 1)
                levelSelectButtons[i].SetActive(true);
            else
            {
                levelSelectButtons[i].SetActive(true);
                break;
            }
        }
    }
    public void Back()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void LoadLevel(int sceneId)
    {
        Time.timeScale = 1;
        LevelManager.Instance.HandleSceneLoad(sceneId, 0);
    }
}
