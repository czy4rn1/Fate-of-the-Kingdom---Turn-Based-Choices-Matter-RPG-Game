using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class PonterVolsenInn : MonoBehaviour
{
    public Player player;
    public DialogueManager dialogueManager;
    public PlayableDirector ponterEntersInn;
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    private DialogueChoice[] storyChoices = new DialogueChoice[4];
    private DialogueChoice[] dialogueChoices = new DialogueChoice[3];
    public string[] introLines;
    private byte commandType = 0;
    public string[] jewelBladeDialogue;
    public string[] informationDialogue;
    public string[] castleChoice;
    public string[] raggenfallChoice;
    public string[] blossomGardenChoice;
    public string[] mayhemIslandChoice;
    private string command;
    private int chosenPath = 255;
    private bool choosePathAgain = false;
    private bool infoPlayed = false;
    void Start()
    {
        
        if (!WorldState.Instance.keyStolen)
        {
            transform.position = new Vector2(-6.51f, -0.72f);
        }
        if (WorldState.Instance.ponterQuestEnded)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            player.isControllable = false;
            initInteraction.interactionActive = true;
            if (!WorldState.Instance.ponterQuestStarted) {
                StartCoroutine(dialoguePlayer.PlayDialogue(introLines, CloseDialogue));
                WorldState.Instance.ponterQuestStarted = true;
            }
            else
            {   
                if (!WorldState.Instance.ponterQuestEnded) {
                command = "";
                    if (!WorldState.Instance.ponterInfoObtained && !WorldState.Instance.jewelBladeObtained)
                    {
                        StartCoroutine(dialoguePlayer.PlayDialogue(new []{"Ponter: Come back to me when you find out anything."}, CloseDialogue));
                    }
                    else
                    {
                        commandType = 1;
                        byte id = 0;
                        for(int i=0; i<dialogueChoices.Length; i++)
                        {
                            dialogueChoices[i] = new DialogueChoice();
                        }
                        dialogueChoices[0].text = "Give him the information you've obtained";
                        dialogueChoices[1].text = "Tell him about the Jewel Blade";
                        dialogueChoices[2].text = "Nothing";

                        List<DialogueChoice> finalDialogueChoices = new List<DialogueChoice>();

                        if (WorldState.Instance.ponterInfoObtained)
                        {
                            dialogueChoices[0].id = id;
                            finalDialogueChoices.Add(dialogueChoices[0]);
                            id++;
                        }
                        if (WorldState.Instance.jewelBladeObtained)
                        {
                            dialogueChoices[1].id = id;
                            finalDialogueChoices.Add(dialogueChoices[1]);
                            id++;
                        }
                        dialogueChoices[2].id = id;
                        finalDialogueChoices.Add(dialogueChoices[2]);
                        dialoguePlayer.PlayCommand("Ponter: Do you want to tell me something?", finalDialogueChoices, OnChosenCommand);
                    }
                    
                }
                else
                {
                    StartCoroutine(dialoguePlayer.PlayDialogue(new [] {"Ponter: Let's move."}, CloseDialogue));
                }
            }
        }
    }

    public void OnChosenCommand(int command)
    {
        if (commandType == 1)
        {
            DialogueChoice chosen = dialogueChoices.FirstOrDefault(o => o.id == command);
            for(int i=0; i<dialogueChoices.Length; i++)
            {
                if (chosen.id == dialogueChoices[i].id)
                {
                    DialogueChoice(i);
                    break;
                }
            }
        }
        else if (commandType == 2){
            DialogueChoice chosen = storyChoices.FirstOrDefault(o => o.id == command);
            Debug.Log(chosen.text);
            for(int i=0; i<storyChoices.Length; i++)
            {
                if (chosen.id == storyChoices[i].id)
                {
                    StartCoroutine(StoryChoice(i));
                    break;
                }
            }
        }
        else if (commandType == 3)
        {
            if (command == 0)
            {
                choosePathAgain = false;
               switch (chosenPath)
                {
                    case 0:
                        WorldState.Instance.castle = true;
                        break;
                    case 1:
                        WorldState.Instance.mayhemIsland = true;
                        break;
                    case 2:
                        WorldState.Instance.blossomGarden = true;
                        break;  
                    case 3:
                        WorldState.Instance.roggenfall = true;
                        break;
                    default:
                        break;       
                }
                dialoguePlayer.dialogueEnded = true;  
            }
            else if (command == 1)
            {
                choosePathAgain = true;
                dialoguePlayer.dialogueEnded = true;
                StopAllCoroutines();
                StartCoroutine(InfoToChoice());
            }
        }
        
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        initInteraction.interactionActive = false;
    }

    public IEnumerator InfoToChoice()
    {
        commandType = 2;
        if (!infoPlayed) {
            StartCoroutine(dialoguePlayer.PlayDialogue(informationDialogue, null));
            infoPlayed = true;
        }
        byte id = 0;
        for (int i=0; i<storyChoices.Length; i++)
        {
            storyChoices[i] = new DialogueChoice();
        }
        storyChoices[0].text = "We can sneak into the castle as workers";
        storyChoices[1].text = "The fisherman can get us to Mayhem Island";
        storyChoices[2].text = "Kilmor could help us get to Blossom Garden";
        storyChoices[3].text = "Let's get to Raggenfall";
        List<DialogueChoice> finalstoryChoices = new List<DialogueChoice>();

        command = "Ponter: So what are we going to do?";
        if (WorldState.Instance.fireFixing) {
            storyChoices[0].id = id; 
            finalstoryChoices.Add(storyChoices[0]);
            id++;
        }
        if (WorldState.Instance.fish_willHelp)
        {
            storyChoices[1].id = id;
            finalstoryChoices.Add(storyChoices[1]);
            id++;
        } 
        if (WorldState.Instance.savedChildren)
        {
            storyChoices[2].id = id;
            finalstoryChoices.Add(storyChoices[2]);
            id++;
        } 
        storyChoices[3].id = id;
        finalstoryChoices.Add(storyChoices[3]);

        while(!dialoguePlayer.dialogueEnded) yield return null;

        dialoguePlayer.PlayCommand(command, finalstoryChoices, OnChosenCommand);
    }


    public void DialogueChoice(int choice)
    {
        if (choice == 0)
        {
            StartCoroutine(InfoToChoice());
        }
        else if (choice == 1)
        {
            StartCoroutine(dialoguePlayer.PlayDialogue(jewelBladeDialogue, CloseDialogue));
            WorldState.Instance.askSellerAboutJewelBlade = true;
        }
        else if (choice == 2)
        {
            StartCoroutine(dialogueManager.HideShowPanel("hide"));
            CloseDialogue(0);
        }
    }
    private IEnumerator StoryChoice(int choice)
    {
        commandType = 3;
        chosenPath = choice;
        string[] choiceEnded = {"Ponter: It is settled, then.", "Ponter: Let's not waste any time, !<NAME>!.", "Ponter: We should move."};
        if (choice == 0)
            StartCoroutine(dialoguePlayer.PlayDialogue(castleChoice, null));
        else if (choice == 1)
            StartCoroutine(dialoguePlayer.PlayDialogue(mayhemIslandChoice, null));
        else if (choice == 2)
            StartCoroutine(dialoguePlayer.PlayDialogue(blossomGardenChoice, null));
        else if (choice == 3)
            StartCoroutine(dialoguePlayer.PlayDialogue(raggenfallChoice, null));
       
        while (!dialoguePlayer.dialogueEnded) yield return null;
        dialoguePlayer.PlayCommand("Are you sure?\n1. Yes\n2. No", 2, OnChosenCommand);
        while (!dialoguePlayer.dialogueEnded) yield return null;
        if(!choosePathAgain) {
            StartCoroutine(dialoguePlayer.PlayDialogue(choiceEnded, CloseDialogue));
            WorldState.Instance.ponterQuestEnded = true;
        }
    }
}
