using UnityEngine;

public class DontGoToKing : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public Player player;
    public DialogueManager dialogueManager;
    private bool show = true;

    void Update()
    {
        if (playerDetection.isPlayerNearby && !dialogueManager.dialogueActive && show)
        {
            player.isControllable = false;
            show = false;
            dialogueManager.ShowDialogue("!<NAME>!: I don't think it's a good idea...", true, 0, true, CloseDialogue);           
        }  
        if (!playerDetection.isPlayerNearby) show = true;  
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
    }
}
