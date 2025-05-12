using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] TMP_Text textDialogue;
    [SerializeField] GameObject continueText;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] float textSpeed;
    [SerializeField] bool requiresDefeatedEnemy;
    string[] lines;
    float currentTextSpeed;
    AudioManager audioManager;
    public bool IsInDialogue { get; set; }
    public bool FinishedDialogue { get; set; }
    public bool StartedDialogue { get; set; }
    public DialogueTrigger AttachedDialogueTrigger { get; set; }
    PlayerKeys keys;
    int index;
    void Start()
    {
        keys = FindObjectOfType<PlayerController>().GetComponent<PlayerKeys>();
        textDialogue.text = string.Empty;
        continueText.SetActive(false);
        dialoguePanel.SetActive(false);
        audioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keys.interactKey))
        {
            if (continueText.activeSelf)
            {
                NextLine();
            }
            else
            {
                currentTextSpeed = 0f;
            }
        }
    }
    public void SetDialogue(string[] newDialogue, DialogueTrigger trigger)
    {
        AttachedDialogueTrigger = trigger;
        IsInDialogue = true;
        StartedDialogue = true;
        FinishedDialogue = false;
        lines = newDialogue;
    }
    public void StartDialogue()
    {
        index = 0;
        dialoguePanel.SetActive(true);
        currentTextSpeed = textSpeed;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach (char ch in lines[index].ToCharArray())
        {
            textDialogue.text += ch;
            if (currentTextSpeed > 0f)
            {
                audioManager.PlaySound(audioManager.textPopupSound);
                yield return new WaitForSeconds(currentTextSpeed);
            }
        }
        continueText.SetActive(true);
    }
    void NextLine()
    {
        currentTextSpeed = textSpeed;
        continueText.SetActive(false);
        if (index == lines.Length - 1)
        {
            dialoguePanel.SetActive(false);
            FinishedDialogue = true;
            StartedDialogue = false;
            IsInDialogue = false;
            if(!requiresDefeatedEnemy && AttachedDialogueTrigger)
                AttachedDialogueTrigger.SaveData();
            return;
        }
        index += 1;
        textDialogue.text = string.Empty;
        StartCoroutine(TypeLine());
    }
}
