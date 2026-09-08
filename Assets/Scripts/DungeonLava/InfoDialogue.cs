using UnityEngine;

public class InfoDialogue : MonoBehaviour
{
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public string[] lines;

    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
        }
    } 
}
