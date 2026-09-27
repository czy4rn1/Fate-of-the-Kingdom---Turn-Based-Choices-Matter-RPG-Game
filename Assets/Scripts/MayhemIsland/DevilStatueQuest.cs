using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class DevilStatueQuest : MonoBehaviour
{
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public byte allItems = 4;
    public byte curItems = 0;
    public string[] introLines;
    public MoveToCutscene moveToCutscene;
    public GameObject digging;
    public BlackoutManager blackoutManager;
    public PlayableDirector cutscene;
    public string[] unlockAtlantis;
    public PlayableDirector entryCutscene;

    void Start()
    {
        if (WorldState.Instance.mayhemQuestStarted && !WorldState.Instance.mayhemQuestEnded) digging.SetActive(true);
        else digging.SetActive(false);
        dialoguePlayer.dialogueManager.timelineDirector = entryCutscene;
        entryCutscene.Play();
        StartCoroutine(blackoutManager.Fade(true));
    }

    
    void Update()
    {
        if (!initInteraction.player.isControllable) initInteraction.playerDetection.allowIcon = false;
        else initInteraction.playerDetection.allowIcon = true;
        if (initInteraction.Interaction())
        {
            StartCoroutine(InteractWithPonter());
        }
    }

    IEnumerator InteractWithPonter()
    {
        yield return StartCoroutine(moveToCutscene.MoveForDialogue(false));
        if (!WorldState.Instance.mayhemQuestStarted) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines));
            WorldState.Instance.mayhemQuestStarted = true;
            digging.SetActive(true);
        }
        else if (WorldState.Instance.mayhemQuestStarted && !WorldState.Instance.mayhemQuestEnded)
        {
            if (curItems < allItems)
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[]
                {
                    "!<NAME>!: Hmm... Nothing happened.",
                    "!<NAME>!: I suppose that's not enough."
                }));
            }
            else
            {
                WorldState.Instance.mayhemQuestEnded = true;
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[]
                {
                    "!<NAME>!: Woah! You feel it?",
                    "Ponter: Yes, I do! Something big is happening!"
                }));
                yield return StartCoroutine(blackoutManager.Fade(false));
                cutscene.Play();
                StartCoroutine(blackoutManager.Fade(true));
                while(cutscene.state == PlayState.Playing) yield return null;
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(unlockAtlantis));

            }
        }
        yield return StartCoroutine(moveToCutscene.MoveForDialogue(true));
        initInteraction.CloseInteraction(0);
    }
}
