using System.Collections;
using UnityEngine;

public class TalkingNPC : MonoBehaviour
{
    public string[] introDialogueLines;
    public string[] dialogueLines;
    public DialogueManager dialogueManager;
    public PlayDialogueLines dialoguePlayer;
    public PlayerDetection playerDetection;
    public Player player;
    private bool introDialogue = true;
    public InitInteraction initInteraction;

    void Update()
    {
        if (initInteraction.Interaction())
        {
            player.isControllable = false;
            initInteraction.interactionActive = true;
            StartCoroutine(dialoguePlayer.PlayDialogue(introDialogue ? introDialogueLines : dialogueLines, CloseDialogue));
            if (introDialogue) introDialogue = false;
        }
    }
    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        initInteraction.interactionActive = false;
        introDialogue = false;
    }

}
