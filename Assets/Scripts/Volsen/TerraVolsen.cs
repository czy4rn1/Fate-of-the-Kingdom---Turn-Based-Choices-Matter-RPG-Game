using System.Collections;
using UnityEngine;

public class TerraVolsen : MonoBehaviour
{
    public PlayDialogueLines dialoguePlayer;
    public InitInteraction initInteraction;
    public PlayerDetection playerDetection;
    public string[] ponterQuestLines;
    private bool botheringTerra = false;
    void Start()
    {
        if (WorldState.Instance.ponter_state < (byte)WorldState.Ponter.QuestStarted ||
            WorldState.Instance.ponter_state > (byte)WorldState.Ponter.QuestEnded) Destroy(gameObject);
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(Dialogue());
        }
    }

    IEnumerator Dialogue()
    {
        if (WorldState.Instance.ponter_state < (byte)WorldState.Ponter.InfoObtained)
            {
                if (WorldState.Instance.ponter_state >= (byte)WorldState.Ponter.InfoFromForgan)
                {
                    yield return StartCoroutine(dialoguePlayer.PlayDialogue(ponterQuestLines, initInteraction.CloseInteraction));
                    WorldState.Instance.ponter_state = (byte)WorldState.Ponter.InfoObtained;
                }
                else
                {
                    if (!botheringTerra) {
                        string[] lines = {"Terra: The hell you want from me, kid?",
                        "!<NAME>!: I'm looking for information about...",
                        "Terra: Stop bothering me, you parentless troglodite!",
                        "Terra: You come here again with a nothing business and I'll feed crocodiles with you!",
                        "Terra: You hear me?!",
                        "!<NAME>!: I'm sorry..."};
                        yield return StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
                        botheringTerra = true;
                    } else
                    {
                        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new[] {"Terra: Stop bothering me!"}, initInteraction.CloseInteraction));
                    }
                }
            }
            else 
            {
                string[] lines = {"Terra: Don't forget to return it, you little rascal!",
                "Terra: And come back if you want something else to read!"};
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
            }
    }
}
