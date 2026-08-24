using UnityEngine;

public class InitInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Player player;
    public PlayerDetection playerDetection;
    public bool interactionActive = false;
    public bool Interaction()
    {
        if (interactionActive) {
            playerDetection.allowIcon = false;
            return false;
        }
        else playerDetection.allowIcon = true;
        if (dialogueManager.dialogueActive) return false;
        if (!player.isControllable) return false;
        if (!playerDetection.isPlayerNearby) return false;
        if (!Input.GetKeyDown(KeyCode.F)) return false;
        return true;

    }
}
