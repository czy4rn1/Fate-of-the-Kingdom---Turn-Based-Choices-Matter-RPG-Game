using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class VenardVolsen : MonoBehaviour
{
    public Player player;
    public PlayerDetection playerDetection;
    public PlayDialogueLines dialoguePlayer;
    private bool interactionActive = false;
    public string[] introLines;
    public string[] standardLines;
    public string commands;
    public string[] agreeLines;
    public string[] disagreeLines;
    public PlayableDirector playableDirector;
    private bool interactionEnded = false;
    void Start()
    {
        if (WorldState.Instance.venardKilled) gameObject.SetActive(false);
        if (WorldState.Instance.venardRemorse) ChangeStandardLines();
    }

    
    void Update()
    {
        if (!interactionActive)
        {
            playerDetection.allowIcon = true;
            if (playerDetection.isPlayerNearby &&
            player.isControllable &&
            !dialoguePlayer.dialogueManager.dialogueActive &&
            Input.GetKeyDown(KeyCode.F))
            {
                player.isControllable = false;
                interactionActive = true;
                if (!interactionEnded && !WorldState.Instance.venardEncounterEnded) {
                    StartCoroutine(Interaction());
                }
                else StartCoroutine(dialoguePlayer.PlayDialogue(standardLines, CloseDialogue));
            }
        }
        else playerDetection.allowIcon = false;
    }

    public void OnCommandSelected(int command)
    {
        switch (command)
        {
            case 0:
                StartCoroutine(dialoguePlayer.PlayDialogue(agreeLines, CloseDialogue));
                break;
            case 1:
                WorldState.Instance.venardRemorse = true;
                StartCoroutine(dialoguePlayer.PlayDialogue(disagreeLines, CloseDialogue));
                ChangeStandardLines();
                break;
            case 2:
                WorldState.Instance.venardKilled = true;
                if (playableDirector != null) playableDirector.Play();
                break;  
            default: break;          
        }
        interactionEnded = true;
        WorldState.Instance.venardEncounterEnded = true;
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        interactionActive = false;
    }

    public IEnumerator Interaction()
    {
        StartCoroutine(dialoguePlayer.PlayDialogue(introLines, null));
        while (!dialoguePlayer.dialogueEnded) yield return null;
        dialoguePlayer.PlayCommand(commands, 3, OnCommandSelected);
    }

    private void ChangeStandardLines()
    {
        if (standardLines[0] != null && standardLines[1] != null) {
            standardLines[0] = "Venard: Don't talk to me.";
            standardLines[1] = "Venard: Leave me alone, please.";
        }
    }
}
