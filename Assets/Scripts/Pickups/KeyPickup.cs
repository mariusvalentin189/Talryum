using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] Key key;
    [SerializeField] DialogueSystem dialogue;
    [SerializeField] string[] lines;
    PlayerController playerController;
    [SerializeField] TMP_Text interactText;
    [SerializeField] GameObject interactUI;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PickedUpKeys + key.id))
        {
            int picked = 0;
            picked = PlayerPrefs.GetInt(PlayerPrefsKeys.PickedUpKeys + key.id);
            if (picked == 1)
                Destroy(gameObject);
        }
        string keyText = PlayerKeys.Instance.interactKey.ToString().ToLower();
        interactText.text = $"press {keyText} to open";
    }
    private void Update()
    {
        if(interactUI.activeSelf)
        {
            if (Input.GetKeyDown(PlayerKeys.Instance.interactKey))
            {
                Keys.Instance.AddKey(key);
                LevelManager.Instance.UnlockLevel();
                PlayerPrefs.SetInt(PlayerPrefsKeys.PickedUpKeys + key.id, 1);
                dialogue.SetDialogue(lines, null);
                playerController.EnterDialogue();
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactUI.SetActive(false);
        }
    }
}
