using System.Collections;
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

    
    void Update()
    {
        if (!isInteractable) {
            playerDetection.allowIcon = false;
            return;
        }
        else playerDetection.allowIcon = true;
        if (choiceEnded && WorldState.Instance.fish_killed) gameObject.SetActive(false);
        if (isInteractable && 
        playerDetection.isPlayerNearby && 
        player.isControllable && 
        Input.GetKeyDown(KeyCode.F)) {
            isInteractable = false;
            player.isControllable = false;
            if (!introEnded) {
                StartCoroutine(PlayDialogue(introDialouge));
                introEnded = true;
            }
            else {
                if (!choiceEnded) dialogueManager.ShowDialogue("Would you help a poor old bastard like me?\n" +
                "1. Sure, I'll do it for you.\n" +
                $"2. [PER {PlayerData.Instance.persuasion}/{req_per}] Do you realize what's on the line?\n" +
                "3. Attack him\n" +
                "4. I have to think about it", false, 4, true, OnCommandSelected);
                if (WorldState.Instance.fish_questStarted && !WorldState.Instance.fish_questEnded)
                {
                    if (collectFishes.fishes_collected < 7) {
                        string fishes = collectFishes.fishes_collected == 1 ? "fish" : "fishes";
                        string text = $"Fisherman: You've collected {collectFishes.fishes_collected} {fishes}! {7-collectFishes.fishes_collected} more to go!";
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
            choiceEnded = true;
        }
        else if (command == 1) {
            if (PlayerData.Instance.persuasion >= req_per)
            {
               string[] dialogueLines = {"!<NAME>!: Please, listen to me. You might not realize it, because you've left the civilization years ago, but we've entered the dark ages",
               "!<NAME>!: I haven't introduced myself to you. My name is !<NAME>!, and for the last year I've been a part of a rebellion.",
               "!<NAME>!: Our mission is to overthrow Lord Magnus. Under his dynasty many of my friends have died, been captured or executed by his battalion.",
               "!<NAME>!: With each month his army is growing bigger and stronger, and we were not certain, how he convinces people to join him.",
               "!<NAME>!: Even a couple of former members of the rebellion have joined his army. One of them was my best friend.",
               "!<NAME>!: I have just escaped his jail, and I managed to have a closer look at his knights.",
               "!<NAME>!: Every single one of them had black eyes, some of them were worshipping him like a god. Their movements were unnatural, and many times it looked like they were fighting themselves from the inside.",
               "!<NAME>!: I am certain he is a user of the Devil Magic, and I have to stop him before he brings Apocalypse on all of us.",
               "!<NAME>!: And that includes you, as well.",
               "!<NAME>!: So please, let's not delay my mission any further, and please transport me to Vilson.",
               "Fisherman: I-uh... I see... I'm really sorry, I didn't know things got this bad up there...",
               "Fisherman: I left when my son was convinced to join Magnus' army. It happened 3 years ago...",
               "Fisherman: We had an awful fight - many epithets thrown at each other, and then it turned into a brawl.",
               "Fisherman: He beat me up pretty badly.",
               "Fisherman: It broke my heart, but that day I considered my son dead.",
               "Fisherman: I decided to leave the town. Heartbroken, weak and lost of any hope.",
               "Fisherman: I know that I'm an old man and my help isn't much worth, but I'd like to help you in any way I can.",
               "Fisherman: Do you think it'd be possible to bring my son back?",
               "!<NAME>!: Currently with the knowledge I have, I am not able to answer this question, but my goal is to save as many people as possible.",
               "!<NAME>!: I'll do everything I can to bring your son back.",
               "Fisherman: Thank you, !<NAME>!. Get in the boat, whenever you're ready."
               }; 
               WorldState.Instance.fish_questStarted = WorldState.Instance.fish_questEnded = WorldState.Instance.fish_willHelp = true;
               StartCoroutine(PlayDialogue(dialogueLines));
               choiceEnded = true;
            }
            else
            {
               string[] dialogueLines = {"!<NAME>!: Look, I don't have time for this.",
               "!<NAME>!: I'm on a very important mission, and I have to get to Vilson as soon as possible.",
               "!<NAME>!: Just quit talking, get on the boat and transport me there.",
               "Fisherman: Yeah sure, bud. And I'm the king of Atlantis! Now get me some fish, please."}; 
               StartCoroutine(PlayDialogue(dialogueLines));
            }
        }
        else if (command == 2) {
            WorldState.Instance.fish_killed = true;
            string[] dialogueLines = {"!<NAME>!: You could have made it easy on yourself, but you've chosen otherwise.", "!<NAME>!: That's on you, old man"}; 
            StartCoroutine(PlayDialogue(dialogueLines));
            choiceEnded = true;
        }
        else if (command == 3)
        {
            dialogueManager.ShowDialogue("Fisherman: Well, I'm not going anywhere! You'll find me here.", true, 0, true, CloseDialogue);
        }
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
    }
}
