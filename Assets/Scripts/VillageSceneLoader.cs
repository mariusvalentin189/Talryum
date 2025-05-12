using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageSceneLoader : MonoBehaviour
{
    [SerializeField] GameObject levelSelectUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            levelSelectUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
