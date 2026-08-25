using UnityEngine;

public class Merchant : MonoBehaviour
{
    public InitInteraction initInteraction;
    public Player player;
    public PlayDialogueLines dialoguePlayer;
    public DialogueManager dialogueManager;
    public string[] jewelBladeLines;
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
                if (WorldState.Instance.askSellerAboutJewelBlade)
                {
                    commandType = 1;
                    command = "Merchant: How can I help you?\n" + 
                    "1. Let me see what you're selling\n" + 
                    "2. What do you know about the Jewel Blade?\n" +
                    "3. I don't need anything";
                    dialoguePlayer.PlayCommand(command, 3, OnChosenCommand);
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
        commandType = 0;
    }
    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        initInteraction.interactionActive = false;
    }
}
