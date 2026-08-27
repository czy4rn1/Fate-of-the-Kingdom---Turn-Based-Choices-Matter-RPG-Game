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
        if (!WorldState.Instance.ponterQuestStarted || WorldState.Instance.ponterQuestEnded) gameObject.SetActive(false);
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (!WorldState.Instance.ponterInfoObtained)
            {
                if (WorldState.Instance.ponterInfoFromForgan)
                {
                    StartCoroutine(dialoguePlayer.PlayDialogue(ponterQuestLines, initInteraction.CloseInteraction));
                    WorldState.Instance.ponterInfoObtained = true;
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
                        StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
                        botheringTerra = true;
                    } else
                    {
                        StartCoroutine(dialoguePlayer.PlayDialogue(new[] {"Terra: Stop bothering me!"}, initInteraction.CloseInteraction));
                    }
                }
            }
            else 
            {
                string[] lines = {"Terra: Don't forget to return it, you little rascal!",
                "Terra: And come back if you want something else to read!"};
                StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
            }
        }
    }
}
