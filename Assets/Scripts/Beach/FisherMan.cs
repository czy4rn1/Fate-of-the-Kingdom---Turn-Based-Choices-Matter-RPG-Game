using System.Collections;
using TMPro;
using UnityEngine;

public class FisherMan : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public Player player;
    public DialogueManager dialogueManager;
    public string[] introDialouge;
    private bool isInteractable = true;
    private bool introEnded = false;
    private bool choiceEnded = false;
    public byte req_per;
    public CollectFishes collectFishes;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (!isInteractable) {
            playerDetection.allowIcon = false;
            return;
        }
        else playerDetection.allowIcon = true;
        if (isInteractable && 
        playerDetection.isPlayerNearby && 
        player.isControllable && 
        Input.GetKeyDown(KeyCode.F)) {
            isInteractable = false;
            player.isControllable = false;
            if (!introEnded) StartCoroutine(PlayDialogue(introDialouge));
            else {
                if (!choiceEnded) dialogueManager.ShowDialogue("Would you help a poor old bastard like me?\n" +
                "1. Sure, I'll do it for you.\n" +
                $"2. [PER {PlayerData.Instance.persuasion}/{req_per}] Do you realize what's on the line?\n" +
                "3. Attack him\n" +
                "4. I have to think about it", false, 4, true, OnCommandSelected);
                if (WorldState.Instance.fish_questStarted && !WorldState.Instance.fish_questEnded)
                {
                    if (collectFishes.fishes_collected < 5) {
                        string fishes = collectFishes.fishes_collected == 1 ? "fish" : "fishes";
                        string text = $"Fisherman: You've collected {collectFishes.fishes_collected} {fishes}! {5-collectFishes.fishes_collected} more to go!";
                        dialogueManager.ShowDialogue(text, true, 0, true, CloseDialogue);
                    }
                    else
                    {
                        string[] dialogueLines = {"Fisherman: Ohohoh, thank you so much! You have no idea how much easier you made my next week.", 
                        "Fisherman: Very well, I's time for me to keep my end of the bargain now.",
                        "Fisherman: Get in the boat, whenever you're ready.",
                        "Fisherman: Oh, and one more thing. Please take this. You'll have more use of this than me, anyway.",
                        "Obtained x1 of DEX Up",
                        "Fisherman: Feel free to come back any time. You'll always be welcome here."};
                        StartCoroutine(PlayDialogue(dialogueLines));
                        PlayerData.Instance.AddItem("DEX Up");
                        WorldState.Instance.fish_questEnded = true;
                    }
                }
                else if (WorldState.Instance.fish_questStarted && WorldState.Instance.fish_questEnded)
                {
                    dialogueManager.ShowDialogue("Fisherman: Get in the boat, whenever you're ready.", true, 0, true, CloseDialogue);
                }
            }
        }
    }

    public void OnCommandSelected(int command) {
        if (command == 0) {
            WorldState.Instance.fish_questStarted = true;
            dialogueManager.ShowDialogue("Fisherman: Alrighty, then! I'll be waiting for you right here.", true, 0, true, CloseDialogue);
        }
        else if (command == 1) {
            if (PlayerData.Instance.persuasion >= req_per)
            {
                
            }
            else
            {
                
            }
        }
        else if (command == 2) {
            WorldState.Instance.fish_killed = true;
        }
        else if (command == 3)
        {
            dialogueManager.ShowDialogue("Fisherman: Well, I'm not going anywhere! You'll find me here.", true, 0, true, CloseDialogue);
        }
        if (command != 3) choiceEnded = true;
    }
    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        isInteractable = true;
    }

    private IEnumerator PlayDialogue(string[] dialogueLines)
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
        introEnded = true;
    }
}
