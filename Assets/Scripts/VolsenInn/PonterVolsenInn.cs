using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PonterVolsenInn : MonoBehaviour
{
    public Player player;
    public DialogueManager dialogueManager;
    public PlayableDirector ponterEntersInn;
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public string[] introLines;
    private byte commandType = 0;
    public string[] jewelBladeDialogue;
    public string[] informationDialogue;
    private string command;
    private string[] roggenfallChoice = {"Ponter: It is settled, then.", "Ponter: Let's not waste any time, !<NAME>!.", "Ponter: We should move."};
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
                    else if (!WorldState.Instance.ponterInfoObtained && WorldState.Instance.jewelBladeObtained)
                    {
                        commandType = 1;
                        command = "Ponter: Do you want to tell me something?\n" + 
                        "1. Tell him about the Jewel Blade\n" + 
                        "2. Nothing";
                        dialoguePlayer.PlayCommand(command, 2, OnChosenCommand);
                    }
                    else if (WorldState.Instance.ponterInfoObtained && !WorldState.Instance.jewelBladeObtained)
                    {
                        commandType = 2;
                        command = "Ponter: Do you want to tell me something?\n" + 
                        "1. Give him the information you've obtained\n" + 
                        "2. Nothing";
                        dialoguePlayer.PlayCommand(command, 2, OnChosenCommand);
                    }
                    else if (WorldState.Instance.ponterInfoObtained && WorldState.Instance.jewelBladeObtained)
                    {
                        commandType = 3;
                        command = "Ponter: Do you want to tell me something?\n" + 
                        "1. Give him the information you've obtained\n" + 
                        "2. Tell him about the Jewel Blade\n" +
                        "3. Nothing";
                        dialoguePlayer.PlayCommand(command, 3, OnChosenCommand);
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
            if (command == 0)
            {
                StartCoroutine(dialoguePlayer.PlayDialogue(jewelBladeDialogue, CloseDialogue));
                WorldState.Instance.askSellerAboutJewelBlade = true;
            }
            else if (command == 1)  {
                StartCoroutine(dialogueManager.HideShowPanel("hide"));
                CloseDialogue(0);
            }
        }
        else if (commandType == 2)
        {
            if (command == 0)
            {
                StartCoroutine(InfoToChoice());
            }
            else if (command == 1) {
                StartCoroutine(dialogueManager.HideShowPanel("hide"));
                CloseDialogue(0);
            }
        }
        else if (commandType == 3)
        {
            if (command == 0)
            {
                StartCoroutine(InfoToChoice());
            }
            else if (command == 1)
            {
                StartCoroutine(dialoguePlayer.PlayDialogue(jewelBladeDialogue, CloseDialogue));
                WorldState.Instance.askSellerAboutJewelBlade = true;
            }
            else if (command == 2) {
                StartCoroutine(dialogueManager.HideShowPanel("hide"));
                CloseDialogue(0);
            }
        }
        else if (commandType == 4)
        {
            StoryChoice(1);
        }
        else if (commandType == 5)
        {
            if (command == 0)
            {
                StoryChoice(2);
            }
            else if (command == 1)
            {
                StoryChoice(1);
            }
        }
        else if (commandType == 6)
        {
            if (command == 0)
            {
                StoryChoice(4);
            }
            else if (command == 1)
            {
                StoryChoice(1);
            }
        }
        else if (commandType == 7)
        {
            if (command == 0)
            {
                StoryChoice(3);
            }
            else if(command == 1)
            {
                StoryChoice(4);
            }
            else if (command == 2)
            {
                StoryChoice(1);
            }
        }
        else if (commandType == 8)
        {
            if (command == 0)
            {
                StoryChoice(3);
            }
            else if (command == 1)
            {
                StoryChoice(1);
            }
        }
        //commandType = 0;
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        initInteraction.interactionActive = false;
    }

    public IEnumerator InfoToChoice()
    {
        StartCoroutine(dialoguePlayer.PlayDialogue(informationDialogue, null));
        byte numOfCommands = 1;
        command = "Ponter: So what are we going to do?";
        if (WorldState.Instance.fireFixing) {
            command += $"\n{numOfCommands}. " + "We can pretend to be workers"; 
            numOfCommands++;
        }
        if (WorldState.Instance.fish_willHelp)
        {
            command += $"\n{numOfCommands}. " + "The fisherman can get us to XXX"; 
            numOfCommands++;
        } 
        if (WorldState.Instance.savedChildren)
        {
            command += $"\n{numOfCommands}. " + "Kilmor could help us get to XXX"; 
            numOfCommands++;
        } 
        
        command += $"\n{numOfCommands}. " + "Let's get to Roggenfall";

        if (!WorldState.Instance.fireFixing && !WorldState.Instance.fish_willHelp && !WorldState.Instance.savedChildren) commandType = 4;
        if (WorldState.Instance.fireFixing && !WorldState.Instance.fish_willHelp && !WorldState.Instance.savedChildren) commandType = 5;
        if (!WorldState.Instance.fireFixing && !WorldState.Instance.fish_willHelp && WorldState.Instance.savedChildren) commandType = 6;
        if (!WorldState.Instance.fireFixing && WorldState.Instance.fish_willHelp && WorldState.Instance.savedChildren) commandType = 7;
        if (!WorldState.Instance.fireFixing && WorldState.Instance.fish_willHelp && !WorldState.Instance.savedChildren) commandType = 8;
        while(!dialoguePlayer.dialogueEnded) yield return null;

        dialoguePlayer.PlayCommand(command, numOfCommands, OnChosenCommand);
    }

    private void StoryChoice(byte choice)
    {
        if (choice == 1)
        {
            StartCoroutine(dialoguePlayer.PlayDialogue(roggenfallChoice, CloseDialogue));
            WorldState.Instance.roggenfall = true;
        }
        else if (choice == 2)
        {
            StartCoroutine(dialoguePlayer.PlayDialogue(roggenfallChoice, CloseDialogue));
            WorldState.Instance.castle = true;
        }
        else if (choice == 3)
        {
            StartCoroutine(dialoguePlayer.PlayDialogue(roggenfallChoice, CloseDialogue));
            WorldState.Instance.flower = true;
        }
        else if (choice == 4)
        {
            StartCoroutine(dialoguePlayer.PlayDialogue(roggenfallChoice, CloseDialogue));
            WorldState.Instance.lava = true;
        }
        WorldState.Instance.ponterQuestEnded = true;
    }
}
