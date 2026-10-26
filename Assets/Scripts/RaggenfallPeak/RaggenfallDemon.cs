using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class RaggenfallDemon : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    public bool front = false;
    public Sprite frontSprite;
    private Sprite backSprite;
    private SpriteRenderer sr;
    private InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public MoveToCutscene ponter;
    public MoveToCutscene movePlayer;
    public PlayableDirector sealMoving;
    public GameObject seal;
    public GameObject jewelBlade;
    public Portal demonShield;
    public GameObject ponterFire; 
    public Portal shadow;
    
    public string[] introLines;
    public string[] sacrificeDialogue;
    public string[] jewelBladeDialogue;
    public string[] intSuccess;
    public string[] intFail;

    private DialogueChoice[] storyChoices = new DialogueChoice[4];
    public GameObject playerFire;
    WaitTimer waitTimer = new WaitTimer();
    public byte reqInt = 40;
    void Start()
    {
        jewelBlade.SetActive(false);
        seal.SetActive(false);
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) backSprite = sr.sprite;
        initInteraction = GetComponent<InitInteraction>();
        WorldState.Instance.currentLevel = "RaggenfallPeak";
        StartCoroutine(blackoutManager.Fade(true));
        if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.DemonMoon) gameObject.SetActive(false);
    }

    void Update()
    {
        if (initInteraction.player.transform.position.x < transform.position.x) sr.flipX = false;
        else sr.flipX = true;
        
        if (WorldState.Instance.raggenfall_state > (byte)WorldState.Raggenfall.NotStarted &&
        WorldState.Instance.raggenfall_state < (byte)WorldState.Raggenfall.ChoiceMade) front = true;
        else if (WorldState.Instance.raggenfall_state > (byte)WorldState.Raggenfall.ChoiceMade) front = false;
        
        if (!front && sr != null && backSprite != null) sr.sprite = backSprite;
        else if (front && sr != null && frontSprite != null) sr.sprite = frontSprite; 
        
        if (initInteraction.Interaction())
        {
            StartCoroutine(Dialogue());
        }
    }

    IEnumerator Dialogue()
    {
        if (movePlayer.player.transform.position.x < transform.position.x) movePlayer.player.spriteRenderer.flipX = false;
        else movePlayer.player.spriteRenderer.flipX = true;
        if (!WorldState.Instance.ponterCaptured) yield return StartCoroutine(ponter.MoveForDialogue(false));
        if (WorldState.Instance.raggenfall_state <= (byte)WorldState.Raggenfall.NotStarted) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines[..4]));
            front = true;
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines[4..]));
            WorldState.Instance.raggenfall_state = 5;
        }
        if (WorldState.Instance.raggenfall_state < (byte)WorldState.Raggenfall.ChoiceMade)
        {
            for (int i=0; i<storyChoices.Length; i++)
            {
                storyChoices[i] = new DialogueChoice();
            }
            List<DialogueChoice> finalChoices = new List<DialogueChoice>();
            byte id = 0;
            storyChoices[0].text = "Fine. I will sacrifice myself";
            storyChoices[0].id = id++;
            finalChoices.Add(storyChoices[0]);
            storyChoices[1].text = $"[INT {PlayerData.Instance.intelligence}/{reqInt}] Trap the demon and use him as the weapon";
            storyChoices[1].id = id++;
            finalChoices.Add(storyChoices[1]);
            if (WorldState.Instance.jewelBladeObtained)
            {
                storyChoices[2].text = "I am the current owner of the Jewel Blade";
                storyChoices[2].id = id++;
                finalChoices.Add(storyChoices[2]);
            }
            storyChoices[3].text = "I have to think about it";
            storyChoices[3].id = id++;
            finalChoices.Add(storyChoices[3]);

            dialoguePlayer.PlayCommand("Demon: What are you planning?", finalChoices, OnChosenCommand);
        }
        else
        {
            if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.Moon) yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Demon: I admire your strength of heart.", "Demon: I hope you'll strike down your fo e."}));
            else if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.JewelBlade) yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Demon: You're the only one who can see the path.", "Demon: Go forth, and complete your mission."}));
            else if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.PosessedMoon) yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Demon: Go forth, my apprentice.", "Demon: Your Kingdom is about to have a new King..."}));
            yield return StartCoroutine(EndInteraction());            
        }
    }

    void OnChosenCommand(int command)
    {
        DialogueChoice chosen = storyChoices.FirstOrDefault(o => o.id == command);
            Debug.Log(chosen.text);
            for(int i=0; i<storyChoices.Length; i++)
            {
                if (chosen.id == storyChoices[i].id)
                {
                    StartCoroutine(StoryChoice(i));
                    break;
                }
            }
    }

    IEnumerator StoryChoice(int choice)
    {
        if (choice == 0)
        {
            yield return StartCoroutine(movePlayer.Move());
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(sacrificeDialogue[..8]));
            dialoguePlayer.dialogueManager.timelineDirector = sealMoving;
            seal.SetActive(true);
            sealMoving.Play();
            while (sealMoving.gameObject.activeInHierarchy) yield return null;
            waitTimer.Wait(1f);
            seal.SetActive(false);
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(sacrificeDialogue[8..]));
            WorldState.Instance.raggenfall_state = (byte)WorldState.Raggenfall.Moon;
        }
        else if (choice == 1)
        {
            yield return StartCoroutine(movePlayer.Move());
            if (PlayerData.Instance.intelligence >= reqInt)
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intSuccess[..9]));
                demonShield.gameObject.SetActive(true);
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intSuccess[9..10]));
                Portal portal = GetComponent<Portal>();
                if (portal != null) portal.visible = false;
                shadow.visible = false;
                demonShield.visible = false;
                yield return StartCoroutine(waitTimer.Wait(2f));
                demonShield.gameObject.SetActive(false);
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intSuccess[10..]));
                WorldState.Instance.raggenfall_state = (byte)WorldState.Raggenfall.DemonMoon;
            }
            else
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intFail[..5]));
                demonShield.gameObject.SetActive(true);
                yield return StartCoroutine(waitTimer.Wait(2f));
                demonShield.visible = false;
                yield return StartCoroutine(waitTimer.Wait(2f));
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intFail[5..10]));
                ponterFire.SetActive(true);
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intFail[10..11]));
                yield return StartCoroutine(waitTimer.Wait(2f));
                ponter.character.gameObject.SetActive(false);
                ponterFire.SetActive(false);
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intFail[11..14]));
                playerFire.SetActive(true);
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intFail[14..15]));
                yield return StartCoroutine(waitTimer.Wait(2f));
                WorldState.Instance.playerPosessed = true;
                yield return StartCoroutine(waitTimer.Wait(2f));
                playerFire.SetActive(false);
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intFail[15..17]));
                seal.SetActive(true);
                dialoguePlayer.dialogueManager.timelineDirector = sealMoving;
                sealMoving.Play();
                while (sealMoving.gameObject.activeInHierarchy) yield return null;
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(intFail[17..]));
                demonShield.gameObject.SetActive(false);
                WorldState.Instance.ponterCaptured = true;
                WorldState.Instance.raggenfall_state = (byte)WorldState.Raggenfall.PosessedMoon;
            }
        }
        else if (choice == 2)
        {
            jewelBlade.SetActive(true);
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(jewelBladeDialogue[..4]));
            jewelBlade.SetActive(false);
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(jewelBladeDialogue[4..]));
            WorldState.Instance.raggenfall_state = (byte)WorldState.Raggenfall.JewelBlade;
        }
        yield return StartCoroutine(EndInteraction());
    }

    IEnumerator EndInteraction()
    {
        if (!WorldState.Instance.ponterCaptured) yield return StartCoroutine(ponter.MoveForDialogue(true));
        initInteraction.CloseInteraction(0);
        if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.DemonMoon) gameObject.SetActive(false);
    }
}
