using System.Collections;
using UnityEngine;

public class ForganVolsen : MonoBehaviour
{
    public TalkingNPC talkingNPC;
    public PlayDialogueLines dialoguePlayer;
    public InitInteraction initInteraction;
    public PlayerDetection playerDetection;
    public string[] ponterQuestLines;
    string[] lines = {"Forgan: Good luck, !<NAME>!."};
    public Enemy enemy;
    public Combat combat;
    public string[] combatLines;

    void Start()
    {
        if (WorldState.Instance.ponter_state >= (byte)WorldState.Ponter.QuestStarted) talkingNPC.enabled = false;
        if (WorldState.Instance.forganDead) Destroy(gameObject);
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
                else StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
            }
            else
            {
                if (!WorldState.Instance.playerPosessed) StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
                else StartCoroutine(Combat());
            }
        }
    }

    IEnumerator Combat()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(combatLines));
        combat.disableEscape = true;
        combat.enemies[0] = enemy;
        StartCoroutine(combat.StartBattle());
        while (!initInteraction.player.isControllable) yield return null;
        WorldState.Instance.forganDead = true;
        Destroy(gameObject);
    }
}
