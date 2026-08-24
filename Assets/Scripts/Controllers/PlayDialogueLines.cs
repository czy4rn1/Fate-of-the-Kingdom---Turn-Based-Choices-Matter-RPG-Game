using System;
using System.Collections;
using UnityEngine;

public class PlayDialogueLines : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public bool dialogueEnded = false;
    public IEnumerator PlayDialogue(string[] dialogueLines, Action<int> CloseDialogue = null)
    {
        dialogueEnded = false;
        if (dialogueLines != null) {
            for(int i=0; i<dialogueLines.Length; i++)
                {
                    bool last = false;
                    if (i == dialogueLines.Length-1) last = true;
                    dialogueManager.ShowDialogue(dialogueLines[i], true, 0, last, last ? CloseDialogue : null);
                    yield return null;
                    while (!dialogueManager.isWaitingForPlayer) yield return null;
                    while(dialogueManager.isWaitingForPlayer) yield return null; 
                }
            }
            dialogueEnded = true;    
    }
    public void PlayCommand(string line, byte numOfCommands, Action<int> OnCommandSelected)
    {
        dialogueManager.ShowDialogue(line, false, numOfCommands, true, OnCommandSelected);
    }

}
