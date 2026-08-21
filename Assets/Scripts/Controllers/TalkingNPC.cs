using System.Collections;
using UnityEngine;

public class TalkingNPC : MonoBehaviour
{
    public string[] introDialogueLines;
    public string[] dialogueLines;
    public DialogueManager dialogueManager;
    public PlayerDetection playerDetection;
    public Player player;
    private bool interactionActive = false;
    private bool introDialogue = true;

    void Update()
    {
        if (!interactionActive) {
            playerDetection.allowIcon = true;
            if (!dialogueManager.dialogueActive)
            {
                if (player.isControllable &&
                playerDetection.isPlayerNearby &&
                Input.GetKeyDown(KeyCode.F))
                {
                    player.isControllable = false;
                    interactionActive = true;
                    StartCoroutine(PlayDialogue(introDialogue ? introDialogueLines : dialogueLines));
                }
            }
        }
        else playerDetection.allowIcon = false;
    }

    public IEnumerator PlayDialogue(string[] dialogueLines)
    {
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
    }

    public void CloseDialogue(int nothing)
    {
        interactionActive = false;
        player.isControllable = true;
        introDialogue = false;
    }

}
