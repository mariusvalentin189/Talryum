using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnSystem : Singleton<PlayerSpawnSystem>
{
    [SerializeField] Transform defaultSpawnPoint;
    [SerializeField] Checkpoint[] levelSpawnPoints;
    Transform player;
    int currenctCheckpoint=-1;
    GameCamera gc;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        gc = FindObjectOfType<GameCamera>();
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentSpawnPoint))
        {
            int spawnPosition = PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentSpawnPoint);
            player.transform.position = levelSpawnPoints[spawnPosition].transform.position;
            if (levelSpawnPoints[spawnPosition].FaceLeft)
            {
                player.GetComponent<PlayerController>().SpinPlayer();
            }
        }
        else if (defaultSpawnPoint)
        {
            player.transform.position = defaultSpawnPoint.position;
        }
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentScene, SceneManager.GetActiveScene().buildIndex);
        CheckCheckpoints();
    }
    public void RespawnPlayer()
    {
        gc.FadeIn();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public void CheckCheckpoints()
    {
        if (levelSpawnPoints.Length > 0)
        {
            if (currenctCheckpoint == -1)
            {
                if (PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentLevelCheckpoint))
                {
                    currenctCheckpoint = PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelCheckpoint);
                    levelSpawnPoints[currenctCheckpoint].EnableFlag();
                    player.position = levelSpawnPoints[currenctCheckpoint].transform.position;
                    if (levelSpawnPoints[currenctCheckpoint].FaceLeft)
                    {
                        player.GetComponent<PlayerController>().SpinPlayer();
                    }
                }
                else player.position = defaultSpawnPoint.position;
            }
            else player.position = levelSpawnPoints[currenctCheckpoint].transform.position;
        }
    }
    public void AssignCheckpoint(Transform checkpoint)
    {
        for(int i=0;i<levelSpawnPoints.Length;i++)
        {
            if(levelSpawnPoints[i].name==checkpoint.name)
            {
                if (currenctCheckpoint != i)
                {
                    if(currenctCheckpoint!=-1)
                        levelSpawnPoints[currenctCheckpoint].ResetCheckpoint();
                    currenctCheckpoint = i;
                    PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentLevelCheckpoint, currenctCheckpoint);
                    break;
                }
            }
        }
    }
}
