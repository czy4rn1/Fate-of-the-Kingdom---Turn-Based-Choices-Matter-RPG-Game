using UnityEngine;

public class Gardener : MonoBehaviour
{

    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public string[] introNotSummoned;
    public string[] introSummoned;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (WorldState.Instance.gardenerSummoned)
            {
                if (!WorldState.Instance.gardenerIntroPlayed)
                {
                    StartCoroutine(dialoguePlayer.PlayDialogue(introSummoned, initInteraction.CloseInteraction));
                }
                else
                {
                    
                }
            }
            else
            {
                if (!WorldState.Instance.gardenerIntroPlayed)
                {
                    StartCoroutine(dialoguePlayer.PlayDialogue(introSummoned, initInteraction.CloseInteraction));
                }
                else
                {
                    
                }
            }
        }
    }
}
