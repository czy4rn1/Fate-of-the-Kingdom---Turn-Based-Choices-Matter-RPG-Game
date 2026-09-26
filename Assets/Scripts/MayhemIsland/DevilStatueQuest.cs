using System.Collections;
using UnityEngine;

public class DevilStatueQuest : MonoBehaviour
{
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    private byte allItems = 5;
    public byte curItems = 0;
    public string[] introLines;
    public MoveToCutscene moveToCutscene;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(InteractWithPonter());
        }
    }

    IEnumerator InteractWithPonter()
    {
        yield return StartCoroutine(moveToCutscene.MoveForDialogue(false));
        if (!WorldState.Instance.mayhemQuestStarted) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines, initInteraction.CloseInteraction));
            WorldState.Instance.mayhemQuestStarted = true;
        }
        else
        {
            if (curItems < allItems)
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[]
                {
                    "!<NAME>!: Hmm... Nothing happened.",
                    "!<NAME>!: I suppose that's not enough."
                }, initInteraction.CloseInteraction));
            }
        }
        yield return StartCoroutine(moveToCutscene.MoveForDialogue(true));
    }
}
