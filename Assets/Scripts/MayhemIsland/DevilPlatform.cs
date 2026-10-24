using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DevilPlatform : MonoBehaviour
{
    public PlayableDirector cutscene;
    public bool allVisible = false;
    public InitInteraction initInteraction;
    public DialogueManager dialogueManager;
    public BlackoutManager blackoutManager;

    void Start()
    {
        if (WorldState.Instance.mayhem_state >= (byte)WorldState.Mayhem.Ended) allVisible = true;
        else allVisible = false;
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (!allVisible) {
                initInteraction.CloseInteraction(0);
                return;
            }
            dialogueManager.ShowDialogue("Do you wish to enter Ahorgis?\n1. Yes\n2. No", false, 2, true, OnChosenCommand);
        }
    }

    public void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            StartCoroutine(LoadNextArea());
        }
        else if (command == 1)
        {
            initInteraction.CloseInteraction(0);
        }
    }

    IEnumerator LoadNextArea()
    {
        yield return StartCoroutine(blackoutManager.Fade(false));
        cutscene.Play();
        StartCoroutine(blackoutManager.Fade(true));
        while (cutscene.state == PlayState.Playing) yield return null;
        yield return StartCoroutine(blackoutManager.Fade(false));
        SceneManager.LoadScene("Ahorgis");
    }

}
