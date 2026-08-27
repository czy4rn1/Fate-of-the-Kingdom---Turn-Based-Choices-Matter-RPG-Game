using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    public InitInteraction initInteraction;
    public Player player;
    public PlayDialogueLines dialoguePlayer;
    public DialogueManager dialogueManager;
    public DialogueChoice[] dialogueChoices = new DialogueChoice[4];
    public string[] jewelBladeLines;
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
                byte id = 0;
                for(int i=0; i<dialogueChoices.Length; i++)
                {
                    dialogueChoices[i] = new DialogueChoice();
                }
                dialogueChoices[0].text = "Let me see what you're selling";
                dialogueChoices[0].id = id;
                dialogueChoices[1].text = "What do you know about the Jewel Blade?";
                dialogueChoices[2].text = "Do you know anything about the Devil's Magic?";
                dialogueChoices[3].text = "I don't need anything";

                List<DialogueChoice> finalDialogueChoices = new List<DialogueChoice>
                {
                    dialogueChoices[0]
                };
                id++;
                if (!WorldState.Instance.ponterQuestEnded)
                {
                    if (WorldState.Instance.askSellerAboutJewelBlade)
                    {
                        dialogueChoices[1].id = id;
                        finalDialogueChoices.Add(dialogueChoices[1]);
                        id++;
                    }
                    dialogueChoices[2].id = id;
                    finalDialogueChoices.Add(dialogueChoices[2]);
                    id++;
                }
                dialogueChoices[3].id = id;
                finalDialogueChoices.Add(dialogueChoices[3]);
                dialoguePlayer.PlayCommand("Merchant: What can I do for you?", finalDialogueChoices, OnChosenCommand);
            }
        }
    }

    public void OnChosenCommand(int command)
    {
        DialogueChoice chosen = dialogueChoices.FirstOrDefault(o => o.id == command);
        for(int i=0; i<dialogueChoices.Length; i++)
        {
            if (chosen.id == dialogueChoices[i].id)
            {
                if (i == 0)
                {
                    StartCoroutine(dialogueManager.HideShowPanel("hide"));
                    CloseDialogue(0); // CHANGE THIS LATER
                }
                else if (i == 1)
                {
                    StartCoroutine(dialoguePlayer.PlayDialogue(jewelBladeLines, CloseDialogue));
                    WorldState.Instance.learnedAboutHellsGarden = true;
                }
                else if (i == 2) {
                    string[] ponterQuestLines = {"Merchant: Me? Not exactly.", 
                    "Merchant: I do, sometimes, collect items that happen to be enchanted with magic, but I never dwelled deeper into it.",
                    "Merchant: You'll have to ask someone else."};
                    StartCoroutine(dialoguePlayer.PlayDialogue(ponterQuestLines, CloseDialogue));
                }
                else if (i == 3)
                {
                    StartCoroutine(dialogueManager.HideShowPanel("hide"));
                    CloseDialogue(0);
                }
            break;
            }
        }
    }
    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        initInteraction.interactionActive = false;
    }
}
