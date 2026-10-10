using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;

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
    public MoveToCutscene moveToCutscene;
    bool retry = false;
    SpriteRenderer sr;

    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        if (initInteraction.enabled && initInteraction.Interaction())
        {
            StartCoroutine(Dialogue());
        }
    }

    IEnumerator Dialogue()
    {
        if (!WorldState.Instance.ponterDead && !retry) yield return StartCoroutine(moveToCutscene.MoveForDialogue(false, true));
        retry = false;
        if (!WorldState.Instance.gardenerIntroPlayed) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(
            WorldState.Instance.gardenerSummoned ? introSummoned : introNotSummoned));
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introGeneral));
            WorldState.Instance.gardenerIntroPlayed = true;
        }
        
        if (!WorldState.Instance.gardenerChoiceMade) {
            SetUpChoices();
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
                if (!WorldState.Instance.ponterDead) yield return StartCoroutine(moveToCutscene.MoveForDialogue(true, true));
                initInteraction.CloseInteraction(0);
                yield break;
            }
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(lines));
            if (!WorldState.Instance.ponterDead) yield return StartCoroutine(moveToCutscene.MoveForDialogue(true, true));
            initInteraction.CloseInteraction(0);
        }
           
        
    }

    public void OnChosenCommand(int command)
    {
        DialogueChoice chosen = choices.FirstOrDefault(o => o.id == command);
        for(int i=0; i<choices.Length; i++)
        {
            if (chosen.id == choices[i].id)
            {
                StartCoroutine(StoryChoice(i));
                break;
            }
        }
        

    }

    IEnumerator StoryChoice(int i)
    {
        if (i == 4) {
            if (!WorldState.Instance.ponterDead) yield return StartCoroutine(moveToCutscene.MoveForDialogue(true, true));
            initInteraction.CloseInteraction(i);
        }
        else
        {
            bool x;
            switch(i){
            case 0:
                string[] questShowUp = new[] {questLines[0], "Gardener: The apple behind me - The Forbidden Fruit..."}.Concat(questLines[3..5]).Append("Gardener: You can take it.").Concat(questLines[7..10]).ToArray();
                x = WorldState.Instance.gardenerSummoned;
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(x ? questLines : questShowUp));
                if (!WorldState.Instance.ponterDead) yield return StartCoroutine(moveToCutscene.MoveForDialogue(true, true));
                initInteraction.CloseInteraction(i);
                WorldState.Instance.gardenerStandard = true;
                if (!x) StartCoroutine(MoveToHouse());
                break;
            case 1:
                x = PlayerData.Instance.persuasion >= reqPer;
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(x ? perSuccess : perFail));
                if (!x) {
                    retry = true;
                    StartCoroutine(Dialogue());
                    yield break;
                }
                else {
                    WorldState.Instance.gardenerJoins = true;
                    if (!WorldState.Instance.ponterDead) yield return StartCoroutine(moveToCutscene.MoveForDialogue(true, true));
                    initInteraction.CloseInteraction(i);
                }
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
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(slaveLines));
                if (!WorldState.Instance.ponterDead) yield return StartCoroutine(moveToCutscene.MoveForDialogue(true, true));
                initInteraction.CloseInteraction(i);
                WorldState.Instance.playerEnslaved = true;
                break;            
            }
            WorldState.Instance.gardenerChoiceMade = true;
        }
    }

    IEnumerator MoveToHouse()
    {
        initInteraction.EnableInteraction(false);
        sr.sortingLayerName = "UI";
        Vector2 targetPos = new Vector2(63.83f, -12.83f);
        while (Vector2.Distance(gameObject.transform.parent.transform.parent.position, targetPos) > 0.05f)
        {    
            gameObject.transform.parent.transform.parent.position = Vector2.MoveTowards(gameObject.transform.parent.transform.parent.position, targetPos, 2f * Time.deltaTime);
            yield return null;
        }
        sr.sortingLayerName = "Characters";
        initInteraction.enabled = true;
        initInteraction.EnableInteraction(true);
    }

    void SetUpChoices()
    {
        for(int i=0; i<choices.Length; i++)
        {
            choices[i] = new DialogueChoice
            {
                text = choicesText[i]
            };
        }
        choices[1].text = choices[1].text.Replace("{y}", reqPer.ToString());
        choices[2].text = choices[2].text.Replace("{w}", reqInt.ToString());
        choices[1].text = choices[1].text.Replace("{x}", PlayerData.Instance.persuasion.ToString());
        choices[2].text = choices[2].text.Replace("{z}", PlayerData.Instance.intelligence.ToString());
    }
}
