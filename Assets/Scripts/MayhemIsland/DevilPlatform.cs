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
        allVisible = false;
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (!allVisible) return;
            dialogueManager.ShowDialogue("Do you wish to enter Atlantis?\n1. Yes\n2. No", false, 2, true, OnChosenCommand);
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
        SceneManager.LoadScene("Atlantis");
    }

}
