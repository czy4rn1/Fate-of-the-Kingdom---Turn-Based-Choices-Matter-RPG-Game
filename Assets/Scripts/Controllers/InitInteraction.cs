using UnityEditor.UI;
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
        player.isControllable = false;
        interactionActive = true;
        return true;

    }

    public bool Interaction(bool dontStop)
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

    public void CloseInteraction(int nothing)
    {
        player.isControllable = true;
        interactionActive = false;
    }

    public void EnableInteraction(bool enable)
    {
        playerDetection.allowIcon = enable;
        if (!enable) playerDetection.interactIcon.SetActive(enable);
        playerDetection.enabled = enable;
        this.enabled = enable;
    }
}
