using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class AhorgisKing : MonoBehaviour
{
    private InitInteraction initInteraction;
    public string[] introDialogue;
    public PlayDialogueLines dialoguePlayer;
    public int reqStr = 35;
    public PlayableDirector floodCutscene;
    public PlayableDirector posessedCutscene;
    public MoveToCutscene ponter;
    public string[] strSucess;
    public string[] strFail;
    public string[] floodDialogue;
    WaitTimer waitTimer = new WaitTimer();
    public SpriteRenderer trident;
    public GameObject tridentFire;
    public Color tridentColor;

    void Start()
    {
        initInteraction = GetComponent<InitInteraction>();
    }

    
    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(Dialogue());
        }
    }



    void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            WorldState.Instance.ahorgis_state = (byte)WorldState.Ahorgis.Trident;
            StartCoroutine(StrCheck());          
        }
        else if (command == 1)
        {
            WorldState.Instance.ahorgis_state = (byte)WorldState.Ahorgis.Posessed;
            dialoguePlayer.dialogueManager.timelineDirector = posessedCutscene;
            posessedCutscene.Play();
        }
        else if (command == 2)
        {
            WorldState.Instance.ahorgis_state = (byte)WorldState.Ahorgis.Flood;
            StartCoroutine(Flood());
        }
        else {
            StartCoroutine(EndInteraction());
        }
    }

    IEnumerator EndInteraction()
    {
        if (!WorldState.Instance.ponterCaptured) yield return StartCoroutine(ponter.MoveForDialogue(true));
        initInteraction.CloseInteraction(0);
    }

    IEnumerator Dialogue()
    {
        if (!WorldState.Instance.ponterCaptured) yield return StartCoroutine(ponter.MoveForDialogue(false));
        if (WorldState.Instance.ahorgis_state <= (byte)WorldState.Ahorgis.BridgeOpened)
        {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introDialogue));
            WorldState.Instance.ahorgis_state = 15;    
        }
        if (WorldState.Instance.ahorgis_state <= (byte)WorldState.Ahorgis.ChoiceMade)
        {
            dialoguePlayer.PlayCommand("King: What do you choose?\n" +
            $"1. [STR {PlayerData.Instance.strength}/{reqStr}] I will take your Trident\n" +
            "2. Hand me your power, and my life is yours\n" +
            "3. Volsen is a sacrifice I'm willing to make\n"+
            "4. I have to think about it", 4, OnChosenCommand);
        }
        else
        {
            if (WorldState.Instance.ahorgis_state == (byte)WorldState.Ahorgis.Trident)
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"King: You have proved your strength.", "King: You may take the Trident and proceed with it as you wish."}));
            }
            else if (WorldState.Instance.ahorgis_state == (byte)WorldState.Ahorgis.Posessed)
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"King: Go forth, my apprentice.", "King: Let us rebuild my Empire together!"}));
            }
            else if (WorldState.Instance.ahorgis_state == (byte)WorldState.Ahorgis.Flood)
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"King: I hope to see you again one day.", "King: Perhaps you'd find peace in my rebuilt Empire."}));
            }
            yield return StartCoroutine(EndInteraction());
        }             
    }

    IEnumerator Flood()
    {
        dialoguePlayer.dialogueManager.timelineDirector = floodCutscene;
        floodCutscene.Play();
        while (floodCutscene.gameObject.activeInHierarchy) yield return null;
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(floodDialogue));
        yield return StartCoroutine(EndInteraction());
    }

    IEnumerator StrCheck()
    {
        StartCoroutine(dialoguePlayer.dialogueManager.HideShowPanel("Hide"));
        yield return waitTimer.Wait(2f);
        bool x = PlayerData.Instance.strength >= reqStr;
        if (x) trident.gameObject.SetActive(false);
        else {
            trident.color = tridentColor;
            tridentFire.SetActive(true);
        }
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(x ? strSucess : strFail));
        if (x) yield return StartCoroutine(EndInteraction());
        else
        {
            /// boss fight
        }
        tridentFire.SetActive(false);
    }
}
