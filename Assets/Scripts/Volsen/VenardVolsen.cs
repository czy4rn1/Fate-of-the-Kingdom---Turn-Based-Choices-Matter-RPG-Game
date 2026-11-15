using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class VenardVolsen : MonoBehaviour
{
    public Player player;
    public PlayDialogueLines dialoguePlayer;
    public string[] introLines;
    public string[] standardLines;
    public string commands;
    public string[] agreeLines;
    public string[] disagreeLines;
    public string[] questNoEncounterNoRemorse;
    public string[] questRemorse;
    public InitInteraction initInteraction;
    void Start()
    {
        if (WorldState.Instance.ponter_state >= (byte)WorldState.Ponter.QuestEnded) Destroy(gameObject);
        if (WorldState.Instance.venardRemorse) ChangeStandardLines();
    }

    
    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (WorldState.Instance.ponter_state < (byte)WorldState.Ponter.QuestStarted) {
                if (!WorldState.Instance.venardEncounterEnded && !WorldState.Instance.venardIntro) {
                    StartCoroutine(Interaction());
                    WorldState.Instance.venardIntro = true;
                }
                else if (!WorldState.Instance.venardEncounterEnded && WorldState.Instance.venardIntro){
                    dialoguePlayer.PlayCommand(commands, 3, OnCommandSelected);
                }
                else StartCoroutine(dialoguePlayer.PlayDialogue(standardLines, initInteraction.CloseInteraction));
            }
            else
            {
                if (WorldState.Instance.ponter_state < (byte)WorldState.Ponter.QuestEnded)
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
                WorldState.Instance.venardEncounterEnded = true;
                break;
            case 1:
                WorldState.Instance.venardRemorse = true;
                StartCoroutine(dialoguePlayer.PlayDialogue(disagreeLines, initInteraction.CloseInteraction));
                ChangeStandardLines();
                WorldState.Instance.venardEncounterEnded = true;
                break;
            case 2:
                initInteraction.CloseInteraction(0);
                break;
            default: break;          
        }
    }

    public IEnumerator Interaction()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines, null));
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
