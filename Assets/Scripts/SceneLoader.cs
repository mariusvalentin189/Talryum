using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] int sceneId;
    [SerializeField] int spawnpointId;
    [SerializeField] bool isExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (isExit)
                LevelManager.Instance.UnlockLevel();
            LevelManager.Instance.HandleSceneLoad(sceneId, spawnpointId);
        }
    }
}
