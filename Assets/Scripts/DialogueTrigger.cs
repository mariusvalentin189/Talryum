using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] protected int id;
    [SerializeField] protected DialogueSystem dialogue;
    [SerializeField] protected string[] lines;
    protected PlayerController playerController;
    [SerializeField] protected DialogueTrigger nextDialogue;
    [SerializeField] GameObject objectToEnable;
    protected BoxCollider2D startCollider;
    protected int triggered;

    protected virtual void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        startCollider = GetComponent<BoxCollider2D>();
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.TriggeredDialogue + id))
            triggered = PlayerPrefs.GetInt(PlayerPrefsKeys.TriggeredDialogue + id);
        if (triggered == 1)
            Destroy(gameObject);
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            startCollider.enabled = false;
            playerController.EnterDialogue();
            dialogue.SetDialogue(lines,this);
        }
    }
    public virtual void SaveData()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.TriggeredDialogue + id, 1);
        if(nextDialogue)
        {
            nextDialogue.gameObject.SetActive(true);
        }
        if(objectToEnable)
        {
            objectToEnable.SetActive(true);
        }
    }
}
