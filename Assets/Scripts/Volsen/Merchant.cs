using UnityEngine;

public class Merchant : MonoBehaviour
{
    public InitInteraction initInteraction;
    public Player player;
    public PlayDialogueLines dialoguePlayer;
    public DialogueManager dialogueManager;
    public string[] jewelBladeLines;
    public string[] ponterQuestLines = {"Merchant: Me? Not exactly.", 
    "Merchant: I do, sometimes, collect items that happen to be enchanted with magic, but I never dwelled deeper into it.",
    "Merchant: You'll have to ask someone else."};
    private string command;
    private byte commandType = 0;
    public TalkingNPC talkingNPC;

    void Start()
    {
        if (WorldState.Instance.ponterQuestStarted) talkingNPC.enabled = false;
    }

    void Update()
    {
        if (WorldState.Instance.ponterQuestStarted)
        {
            if (initInteraction.Interaction())
            {
                player.isControllable = false;
                initInteraction.interactionActive = true;
                if (!WorldState.Instance.ponterQuestEnded)
                {
                    if (WorldState.Instance.askSellerAboutJewelBlade)
                    {
                        commandType = 1;
                        command = "Merchant: How can I help you?\n" + 
                        "1. Let me see what you're selling\n" + 
                        "2. What do you know about the Jewel Blade?\n" +
                        "3. Do you know anything about the Devil's magic?\n" +
                        "4. I don't need anything";
                        dialoguePlayer.PlayCommand(command, 4, OnChosenCommand);
                    }
                    else
                    {
                        commandType = 3;
                        command = "Merchant: How can I help you?\n" + 
                        "1. Let me see what you're selling\n" + 
                        "2. Do you know anything about the Devil's magic?\n" +
                        "3. I don't need anything";
                        dialoguePlayer.PlayCommand(command, 3, OnChosenCommand);
                    }
                }
                else
                {
                    commandType = 2;
                    command = "Merchant: How can I help you?\n" + 
                    "1. Let me see what you're selling\n" + 
                    "2. I don't need anything";
                    dialoguePlayer.PlayCommand(command, 2, OnChosenCommand);
                }
            }
        }
    }

    public void OnChosenCommand(int command)
    {
        if (commandType == 1)
        {
            if (command == 0)
            {
            
            }
            else if (command == 1)
            {
                StartCoroutine(dialoguePlayer.PlayDialogue(jewelBladeLines, CloseDialogue));
                WorldState.Instance.learnedAboutHellsGarden = true;
            }
            else if (command == 2)
            {
                StartCoroutine(dialoguePlayer.PlayDialogue(ponterQuestLines, CloseDialogue));
            }
            else if (command == 3)
            {
                StartCoroutine(dialogueManager.HideShowPanel("hide"));
                CloseDialogue(0);
            }
        }
        else if (commandType == 2)
        {
            if (command == 0)
            {
            
            }
            else if (command == 1)
            {
                StartCoroutine(dialogueManager.HideShowPanel("hide"));
                CloseDialogue(0);
            }
        }
        else if (commandType == 3)
        {
            if (command == 0)
            {
            
            }
            else if (command == 1)
            {
                StartCoroutine(dialoguePlayer.PlayDialogue(ponterQuestLines, CloseDialogue));
            }
            else if (command == 2)
            {
                StartCoroutine(dialogueManager.HideShowPanel("hide"));
                CloseDialogue(0);
            }
        }
        commandType = 0;
    }
    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        initInteraction.interactionActive = false;
    }
}
