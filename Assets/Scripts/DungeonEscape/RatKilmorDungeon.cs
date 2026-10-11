using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class RatKilmorDungeon : MonoBehaviour
{
    public SceneLoader LavaLoader;
    public PlayerDetection playerDetection;
    public Player player;
    public DialogueManager dialogueManager;
    public PlayableDirector supportCutscene;
    public PlayableDirector betrayCutscene;
    public PlayableDirector supportCutscene2;
    public GameObject[] kids = new GameObject[3];
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;

    
    public string[] introDialogues;
    public string questCommands;
    public string[] blossomGardenLines;
    public byte numOfCommands;

    void Start()
    {
        if (WorldState.Instance.savedChildren)
        {
            foreach (GameObject kid in kids) kid.SetActive(true);
        }
        if (WorldState.Instance.kilmor_questStarted && !WorldState.Instance.kilmor_questEnded) gameObject.transform.parent.transform.position = new Vector2(16.67f, 10.56f);
        if (WorldState.Instance.kilmor_dead || WorldState.Instance.attackedKilmor) gameObject.SetActive(false);
    }
    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (!WorldState.Instance.kilmor_intro_ended) {
                StartCoroutine(dialoguePlayer.PlayDialogue(introDialogues, initInteraction.CloseInteraction));
                WorldState.Instance.kilmor_intro_ended = true;
            }
            else
            {
                if (!WorldState.Instance.kilmor_questStarted) dialoguePlayer.PlayCommand(questCommands, 3, OnCommandSelected);
                else
                {
                    if (!WorldState.Instance.kilmor_questEnded)
                    {
                        if (!WorldState.Instance.secretPathOpened) {
                            string[] dialogueLines = {"Rat Kilmor: I think this is the place. I can still smell them.",
                            "Rat Kilmor: However I don't see any pathways that could lead us any further."};
                            StartCoroutine(dialoguePlayer.PlayDialogue(dialogueLines, initInteraction.CloseInteraction));
                        }
                        else
                        {
                            string[] dialogueLines = {"Rat Kilmor: !<NAME>!, let's go! We have to save them!"};
                            StartCoroutine(PlayDialogueSupportKilmor(dialogueLines, false));
                        }
                    }
                    else
                    {
                        if (WorldState.Instance.savedChildren)
                        {
                            if (!WorldState.Instance.blossomGarden)
                            {
                                StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Rat Kilmor: Thank you so much for saving my children. Come back if you need help from me."}, initInteraction.CloseInteraction));
                            }
                            else
                            {
                                Debug.Log("Hello");
                                StartCoroutine(PlayDialogueStartBlossom());
                            }
                        }
                        else initInteraction.CloseInteraction(0);
                    }    
                
                }
            }
        }
    }

    public void OnCommandSelected(int command)
    {
        if (command == 0)
        {
           WorldState.Instance.kilmor_questStarted = true;
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

    public IEnumerator SupportKilmor(bool x)
    {
        if (x) {
            if (supportCutscene != null) {
                initInteraction.EnableInteraction(false);
                supportCutscene.Play();
                while(supportCutscene.state == PlayState.Playing) yield return null;
                supportCutscene = null; 
                initInteraction.enabled = true;
                initInteraction.EnableInteraction(true);
            }
        }
        else {
            if (supportCutscene2 != null)
            {
                    initInteraction.EnableInteraction(false);
                    supportCutscene2.Play();
                    while(supportCutscene2.state == PlayState.Playing) yield return null;
                    supportCutscene2 = null;
                    initInteraction.enabled = true;
                    initInteraction.EnableInteraction(true); 
            }
        }
    }

    public IEnumerator PlayDialogueSupportKilmor(string[] dialogueLines, bool firstCutscene)
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(dialogueLines, initInteraction.CloseInteraction));
        yield return StartCoroutine(SupportKilmor(firstCutscene));
    }

    public IEnumerator PlayDialogueBetrayKilmor(string[] dialogueLines)
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(dialogueLines, initInteraction.CloseInteraction));
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

    public IEnumerator PlayDialogueStartBlossom()
    {
        if (!WorldState.Instance.lavaEntryCutscenePlayed) yield return StartCoroutine(dialoguePlayer.PlayDialogue(blossomGardenLines));
        playerDetection.allowIcon = false;
        player.isControllable = false;
        yield return StartCoroutine(LavaLoader.LoadArea());
    }

}
