using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;


public class ThiefCheck : MonoBehaviour
{
    public Character character;
    public Player player;
    public DialogueManager dialogueManager;
    public PlayerDetection playerDetection;
    private bool checkEnded = false;
    public byte req_lvl;
    private bool interactionActive = false;
    private float waitTime = 12f;
    private bool dialogueExtinguished = false;
    
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
                    $"You see a Key to the Cell in the guard's pocket.\n1. [DEX {PlayerData.Instance.dexterity}/{req_lvl}] Steal the key\n2. Ignore", 
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
        if(WorldState.Instance.keyStolen)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                if (!dialogueExtinguished) {
                    character.rightX = 9.96f;
                    interactionActive = true;
                    character.activateAI(false);
                    player.isControllable = false;
                    string[] dialogueLines = {"GUARD: Wait, where is my key?",
                    "GUARD: It should be in my back pocket...",
                    "GUARD: Oh Lord, it's not there!",
                    "GUARD: Oh, I know... It must have been Ursus. That sneaky little bastard!",
                    "GUARD: It better be him, otherwise I'm dead!"};
                    StartCoroutine(PlayDialogue(dialogueLines));
                    character.movement.x = 1f;
                    character.moveSpeed = 10f;
                    character.kill = true;
                    dialogueExtinguished = true;
                }
            }  
        }
    }
    public void OnCommandSelected(int chosenCommand)
    {
        if (chosenCommand == 0 && !checkEnded)
        {
            if (PlayerData.Instance.dexterity >= req_lvl)
            {
                dialogueManager.ShowDialogue("Obtained Key To The Cell", true, 0, true, CloseDialogue);
                PlayerData.Instance.AddItem("Key To The Cell");
                WorldState.Instance.keyStolen = true;
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

    public IEnumerator PlayDialogue(string[] dialogueLines)
    {
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

