using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class VenardVolsen : MonoBehaviour
{
    public Player player;
    public PlayerDetection playerDetection;
    public PlayDialogueLines dialoguePlayer;
    public string[] introLines;
    public string[] standardLines;
    public string commands;
    public string[] agreeLines;
    public string[] disagreeLines;
    public string[] questNoEncounter;
    public string[] questNoRemorse;
    public string[] questRemorse;
    public PlayableDirector venardKilled;
    public InitInteraction initInteraction;
    void Start()
    {
        if (WorldState.Instance.venardKilled) gameObject.SetActive(false);
        if (WorldState.Instance.venardRemorse) ChangeStandardLines();
    }

    
    void Update()
    {
        if (initInteraction.Interaction())
        {
            player.isControllable = false;
            initInteraction.interactionActive = true;
            if (!WorldState.Instance.ponterQuestStarted) {
                if (!WorldState.Instance.venardEncounterEnded) {
                    StartCoroutine(Interaction());
                }
                else StartCoroutine(dialoguePlayer.PlayDialogue(standardLines, CloseDialogue));
            }
            else
            {
                if (!WorldState.Instance.ponterQuestEnded)
                {
                    if (!WorldState.Instance.venardEncounterEnded)
                    {
                        StartCoroutine(dialoguePlayer.PlayDialogue(questNoEncounter, CloseDialogue));
                    }
                    else
                    {
                        if (!WorldState.Instance.venardRemorse)
                        {
                            StartCoroutine(dialoguePlayer.PlayDialogue(questNoRemorse, CloseDialogue));
                        }
                        else
                        {
                            StartCoroutine(dialoguePlayer.PlayDialogue(questRemorse, CloseDialogue));
                            WorldState.Instance.ponterInfoObtained = true;
                        }
                    }
                }
            }
        }
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
                if (venardKilled != null) venardKilled.Play();
                break;  
            default: break;          
        }
        initInteraction.interactionActive = false;
        WorldState.Instance.venardEncounterEnded = true;
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        initInteraction.interactionActive = false;
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
