using System.Collections;
using UnityEngine;

public class FisherMan : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public Player player;
    public string[] introDialouge;
    public byte req_per;
    public CollectFishes collectFishes;
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public string[] perSuccess;
    public string[] perFail;
    public string[] standardEnd;
    
    void Start()
    {
        if (WorldState.Instance.fisher_state == (byte)WorldState.Fisher.killed) Destroy(gameObject);
    }
    
    void Update()
    {
        if (WorldState.Instance.fisher_state == (byte)WorldState.Fisher.killed) Destroy(gameObject);
        if (initInteraction.Interaction()) {
            StartCoroutine(Dialogue());
        } 
    }

    public void OnCommandSelected(int command) {
        if (command == 0) {
            WorldState.Instance.fisher_state = (byte)WorldState.Fisher.questStarted;
            StartCoroutine(dialoguePlayer.PlayDialogue(new[] {"Fisherman: Alrighty, then! I'll be waiting for you right here."},initInteraction.CloseInteraction));
        }
        else if (command == 1) {
            if (PlayerData.Instance.persuasion >= req_per)
            {
               WorldState.Instance.fisher_state = (byte)WorldState.Fisher.willHelp;
               StartCoroutine(dialoguePlayer.PlayDialogue(perSuccess, initInteraction.CloseInteraction));
            }
            else
            {
               StartCoroutine(dialoguePlayer.PlayDialogue(perFail, initInteraction.CloseInteraction));
            }
        }
        else if (command == 2) {
            StartCoroutine(KillFisher());   
        }
        else if (command == 3)
        {
            StartCoroutine(dialoguePlayer.PlayDialogue(new[] {"Fisherman: Well, I'm not going anywhere! You'll find me here."}, initInteraction.CloseInteraction));
        }
    }

    IEnumerator KillFisher()
    {
        string[] dialogueLines = {"!<NAME>!: You could have made it easy on yourself, but you've chosen otherwise.", "!<NAME>!: That's on you, old man"}; 
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(dialogueLines, initInteraction.CloseInteraction));
        WorldState.Instance.fisher_state = (byte)WorldState.Fisher.killed;
    }

    IEnumerator Dialogue()
    {
        if (WorldState.Instance.fisher_state == (byte)WorldState.Fisher.introNotPlayed) {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(introDialouge));
                WorldState.Instance.fisher_state = 5;
            }
        if (WorldState.Instance.fisher_state < (byte)WorldState.Fisher.questStarted) {
            string command = "Would you help a poor old bastard like me?\n" +
            "1. Sure, I'll do it for you.\n" +
            $"2. [PER {PlayerData.Instance.persuasion}/{req_per}] Do you realize what's on the line?\n" +
            "3. Attack him\n" +
            "4. I have to think about it";
            dialoguePlayer.PlayCommand(command, 4, OnCommandSelected);
        }
        else if (WorldState.Instance.fisher_state < (byte)WorldState.Fisher.questEnded)
        {
            if (collectFishes.fishes_collected < 7) {
                string fishes = collectFishes.fishes_collected == 1 ? "fish" : "fishes";
                string text = $"Fisherman: You've collected {collectFishes.fishes_collected} {fishes}! {7-collectFishes.fishes_collected} more to go!";
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new[] {text}, initInteraction.CloseInteraction));
            }
            else
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(standardEnd, initInteraction.CloseInteraction));
                PlayerData.Instance.AddItem("DEX Up");
                WorldState.Instance.fisher_state = (byte)WorldState.Fisher.questEnded;
            }
        }
        else if (WorldState.Instance.fisher_state >= (byte)WorldState.Fisher.questEnded)
        {
            if (WorldState.Instance.playerPosessed) yield return StartCoroutine(dialoguePlayer.PlayDialogue(new[] {"Fisherman: You look very sickly. You're beginning to look like my son when he chose to join Magnus..."}));
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new[] {"Fisherman: Get in the boat, whenever you're ready."},initInteraction.CloseInteraction));
        }

    }

}

