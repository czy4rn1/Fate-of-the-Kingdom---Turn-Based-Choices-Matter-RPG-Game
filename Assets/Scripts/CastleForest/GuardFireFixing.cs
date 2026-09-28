using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GuardFireFixing : MonoBehaviour
{
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public PlayableDirector cutscene;
    public string[] dialogueLines;
    private bool introPlayed = false;
    public BlackoutManager blackoutManager;
    public MoveToCutscene moveToCutscene;
    
    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(FireFixing());
        }
    }

    public void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            StartCoroutine(CutsceneToNextCutscene());
        }
        else
        {
            initInteraction.CloseInteraction(0);
        }
    }

    IEnumerator FireFixing()
    {
        if (!introPlayed) yield return StartCoroutine(dialoguePlayer.PlayDialogue(dialogueLines));
        dialoguePlayer.PlayCommand("Ursus: Are you ready to go?\n1. Let's go\n2. Not yet", 2, OnChosenCommand);
    }

    IEnumerator CutsceneToNextCutscene()
    {
        yield return StartCoroutine(moveToCutscene.Move());
        while (cutscene.gameObject.activeInHierarchy) yield return null;
        yield return StartCoroutine(blackoutManager.Fade(false));
        SceneManager.LoadScene("VolsenFire");
    }
}
