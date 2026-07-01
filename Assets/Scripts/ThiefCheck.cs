using UnityEngine;
using UnityEngine.SceneManagement;

public class ThiefCheck : MonoBehaviour
{
    public Character character;
    public Player player;
    public DialogueManager dialogueManager;
    public PlayerDetection playerDetection;
    private bool checkEnded = false;
    public byte req_lvl;
    private bool interactionActive = false;

    
    void Update()
    {
        if(interactionActive) return;
        if(character.wait) {
            if (!interactionActive && 
            playerDetection != null && 
            playerDetection.isPlayerNearby && 
            Input.GetKeyDown(KeyCode.F) && 
            player.isControllable && 
            !dialogueManager.dialogueActive)
            {
                interactionActive = true;
                character.activateAI(false);
                player.isControllable = false;
                if (!checkEnded) {
                dialogueManager.ShowDialogue(
                    $"\n1. [DEX {PlayerData.Instance.dexterity}/{req_lvl}] Steal the key to your cell.\n2. Ignore", 
                    false, 
                    2, 
                    false, 
                    OnCommandSelected);
                }
                else
                {
                    dialogueManager.ShowDialogue(
                    $"\n1. Ignore", 
                    false, 
                    1, 
                    false, 
                    OnCommandSelected); 
                }
            }
        }
        else
        {
            playerDetection.allowIcon = false;
        }
    }
    public void OnCommandSelected(int chosenCommand)
    {
        if (chosenCommand == 0 && !checkEnded)
        {
            if (PlayerData.Instance.dexterity >= req_lvl)
            {
                dialogueManager.ShowDialogue("Obtained Key To The Cell", true, 0, true, CloseDialogue);
            }
            else 
            {
                dialogueManager.ShowDialogue("GUARD: Try this manouver again, and I'll cut off your hands!", true, 0, true, CloseDialogue);
            }
            checkEnded = true;
        }
        else if (chosenCommand == 1 || (chosenCommand == 0 && checkEnded))
        {
            StopAllCoroutines();
            StartCoroutine(dialogueManager.HideShowPanel("hide"));
            CloseDialogue(0);
        }
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        character.activateAI(true);
        interactionActive = false;
        character.wait = false;
    }
}

