using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class PentagramFlower : MonoBehaviour
{
    public PlayableDirector moveFlower;
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public BlackoutManager blackoutManager;

    void Update()
    {
        if (initInteraction.Interaction())
        {
            dialoguePlayer.PlayCommand("There is a vase with flowers on the way. Move it?\n1. Yes\n2. No", 2, OnChosenCommand);
        }
    }

    public void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            StartCoroutine(commandToCutscene());
        }
        else initInteraction.CloseInteraction(0);
    }

    IEnumerator commandToCutscene()
    {
        yield return StartCoroutine(blackoutManager.Fade(false));
        moveFlower.Play();
        yield return StartCoroutine(blackoutManager.Fade(true));
        initInteraction.enabled = false;
    }
}
