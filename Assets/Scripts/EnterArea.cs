using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterArea : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Player player;
    public PlayerDetection playerDetection;
    private bool interactionActive = false;
    public string sceneName;
    public BlackoutManager blackoutManager;
    public bool enterLeave;

    
    void Update()
    {
        if (interactionActive)
        {
            playerDetection.allowIcon = false;
        }
        else {playerDetection.allowIcon = true;}
        if (!interactionActive)
        {
            if (!dialogueManager.dialogueActive &&
            player.isControllable && 
            playerDetection.isPlayerNearby && 
            Input.GetKeyDown(KeyCode.F))
            {
                interactionActive = true;
                player.isControllable = false;
                string enterLeaveText = enterLeave ? "enter" : "leave";
                dialogueManager.ShowDialogue("Do you want to " + enterLeaveText + " the area?\n1. Yes\n2. No", false, 2, true, OnChosenCommand);
            }
        }
    }

    public void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            StartCoroutine(LoadArea());
        }
        else if (command == 1)
        {
            CloseDialogue(0);
        }
    }
    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        interactionActive = false;
    }

    public IEnumerator LoadArea()
    {
        yield return blackoutManager.Fade(false);
        while (blackoutManager.curAlpha < 1f) yield return null;
        SceneManager.LoadScene(sceneName);
    }
}
