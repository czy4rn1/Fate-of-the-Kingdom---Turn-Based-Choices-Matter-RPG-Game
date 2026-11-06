using System.Collections;
using Mono.Cecil;
using UnityEngine;

public class Deserter : MonoBehaviour
{
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public BlackoutManager blackoutManager;
    public SpriteRenderer ponter;
    public CameraController cameraController;
    public GameObject minigame;
    public GameObject shadow;
    public GameObject deserterFire;
    WaitTimer waitTimer = new WaitTimer();
    void Start()
    {
        initInteraction = GetComponent<InitInteraction>();
        minigame.SetActive(false);
        if (WorldState.Instance.deserter_state == (byte)WorldState.Deserter.KilledDeserter) gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (initInteraction.Interaction()) {
            initInteraction.player.GetComponent<SpriteRenderer>().flipX = false;
            if (WorldState.Instance.deserter_state < (byte)WorldState.Deserter.ChoiceMade)
            {
                dialoguePlayer.PlayCommand("Deserter: So can we start digging?\n" + 
                "1. Let's go\n" + 
                "2. All Magnus' knights must die\n" + 
                "3. I have to think about it", 3, OnChosenCommand);
            }
            else
            {
                StartCoroutine(PostDialogue());
            }
        }
        
    }

    void OnChosenCommand(int command)
    {
        if (command == 2) StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Deserter: I'll stay here, then."}, initInteraction.CloseInteraction));
        else StartCoroutine(StartMinigame(command));

    }

    IEnumerator StartMinigame(int command)
    {
        if (command == 0) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Deserter: Okay. Follow me."}));
            WorldState.Instance.deserter_state = (byte)WorldState.Deserter.JoinedDeserter;
        }
        else {
            WorldState.Instance.deserter_state = (byte)WorldState.Deserter.KilledDeserter;
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: I've learned enough. We know how to deal with Magnus, and you were a part of Magnus' army.",
            "!<NAME>!: You don't deserve to live.",
            "Deserter: Hey! Wait! There's no need for aggression, come on.",
            "!<NAME>!: Die."}));
            deserterFire.SetActive(true);
            yield return StartCoroutine(waitTimer.Wait(3f));
            deserterFire.SetActive(false);
            Color c = new Color
            {
                a = 0f
            };
            GetComponent<SpriteRenderer>().color = c;
            shadow.SetActive(false);
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Let's do this."}));
        }
        yield return StartCoroutine(blackoutManager.Fade(false));
        initInteraction.player.transform.position = new Vector2(8.16f, 1.84f);
        cameraController.followPlayer = false;
        cameraController.transform.position = new Vector3(12.1f, -0.2099999f, -10f);
        ponter.transform.position = new Vector2(16.02f, 1.84f);
        ponter.flipX = true;
        transform.position = new Vector2(17.06f, -0.27f);
        GetComponent<SpriteRenderer>().flipX = true;
        yield return StartCoroutine(blackoutManager.Fade(true));
        minigame.SetActive(true);
    }

    IEnumerator PostDialogue()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Deserter: You've said it yourself.", "Deserter: Let's get going and finish Magnus."}, initInteraction.CloseInteraction));
    }
}
