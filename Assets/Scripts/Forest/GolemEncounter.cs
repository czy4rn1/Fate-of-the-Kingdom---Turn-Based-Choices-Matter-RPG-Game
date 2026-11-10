using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class GolemEncounter : MonoBehaviour
{
    public Player player;
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
    public PlayDialogueLines dialoguePlayer;

    void Start()
    {
        if (WorldState.Instance.forest_state > (byte)WorldState.Forest.NotStarted)
        {
            Close();
        }
    }

    void Update()
    {
        if (!introEnded) {
            if (WorldState.Instance.forest_state == (byte)WorldState.Forest.PickedGem && !dialoguePlayer.dialogueManager.dialogueActive)
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
            WorldState.Instance.forest_state = (byte)WorldState.Forest.LostGem;
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
                WorldState.Instance.forest_state = (byte)WorldState.Forest.LostGem;
                StartCoroutine(DexCutscene(dex_fail));
            }
        }
    }

    IEnumerator PlayIntro()
    {
       yield return StartCoroutine(blackoutManager.Fade(false));
       dialoguePlayer.dialogueManager.timelineDirector = introScene;
       introScene.Play();
       StartCoroutine(blackoutManager.Fade(true));  
       while (!dialoguePlayer.dialogueManager.cutsceneEnded) yield return null;
       dialoguePlayer.dialogueManager.cutsceneEnded = false;
       dialoguePlayer.PlayCommand("Big Golem: Please, give us the gem back. It's very important.\n" + 
                    "1. Do as they say\n" +
                    $"2. [DEX {PlayerData.Instance.dexterity}/{req_dex}] Escape\n", 2, OnChosenCommand);
       introEnded = true;
    }

    IEnumerator DexCutscene(PlayableDirector cutscene)
    {
        if (cutscene != null)
        {
            dialoguePlayer.dialogueManager.timelineDirector = cutscene;
            cutscene.Play();
            while(!dialoguePlayer.dialogueManager.cutsceneEnded) yield return null;
            dialoguePlayer.dialogueManager.cutsceneEnded = false;
            cameraController.followPlayer = true;
            player.isControllable = true;
        }
        Close();
    }

    public IEnumerator PlayDialogueGolemsLeave(string[] dialogueLines) 
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(dialogueLines));
        if (golemsLeave != null)
        {
            golemsLeave.Play();
            while(golemsLeave.state == PlayState.Playing) yield return null;
        }
        player.isControllable = true;
        cameraController.followPlayer = true;
        Close();
    }

    void Close()
    {
        Destroy(introScene.gameObject);
        Destroy(golemsLeave.gameObject);
        Destroy(dex_success.gameObject);
        Destroy(dex_fail.gameObject);
        Destroy(smallGolem);
        Destroy(gameObject);
    }
}
