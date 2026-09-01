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
    public string[] questNoEncounterNoRemorse;
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
            if (!WorldState.Instance.ponterQuestStarted) {
                if (!WorldState.Instance.venardEncounterEnded) {
                    StartCoroutine(Interaction());
                }
                else StartCoroutine(dialoguePlayer.PlayDialogue(standardLines, initInteraction.CloseInteraction));
            }
            else
            {
                if (!WorldState.Instance.ponterQuestEnded)
                {
                    if (!WorldState.Instance.venardEncounterEnded)
                    {
                        StartCoroutine(dialoguePlayer.PlayDialogue(questNoEncounterNoRemorse, initInteraction.CloseInteraction));
                    }
                    else
                    {
                        if (!WorldState.Instance.venardRemorse)
                        {
                            StartCoroutine(dialoguePlayer.PlayDialogue(questNoEncounterNoRemorse, initInteraction.CloseInteraction));
                        }
                        else
                        {
                            StartCoroutine(dialoguePlayer.PlayDialogue(questRemorse, initInteraction.CloseInteraction));

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
                StartCoroutine(dialoguePlayer.PlayDialogue(agreeLines, initInteraction.CloseInteraction));
                break;
            case 1:
                WorldState.Instance.venardRemorse = true;
                StartCoroutine(dialoguePlayer.PlayDialogue(disagreeLines, initInteraction.CloseInteraction));
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
