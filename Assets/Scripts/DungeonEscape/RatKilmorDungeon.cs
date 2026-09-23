using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class RatKilmorDungeon : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public Player player;
    public DialogueManager dialogueManager;
    public PlayableDirector supportCutscene;
    public PlayableDirector betrayCutscene;
    public PlayableDirector supportCutscene2;
    public GameObject[] kids = new GameObject[3];
    public InitInteraction initInteraction;
    private bool questStarted = false;
    private bool isInteractable = true;

    private bool introductionEnded = false;
    
    public string[] introDialogues;
    public string questCommands;
    public byte numOfCommands;

    void Start()
    {
        if (WorldState.Instance.savedChildren)
        {
            foreach (GameObject kid in kids) kid.SetActive(true);
        }
        if (WorldState.Instance.kilmor_questEnded) questStarted = true;
        introductionEnded = WorldState.Instance.kilmor_intro_ended;
    }
    void Update()
    {
        if (isInteractable) {
            if (initInteraction.Interaction())
            {
                if (!introductionEnded && !WorldState.Instance.kilmor_questStarted)
                {
                    StartCoroutine(PlayDialogue(introDialogues));
                }
                else if (introductionEnded && (!WorldState.Instance.kilmor_questStarted || !questStarted))
                {
                    dialogueManager.ShowDialogue(questCommands, false, numOfCommands, false, OnCommandSelected);
                }
                else if (introductionEnded && 
                WorldState.Instance.kilmor_questStarted &&
                questStarted && 
                !WorldState.Instance.attackedKilmor && 
                !WorldState.Instance.kilmor_questEnded)
                {
                    if (!WorldState.Instance.secretPathOpened) {
                        string[] dialogueLines = {"Rat Kilmor: I think this is the place. I can still smell them.",
                        "Rat Kilmor: However I don't see any pathways that could lead us any further."};
                        StartCoroutine(PlayDialogue(dialogueLines));
                    }
                    else
                    {
                        string[] dialogueLines = {"Rat Kilmor: !<NAME>!, let's go! We have to save them!"};
                        StartCoroutine(PlayDialogueSupportKilmor(dialogueLines, false));
                    }
                }
                else if (introductionEnded && WorldState.Instance.savedChildren)
                {
                    if (!WorldState.Instance.blossomGarden)
                    {
                        StartCoroutine(PlayDialogue(new string[] {"Rat Kilmor: Thank you so much for saving my children. Come back if you need help from me."}));
                    }
                    else
                    {
                        
                    }
                }
            }
        }
        else {playerDetection.allowIcon = false;}
    }

    public void OnCommandSelected(int command)
    {
        if (command == 0)
        {
           WorldState.Instance.kilmor_questStarted = true;
           questStarted = true; 
           string[] dialogues = {"Rat Kilmor: Thank you so much, !<NAME>!!", "Rat Kilmor: All right, follow me."};
           StartCoroutine(PlayDialogueSupportKilmor(dialogues, true));
        }
        else if (command == 1)
        {
            WorldState.Instance.kilmor_questStarted = true;
            WorldState.Instance.attackedKilmor = true; 
            string[] dialogues = {"Rat Kilmor: What? What are you talking about?", 
            "!<NAME>!: You're standing in my way, and I don't care about your children.",
            "!<NAME>!: However I feel like you might be in posession of items I might find useful in my crusade...",
            "Rat Kilmor: You monster! I will not go easy on you!"};
            StartCoroutine(PlayDialogueBetrayKilmor(dialogues));
        }
        else if (command == 2)
        {
            dialogueManager.ShowDialogue("Rat Kilmor: Please come back, if you change your mind. I really need your help. I can't live without my kids.", 
            true, 0, true, initInteraction.CloseInteraction);
        }
    }

    public IEnumerator PlayDialogue(string[] dialogueLines)
    {
        for(int i=0; i<dialogueLines.Length; i++)
            {
                bool last = false;
                if (i == dialogueLines.Length-1) last = true;
                dialogueManager.ShowDialogue(dialogueLines[i], true, 0, last, last ? initInteraction.CloseInteraction : null);
                yield return null;
                while (!dialogueManager.isWaitingForPlayer) yield return null;
                while(dialogueManager.isWaitingForPlayer) yield return null; 
            }    
        introductionEnded = true;
        WorldState.Instance.kilmor_intro_ended = true;
    }

    public IEnumerator SupportKilmor(bool x)
    {
        if (x) {
            if (supportCutscene != null) {
                isInteractable = false;
                playerDetection.allowIcon = false;
                supportCutscene.Play();
                while(supportCutscene.state == PlayState.Playing) yield return null;
                supportCutscene = null; 
                isInteractable = true;
            }
        }
        else {
            if (supportCutscene2 != null)
            {
                    playerDetection.allowIcon = false;
                    supportCutscene2.Play();
                    while(supportCutscene2.state == PlayState.Playing) yield return null;
                    supportCutscene2 = null; 
            }
        }
    }

    public IEnumerator PlayDialogueSupportKilmor(string[] dialogueLines, bool firstCutscene)
    {
        yield return StartCoroutine(PlayDialogue(dialogueLines));
        yield return StartCoroutine(SupportKilmor(firstCutscene));
    }

    public IEnumerator PlayDialogueBetrayKilmor(string[] dialogueLines)
    {
        yield return StartCoroutine(PlayDialogue(dialogueLines));
        player.isControllable = false;
        if (betrayCutscene != null)
        {
            dialogueManager.timelineDirector = betrayCutscene;
            betrayCutscene.Play();
            while (betrayCutscene.state == PlayState.Playing) yield return null;
            betrayCutscene = null;
            player.isControllable = true;
        }
    }

}
