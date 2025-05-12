using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : DialogueTrigger
{
    [SerializeField] GameObject tutorialObject;
    [SerializeField] AudioClip bushSound;
    [SerializeField] AudioSource bushSource;
    protected override void Start()
    {
        tutorialObject.SetActive(false);
        playerController = FindObjectOfType<PlayerController>();
        startCollider = GetComponent<BoxCollider2D>();
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.TriggeredDialogue + id))
            triggered = PlayerPrefs.GetInt(PlayerPrefsKeys.TriggeredDialogue + id);
        if (triggered == 1)
        {
            Destroy(tutorialObject);
            Destroy(gameObject);
        }
    }

    public override void SaveData()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.TriggeredDialogue + id, 1);
        tutorialObject.SetActive(true);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(StartDialogueLines());
        }
    }
    IEnumerator StartDialogueLines()
    {
        startCollider.enabled = false;
        playerController.EnterDialogue();
        yield return new WaitForSeconds(0.4f);
        bushSource.PlayOneShot(bushSound);
        yield return new WaitForSeconds(1f);
        dialogue.SetDialogue(lines, this);
    }
}
