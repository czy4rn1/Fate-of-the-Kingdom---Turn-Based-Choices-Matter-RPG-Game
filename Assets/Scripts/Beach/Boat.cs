using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Boat : MonoBehaviour
{
    public Player player;
    public DialogueManager dialogueManager;
    public PlayerDetection playerDetection;
    public BlackoutManager blackoutManager;
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public string[] mayhemIslandLines;
    public GameObject fisherman;
    public PlayableDirector moveCamera;
    void Start()
    {
        if (WorldState.Instance.currentLevel == "Volsen")
        {
            if (!WorldState.Instance.fish_killed && !WorldState.Instance.fish_questEnded) gameObject.SetActive(false);
            if (fisherman != null) if (WorldState.Instance.fish_killed) fisherman.SetActive(false);
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
                    StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"There's a boat here. It might be useful."}, initInteraction.CloseInteraction));
                }
            }
            else if (WorldState.Instance.currentLevel == "Volsen")
            {
                if (WorldState.Instance.mayhemIsland && WorldState.Instance.fish_willHelp)
                {
                    StartCoroutine(MayhemIsland());
                }
                else StartCoroutine(LoadTransition());
            }
        }
    }

    IEnumerator LoadTransition()
    {
        yield return StartCoroutine(blackoutManager.Fade(false));
        while (blackoutManager.curAlpha < 1f) yield return null;
        SceneManager.LoadScene("BoatTransition");
    }

    IEnumerator MayhemIsland()
    {
        yield return StartCoroutine(blackoutManager.Fade(false));
        if (moveCamera != null) moveCamera.Play();
        yield return StartCoroutine(blackoutManager.Fade(true));
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(mayhemIslandLines));
        yield return StartCoroutine(blackoutManager.Fade(false));
        SceneManager.LoadScene("MayhemIsland");
    }
}
