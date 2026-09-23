using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;

public class DungeonKilmorChildrenMinigame : MonoBehaviour
{
    
    public PlayableDirector startCutscene;
    public PlayableDirector failedCutscene;
    public PlayableDirector successCutscene;
    private bool gameStarted = false;
    public bool gameEnded = false;
    public PlayDialogueLines dialoguePlayer;
    public string[] instructions = new string[3];
    public Player player;
    private bool intro = false;
    public byte curPhase = 0;
    public byte attemptsLeft = 2;
    public DialogueManager dialogueManager;
    public BlackoutManager blackoutManager;
    public GameObject[] rats = new GameObject[3];
    public GameObject glassBox;
    public MoveToCutscene moveToSuccessCutscene;
    public BoxCollider2D bottomBorder;

    void Start()
    {
        WorldState.Instance.currentLevel = "KilmorQuest";
        StartCoroutine(blackoutManager.Fade(true));
        bottomBorder.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameStarted)
        {
            if (!startCutscene.gameObject.activeInHierarchy && !intro)
            {
                intro = true;
                string[] fullInstructions = new string[4];
                fullInstructions[0] = "Pimm: You have to choose the correct box 3 times! When you choose a box, our dinner is moved to a different one!";
                fullInstructions[1] = "Ras: This one is quite a tricky puzzle! You can only make one mistake, and not a single one more";
                fullInstructions[2] = instructions[0];
                fullInstructions[3] = "PRESS X, TO SEE THE INSTRUCTIONS AGAIN";
                StartCoroutine(dialoguePlayer.PlayDialogue(fullInstructions, CloseDialogue));
            }
        }
        else
        {
            if (player.isControllable && Input.GetKeyDown(KeyCode.X))
            {
                player.isControllable = false;
                StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {instructions[curPhase]}, CloseDialogue));
            }
        }
        if (gameEnded) bottomBorder.gameObject.SetActive(true);
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        gameStarted = true;
    }

    public void NextPhase(bool correct, Action<int> action)
    {
        player.isControllable = false;
        if (correct) {
            curPhase++;
            if (curPhase < 3) StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {instructions[curPhase]}, action));
            else
            {
                glassBox.SetActive(false);
                foreach (GameObject rat in rats) rat.SetActive(true);
                moveToSuccessCutscene.StartCutscene();
                gameEnded = true;
                WorldState.Instance.savedChildren = true;
                WorldState.Instance.kilmor_questEnded = true;
            }
        }
        else
        {
            attemptsLeft--;
            if (attemptsLeft > 0) StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Ras: Wrong! No mistakes from now on!"}, action));
            else
            {
                dialogueManager.timelineDirector = failedCutscene;
                failedCutscene.Play();
                WorldState.Instance.kilmor_questEnded = true;
                gameEnded = true;
            }
            
        }
    }
}
