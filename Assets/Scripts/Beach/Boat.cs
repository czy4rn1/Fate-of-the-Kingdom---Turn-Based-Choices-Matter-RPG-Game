using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boat : MonoBehaviour
{
    public Player player;
    public DialogueManager dialogueManager;
    public PlayerDetection playerDetection;
    public BlackoutManager blackoutManager;
    public InitInteraction initInteraction;
    void Start()
    {
        if (WorldState.Instance.currentLevel == "Volsen")
        {
            if (!WorldState.Instance.fish_killed && !WorldState.Instance.fish_questEnded) gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (WorldState.Instance.currentLevel == "Beach")
            {
                if (WorldState.Instance.fish_killed || WorldState.Instance.fish_questEnded)
                {
                    StartCoroutine(LoadTransition());
                }
                else {
                    dialogueManager.ShowDialogue("There's a boat here. It might be useful.", true, 0, true, initInteraction.CloseInteraction);
                }
            }
            else if (WorldState.Instance.currentLevel == "Volsen")
            {
                StartCoroutine(LoadTransition());
            }
        }
    }

    IEnumerator LoadTransition()
    {
        yield return StartCoroutine(blackoutManager.Fade(false));
        while (blackoutManager.curAlpha < 1f) yield return null;
        SceneManager.LoadScene("BoatTransition");
    }
}
