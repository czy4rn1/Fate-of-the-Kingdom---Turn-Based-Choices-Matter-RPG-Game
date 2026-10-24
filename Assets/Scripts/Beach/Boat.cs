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
    public SceneLoader sceneLoader;
    void Start()
    {
        if (WorldState.Instance.currentLevel == "Volsen")
        {
            sceneLoader = GetComponent<SceneLoader>();
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
                    dialoguePlayer.PlayCommand("Fisherman: Where to, my friend?\n1. Go to Mayhem Island\n2. Go to the Beach\n3. Leave", 3, OnChosenCommand);
                }
                else StartCoroutine(LoadTransition());
            }
            else if (WorldState.Instance.currentLevel == "MayhemIsland") StartCoroutine(sceneLoader.LoadArea());
        }
    }

    void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            if (WorldState.Instance.mayhem_state <= (byte)WorldState.Mayhem.NotEntered)
            {
                StartCoroutine(MayhemIsland());
            }
            else StartCoroutine(sceneLoader.LoadArea());
        }
        else if (command == 1) StartCoroutine(LoadTransition());
        else initInteraction.CloseInteraction(command);
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
        yield return StartCoroutine(sceneLoader.LoadArea());
    }
}
