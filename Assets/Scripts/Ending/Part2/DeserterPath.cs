using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeserterPath : AbstractPath
{

    public MoveToCutscene deserter;
    public PlayDialogueLines dialoguePlayer;
    public Player player;
    public CharacterAnimation ponter;
    public Combat combat;
    public Enemy magnus;
    public Magnus cameraShake;
    public string[] preFightNoDes;
    public string[] preFightDes;
    public string[] postFightDes;
    public string[] desFarewell;
    public string[] exorcism;
    public string[] badChoice;
    public SpriteRenderer magnusThrone;
    WaitTimer waitTimer = new WaitTimer();
    public BlackoutManager blackoutManager;
    public GameObject firePlayer, firePonter;
    public Color targetColor;


    override public IEnumerator StartPath()
    {
        yield return null;
        player.isControllable = false;
        if (WorldState.Instance.deserter_state == (byte)WorldState.Deserter.JoinedDeserter) {
            yield return StartCoroutine(deserter.MoveForDialogue(false, false, true));
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(preFightDes));
        }
        else {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(preFightNoDes));
        }
        Vector2 ogPos = magnus.transform.position;
        combat.enemies[0] = magnus;
        combat.disableEscape = true;
        StartCoroutine(combat.StartBattle());
        while(!player.isControllable) yield return null;
        player.isControllable = false;
        player.GetComponent<SpriteRenderer>().flipX = false;
        cameraShake.cameraController.followPlayer = false;
        magnus.transform.position = ogPos;
        if (WorldState.Instance.deserter_state == (byte)WorldState.Deserter.JoinedDeserter)
        {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(postFightDes));
            yield return StartCoroutine(Exorcise(false)); 
        }
        else
        {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Ponter: !<NAME>!! Magnus is weak! We have to do something with the soul!"}));
            dialoguePlayer.PlayCommand("What will you do?\n1. Push the soul into Magnus\n2. Crush the soul", 2, OnChosenCommand);
        }
        


    }

    void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            StartCoroutine(Exorcise(true));
        }
        else if (command == 1)
        {
            StartCoroutine(BadChoice());
        }
    }

    IEnumerator BadChoice()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(badChoice));
        firePlayer.SetActive(true);
        firePonter.SetActive(true);
        yield return StartCoroutine(waitTimer.Wait(3f));
        Destroy(player.gameObject);
        yield return StartCoroutine(waitTimer.Wait(2f));
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Magnus: It was a good attempt at striking me down... I have to give you that.",
        "Magnus: But you've made a grave mistake by not knowing how to use my human soul against me.", "Magnus: You yourself led to your deaths."}));
        WorldState.Instance.deserter_state = (byte)WorldState.Deserter.Failed;
        if (blackoutManager.currentCoroutine != null) StopCoroutine(blackoutManager.currentCoroutine);
        yield return blackoutManager.currentCoroutine = StartCoroutine(blackoutManager.Fade(false));
        SceneManager.LoadScene("EndingPart3");  
    }

    IEnumerator Exorcise(bool fromCommand)
    {
        if (fromCommand) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Let's exorcise this monster!"}));
        }
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(exorcism));
        Color curColor = magnusThrone.color;
        for (float time = 0f; time < 1f; time+=Time.deltaTime)
        {
            magnusThrone.color = Color.Lerp(curColor, targetColor, time / 1f);
            yield return null;
        }
        yield return StartCoroutine(waitTimer.Wait(2f));
        magnusThrone.GetComponent<Portal>().visible = false;
        yield return StartCoroutine(waitTimer.Wait(2f));
        Destroy(magnusThrone.gameObject);
        if (!fromCommand) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(desFarewell));
            yield return StartCoroutine(MoveCharacter(0));
        }
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Ponter: We did it! Magnus is finally gone! We beat a demon!", "!<NAME>!: I can't believe we've done it!", 
        "!<NAME>!: Okay, Ponter. Let's get back and rest up. We deserve it.", "Ponter: What about the other demons?", "!<NAME>!: That is a mission for another day. It's over.", 
        "!<NAME>!: We've won."}));
        yield return StartCoroutine(MoveCharacter(1));
        if (blackoutManager.currentCoroutine != null) StopCoroutine(blackoutManager.currentCoroutine);
        yield return blackoutManager.currentCoroutine = StartCoroutine(blackoutManager.Fade(false));  
        SceneManager.LoadScene("EndingPart3");  
    }

    IEnumerator MoveCharacter(byte character) // 0 - only deserter, 1 - player and ponter
    {
        float moveSpeed = 3f;
        Vector2 targetPos = new Vector2(-0.85f, -7.41f);;
        if (character == 0)
        {
            deserter.GetComponent<CharacterAnimation>().isRunning = true;
            while (Vector2.Distance(deserter.transform.position, targetPos) > 0.05f)
            {               
                deserter.transform.position = Vector2.MoveTowards(deserter.transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            deserter.GetComponent<CharacterAnimation>().isRunning = false;
        }
        else if (character == 1)
        {
            player.StopMoving();
            ponter.isRunning = true;
            while (Vector2.Distance(player.transform.position, targetPos) > 0.05f)
            {              
                player.characterAnimation.isRunning = true; 
                player.transform.position = Vector2.MoveTowards(player.transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            ponter.isRunning = false;
            player.characterAnimation.isRunning = false; 
        }

    }
}
