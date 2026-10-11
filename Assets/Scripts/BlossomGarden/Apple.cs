using System.Collections;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public Portal applePortal;
    public Portal shadowPortal;
    void Start()
    {
        if (initInteraction == null) initInteraction = GetComponent<InitInteraction>();
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            dialoguePlayer.PlayCommand("Pick up the apple?\n1. Yes\n2. No", 2, OnChosenCommand);
        }
    }

    void OnChosenCommand(int command)
    {
        if (command == 1) initInteraction.CloseInteraction(command);
        else
        {
            StartCoroutine(StoryDialogue());
        }
    }

    IEnumerator StoryDialogue()
    {
        initInteraction.EnableInteraction(false);
        shadowPortal.visible = false;
        applePortal.visible = false;
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: We've got the seal... We finally have a real chance to win!", 
        "!<NAME>!: Let's get back to Volsen!"}, initInteraction.CloseInteraction));
        WorldState.Instance.appleTaken = true;
        yield return null;
        gameObject.SetActive(false);
    }
}
