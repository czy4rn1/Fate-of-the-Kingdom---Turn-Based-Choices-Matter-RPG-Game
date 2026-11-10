using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ChoiceStolenKey : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Player player;
    bool choiceEnded = false;
    public byte req_str = 20;
    public byte req_dex = 17;
    public byte req_dex2 = 16;
    public GameObject[] guards;
    public CameraController cameraController;
    public PlayableDirector guardsRunning;
    public PlayableDirector[] cutscenes = new PlayableDirector[6]; // str_suc, dex_suc, dex2_suc, str_fail, fail, attack

    void Start()
    {
        player.isControllable = false;    
    }

    void Update()
    {
        if(!guardsRunning.gameObject.activeInHierarchy) {
            if (!choiceEnded && !dialogueManager.dialogueActive)
            {
                dialogueManager.ShowDialogue("There is someone coming! What will you do?\n" + 
                $"1. [STR {PlayerData.Instance.strength}/{req_str}] Scare them off\n" + 
                $"2. [DEX {PlayerData.Instance.dexterity}/{req_dex}] Quickly hide in a barrel\n" +
                $"3. [DEX {PlayerData.Instance.dexterity}/{req_dex2}] Throw a torch\n" + 
                "4. Attack them with your bare hands", 
                false, 4, true, OnChosenCommand);
            }
            foreach(var cut in cutscenes) {
                if (cut != null) return;
            }
            Destroy(gameObject);
        }   
    }

    public void OnChosenCommand(int command)
    {
        choiceEnded = true;
        if (command == 0)
        {
            StartCoroutine(StrengthCheck());
        }
        else if (command == 1)
        {
            StartCoroutine(DexCheck());
        }
        else if (command == 2)
        {
            StartCoroutine(Dex2Check());
        }
        else if (command == 3)
        {
            StartCoroutine(Attack());
        }
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
    }

    IEnumerator Attack()
    {
        dialogueManager.timelineDirector = cutscenes[5];
        cutscenes[5].Play();
        while (dialogueManager.timelineDirector != null) yield return null;
        ChoiceEnded();
    }

    private IEnumerator DexCheck()
    {
        if (PlayerData.Instance.dexterity >=  req_dex) {
            dialogueManager.cutsceneEnded = false;
            dialogueManager.timelineDirector = cutscenes[1];
            cutscenes[1].Play();
            while (cutscenes[1].gameObject.activeInHierarchy) yield return null;
            dialogueManager.ShowDialogue("Obtained x2 of DEX Up!", true, 0, true, CloseDialogue);
            PlayerData.Instance.AddItem("DEX Up", 2);
            cameraController.followPlayer = true;
        }
        else
        {
            dialogueManager.timelineDirector = cutscenes[4];
            cutscenes[4].Play();
        }
        while (dialogueManager.timelineDirector != null) yield return null;
        ChoiceEnded();
    }

    private IEnumerator Dex2Check()
    {
        if (PlayerData.Instance.dexterity >= req_dex2)
            {
                WorldState.Instance.castleFire = true;
                dialogueManager.timelineDirector = cutscenes[2];
                cutscenes[2].Play();
            }
            else
            {
                dialogueManager.timelineDirector = cutscenes[4];
                cutscenes[4].Play();
            }
            while (dialogueManager.timelineDirector != null) yield return null;
            ChoiceEnded();
    }

    IEnumerator StrengthCheck()
    {
        if (PlayerData.Instance.strength >= req_str)
        {
            dialogueManager.timelineDirector = cutscenes[0];
            cutscenes[0].Play();
            while(cutscenes[0].gameObject.activeInHierarchy) yield return null;
            dialogueManager.ShowDialogue("Obtained x2 of Stat Boost!", true, 0, true, CloseDialogue);
            PlayerData.Instance.AddItem("Stat Boost", 2);
        }
        else
        {
            dialogueManager.timelineDirector = cutscenes[3];
            cutscenes[3].Play();
        }
        while (dialogueManager.timelineDirector != null) yield return null;
        ChoiceEnded();
    }

    private void ChoiceEnded()
    {
        foreach(var cut in cutscenes) {
            Destroy(cut.gameObject);
        }
    }
}
