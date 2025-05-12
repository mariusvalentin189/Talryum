using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableSceneLoader : MonoBehaviour
{
    [SerializeField] int doorId;
    [SerializeField] int sceneId;
    [SerializeField] int spawnpointId;
    [SerializeField] GameObject doorLocked;
    [SerializeField] GameObject doorUnlocked;
    [SerializeField] GameObject interactUI;
    [SerializeField] TMP_Text interactText;
    [SerializeField] Key unlockKey;
    [SerializeField] bool isExit;
    public bool IsUnlocked = true;
    private void Awake()
    {
        if (!IsUnlocked)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.UnlockedDoor + doorId))
            {
                int u = PlayerPrefs.GetInt(PlayerPrefsKeys.UnlockedDoor + doorId);
                if (u == 1)
                {
                    IsUnlocked = true;
                }
            }
        }
        UpdateText();
    }
    private void Update()
    {
        if (interactUI.activeSelf)
        {
            if (Input.GetKeyDown(PlayerKeys.Instance.interactKey))
            {
                if (IsUnlocked)
                {
                    if (isExit)
                        LevelManager.Instance.UnlockLevel();
                    LevelManager.Instance.HandleSceneLoad(sceneId, spawnpointId);
                }
                else if (Keys.Instance.HasKey(unlockKey))
                {
                    IsUnlocked = true;
                    PlayerPrefs.SetInt(PlayerPrefsKeys.UnlockedDoor + doorId, 1);
                    UpdateText();
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactUI.SetActive(true);
            UpdateText();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactUI.SetActive(false);
        }
    }
    public void UpdateText()
    {
        string key = PlayerKeys.Instance.interactKey.ToString().ToLower();
        if (IsUnlocked)
            interactText.text = $"press {key} to enter";
        else if(Keys.Instance.HasKey(unlockKey))
            interactText.text = $"press {key} to unlock";
        else interactText.text = $"locked";
        CheckDoor();
    }
    void CheckDoor()
    {
        if (IsUnlocked)
        {
            doorLocked.SetActive(false);
            doorUnlocked.SetActive(true);
            return;
        }
        doorLocked.SetActive(true);
        doorUnlocked.SetActive(false);
    }
}
