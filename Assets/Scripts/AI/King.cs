using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    [SerializeField] float animationToPlay;
    [SerializeField] Animator anim;
    [SerializeField] DialogueSystem dialogue;
    [SerializeField] PlayerController player;
    private void Start()
    {
        anim.SetFloat("Animation", animationToPlay);
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(StartDialogue());
    }
    IEnumerator StartDialogue()
    {
        yield return new WaitUntil(() => dialogue.IsInDialogue == true);
        dialogue.StartDialogue();
        if(player.kneelOnDialogue)
        {
            player.anim.SetTrigger("Kneel");
        }
        yield return new WaitUntil(() => dialogue.FinishedDialogue == true);
        if (player.kneelOnDialogue)
        {
            player.anim.SetTrigger("KneelUp");
        }
        else player.CanMove = true;
    }
}
