using UnityEngine;

public class Boat : MonoBehaviour
{
    public string location;
    public Player player;
    public DialogueManager dialogueManager;
    private bool isInteractable = true;
    public PlayerDetection playerDetection;
    void Update()
    {
        if (!isInteractable) {
            playerDetection.allowIcon = false;
            return;
        }
        else playerDetection.allowIcon = true;
        if (isInteractable && 
        player.isControllable && 
        playerDetection.isPlayerNearby &&
        Input.GetKeyDown(KeyCode.F))
        {
            isInteractable = false;
            player.isControllable = false;
            if (location == "Beach")
            {
                if (WorldState.Instance.fish_killed || WorldState.Instance.fish_questEnded)
                {
                    
                }
                else dialogueManager.ShowDialogue("There's a boat here. It might be useful.", true, 0, true, CloseDialogue);
            }
            else if (location == "Volsen")
            {
                
            }
        }
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        isInteractable = true;
    }
}
