using UnityEngine;
using UnityEngine.Playables;

public class PonterVolsenInn : MonoBehaviour
{
    public Player player;
    public DialogueManager dialogueManager;
    public PlayableDirector ponterEntersInn;
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public string[] introLines;
    public string command;
    private bool introEnded = false;
    void Start()
    {
        if (!WorldState.Instance.keyStolen)
        {
            transform.position = new Vector2(-6.51f, -0.72f);
        }
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            player.isControllable = false;
            initInteraction.interactionActive = true;
            if (!introEnded) {
                StartCoroutine(dialoguePlayer.PlayDialogue(introLines, CloseDialogue));
                introEnded = true;
            }
        }
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        initInteraction.interactionActive = false;
    }
}
