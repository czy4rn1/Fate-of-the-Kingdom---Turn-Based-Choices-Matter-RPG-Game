using UnityEngine;

public class ForganVolsen : MonoBehaviour
{
    public TalkingNPC talkingNPC;
    public PlayDialogueLines dialoguePlayer;
    public InitInteraction initInteraction;
    public PlayerDetection playerDetection;
    public string[] ponterQuestLines;

    void Start()
    {
        if (WorldState.Instance.ponterQuestStarted) talkingNPC.enabled = false;
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (WorldState.Instance.ponterQuestStarted && !WorldState.Instance.ponterQuestEnded)
            {
                if (!WorldState.Instance.ponterInfoFromForgan) {
                    StartCoroutine(dialoguePlayer.PlayDialogue(ponterQuestLines, initInteraction.CloseInteraction));
                    WorldState.Instance.ponterInfoFromForgan = true;
                }
                else
                {
                    string[] lines = {"Forgan: Good luck, !<NAME>!."};
                    StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
                }
            }
        }
    }
}
