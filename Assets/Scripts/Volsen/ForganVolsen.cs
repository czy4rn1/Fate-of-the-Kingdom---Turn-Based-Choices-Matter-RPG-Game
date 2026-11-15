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
        if (WorldState.Instance.ponter_state >= (byte)WorldState.Ponter.QuestStarted) talkingNPC.enabled = false;
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (WorldState.Instance.ponter_state >= (byte)WorldState.Ponter.QuestStarted &&
                WorldState.Instance.ponter_state < (byte)WorldState.Ponter.QuestEnded)
            {
                if (WorldState.Instance.ponter_state < (byte)WorldState.Ponter.InfoFromForgan) {
                    StartCoroutine(dialoguePlayer.PlayDialogue(ponterQuestLines, initInteraction.CloseInteraction));
                    WorldState.Instance.ponter_state = (byte)WorldState.Ponter.InfoFromForgan;
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
