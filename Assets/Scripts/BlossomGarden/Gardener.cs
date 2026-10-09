using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class Gardener : MonoBehaviour
{

    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public string[] introNotSummoned;
    public string[] introSummoned;
    public string[] introGeneral;
    public string[] choicesText = new string[5];
    private DialogueChoice[] choices = new DialogueChoice[5];
    public PlayableDirector intSuccess;
    public PlayableDirector intFail;
    byte reqInt = 40;
    byte reqPer = 30;
    public string[] perSuccess;
    public string[] perFail;
    public string[] questLines;
    public string[] slaveLines;

    
    void Start()
    {
        if (!WorldState.Instance.gardenerChoiceMade)
        {
            for(int i=0; i<choices.Length; i++)
            {
                choices[i] = new DialogueChoice
                {
                    text = choicesText[i]
                };
            }
            
        }
    }

    
    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(Dialogue());
        }
    }

    IEnumerator Dialogue()
    {
        if (!WorldState.Instance.gardenerIntroPlayed) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(
            WorldState.Instance.gardenerSummoned ? introSummoned : introNotSummoned));
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introGeneral));
            WorldState.Instance.gardenerIntroPlayed = true;
        }
        
        if (!WorldState.Instance.gardenerChoiceMade) {
            byte id = 0;
            List<DialogueChoice> finalChoices = new List<DialogueChoice>();
            choices[0].id = id++;
            finalChoices.Add(choices[0]);
            choices[1].id = id++;
            finalChoices.Add(choices[1]);
            if (WorldState.Instance.gardenerSummoned) {
                choices[2].id = id++;
                finalChoices.Add(choices[2]);
            }
            choices[3].id = id++;
            finalChoices.Add(choices[3]);
            choices[4].id = id++;
            finalChoices.Add(choices[4]);
            dialoguePlayer.PlayCommand(introGeneral[introGeneral.Length-1], finalChoices, OnChosenCommand); 
        }
        else
        {
            string[] lines = new string[2];
            if (WorldState.Instance.gardenerStandard) {
                lines[0] = "Gardener: I admire your determination, but this is all I can do for you.";
                lines[1] = "Gardener: You better leave, before I change my mind and trap your soul in Hell.";
                
            }
            else if (WorldState.Instance.gardenerJoins)
            {
                lines[0] = "Gardener: I'll stay here, while I can.";
                lines[1] = "Gardener: I'll be there, when you need me. Be sure of that.";
            }
            else if (WorldState.Instance.playerEnslaved)
            { 
                lines[0] = "Gardener: Go forth, my subordinate.";
                if (WorldState.Instance.ponterDead) lines[1] = "Gardener: Unless you wish your fate to be worse than your friend's...";
                else lines[1] = "Gardener: Do your bidding, and we'll achieve great things together!";
            }
            else {
                initInteraction.CloseInteraction(0);
                yield break;
            }
            StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
        }
           
        
    }

    public void OnChosenCommand(int command)
    {
        DialogueChoice chosen = choices.FirstOrDefault(o => o.id == command);
        for(int i=0; i<choices.Length; i++)
        {
            if (chosen.id == choices[i].id)
            {
                StoryChoice(i);
                break;
            }
        }
        

    }

    void StoryChoice(int i)
    {
        if (i == 4) initInteraction.CloseInteraction(i);
        else
        {
            bool x;
            switch(i){
            case 0:
                StartCoroutine(dialoguePlayer.PlayDialogue(questLines, initInteraction.CloseInteraction));
                WorldState.Instance.gardenerStandard = true;
                break;
            case 1:
                x = PlayerData.Instance.persuasion >= reqPer;
                StartCoroutine(dialoguePlayer.PlayDialogue(x ? perSuccess : perFail, initInteraction.CloseInteraction));
                if (!x) return;
                else WorldState.Instance.gardenerJoins = true;
                break;
            case 2:
                x = PlayerData.Instance.intelligence >= reqInt;
                dialoguePlayer.dialogueManager.timelineDirector = x ? intSuccess : intFail;
                dialoguePlayer.dialogueManager.timelineDirector.Play();
                if (!x) {
                    WorldState.Instance.ponterDead = true;
                    WorldState.Instance.playerEnslaved = true;
                }
                break;
            case 3:
                StartCoroutine(dialoguePlayer.PlayDialogue(slaveLines, initInteraction.CloseInteraction));
                WorldState.Instance.playerEnslaved = true;
                break;            
            }
            WorldState.Instance.gardenerChoiceMade = true;
        }
    }
}
