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
    public string commandEntry;
    private byte commandType = 0;
    public string[] jewelBladeDialogue;
    public string[] informationDialogue;
    private string command;
    void Start()
    {
        if (!WorldState.Instance.keyStolen)
        {
            transform.position = new Vector2(-6.51f, -0.72f);
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
                StartCoroutine(dialoguePlayer.PlayDialogue(informationDialogue, CloseDialogue));
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
                StartCoroutine(dialoguePlayer.PlayDialogue(informationDialogue, CloseDialogue));
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
        commandType = 0;
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        initInteraction.interactionActive = false;
    }
}
