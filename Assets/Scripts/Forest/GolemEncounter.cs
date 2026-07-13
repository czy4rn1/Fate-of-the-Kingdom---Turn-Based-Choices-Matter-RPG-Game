using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class GolemEncounter : MonoBehaviour
{
    public Player player;
    public DialogueManager dialogueManager;
    private bool introEnded = false;
    public PlayableDirector introScene;
    public PlayableDirector golemsLeave;
    public PlayableDirector dex_success;
    public PlayableDirector dex_fail;
    public BlackoutManager blackoutManager;
    public GameObject smallGolem;
    public byte req_dex;
    private bool cutsceneActive = false;
    public CameraController cameraController;

    void Update()
    {
        if (!introEnded) {
            if (WorldState.Instance.redGemObtained && !dialogueManager.dialogueActive)
            {
                player.isControllable = false;  
                if (introScene != null && !cutsceneActive) {
                    cutsceneActive = true;
                    StartCoroutine(PlayIntro());
                }           
            }
        }
    }

    public void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            WorldState.Instance.redGemObtained = false;
            WorldState.Instance.golemsHaveGem = true;
            PlayerData.Instance.RemoveItem("Red Gem");
            string[] lines = {"Removed Red Gem from Inventory", "Big Golem: Thank you a lot. We really appreciate that.", "Big Golem: See you soon, stranger. Haha!"};
            StartCoroutine(PlayDialogueGolemsLeave(lines));

        }
        else if (command == 1)
        {
            if (PlayerData.Instance.dexterity >= req_dex)
            {
                StartCoroutine(DexCutscene(dex_success));
            }
            else
            {
                WorldState.Instance.redGemObtained = false;
                WorldState.Instance.golemsHaveGem = true;
                StartCoroutine(DexCutscene(dex_fail));
            }
        }
        else if (command == 2)
        {
            StartCoroutine(HideAndDisable());
        }
    }

    IEnumerator HideAndDisable()
    {
        player.isControllable = true;
        yield return StartCoroutine(dialogueManager.HideShowPanel("hide"));
        yield return new WaitForSeconds(0.2f);
        smallGolem.SetActive(false);
        gameObject.SetActive(false);
    }

    IEnumerator PlayIntro()
    {
       blackoutManager.SetFade(false);
       while (blackoutManager.curAlpha < 1f) yield return null;
       dialogueManager.timelineDirector = introScene;
       introScene.Play();  
       while (!dialogueManager.cutsceneEnded) yield return null;
       dialogueManager.cutsceneEnded = false;
       dialogueManager.ShowDialogue("Big Golem: Please, give us the gem back. It's very important.\n" + 
                    "1. Do as they say\n" +
                    $"2. [DEX {PlayerData.Instance.dexterity}/{req_dex}] Escape\n"+
                    "3. Fight them", false, 3, false, OnChosenCommand);
       introEnded = true;
    }

    IEnumerator DexCutscene(PlayableDirector cutscene)
    {
        if (cutscene != null)
        {
            yield return StartCoroutine(dialogueManager.HideShowPanel("hide"));
            yield return new WaitForSeconds(0.2f);
            dialogueManager.timelineDirector = cutscene;
            cutscene.Play();
            while(!dialogueManager.cutsceneEnded) yield return null;
            dialogueManager.cutsceneEnded = false;
            cameraController.followPlayer = true;
            player.isControllable = true;
            smallGolem.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
    }

    public IEnumerator PlayDialogueGolemsLeave(string[] dialogueLines) 
    {
        yield return StartCoroutine(PlayDialogue(dialogueLines));
        yield return StartCoroutine(GolemsLeave());
        player.isControllable = true;
        cameraController.followPlayer = true;
    }

    public IEnumerator GolemsLeave()
    {
        if (golemsLeave != null)
        {
            golemsLeave.Play();
            while(golemsLeave.state == PlayState.Playing) yield return null;
            golemsLeave = null;
        }
    }
    
    public IEnumerator PlayDialogue(string[] dialogueLines)
    {
        for(int i=0; i<dialogueLines.Length; i++)
            {
                bool last = false;
                if (i == dialogueLines.Length-1) last = true;
                dialogueManager.ShowDialogue(dialogueLines[i], true, 0, last, last ? CloseDialogue : null);
                yield return null;
                while (!dialogueManager.isWaitingForPlayer) yield return null;
                while(dialogueManager.isWaitingForPlayer) yield return null; 
            }    
    }
}
