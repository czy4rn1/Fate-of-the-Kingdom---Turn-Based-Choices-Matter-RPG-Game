using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boat : MonoBehaviour
{
    public string location;
    public Player player;
    public DialogueManager dialogueManager;
    private bool isInteractable = true;
    public PlayerDetection playerDetection;
    public BlackoutManager blackoutManager;
    void Update()
    {
        if (!isInteractable) {
            playerDetection.allowIcon = false;
            return;
        }
        else playerDetection.allowIcon = true;
        if (isInteractable && 
        player.isControllable && 
        playerDetection.isPlayerNearby &&
        Input.GetKeyDown(KeyCode.F))
        {
            isInteractable = false;
            player.isControllable = false;
            if (location == "Beach")
            {
                if (WorldState.Instance.fish_killed || WorldState.Instance.fish_questEnded)
                {
                    StartCoroutine(LoadTransition());
                }
                else {
                    dialogueManager.ShowDialogue("There's a boat here. It might be useful.", true, 0, true, CloseDialogue);
                }
            }
            else if (location == "Volsen")
            {
                
            }
        }
    }

    IEnumerator LoadTransition()
    {
        yield return StartCoroutine(blackoutManager.Fade(false));
        while (blackoutManager.curAlpha < 1f) yield return null;
        SceneManager.LoadScene("BoatTransition");
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        StartCoroutine(SetInteractive());
    }

    IEnumerator SetInteractive()
    {
        yield return new WaitForSeconds(1f);
        isInteractable = true;
    }
}
