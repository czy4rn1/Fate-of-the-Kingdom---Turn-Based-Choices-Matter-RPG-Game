using System.Collections;
using UnityEngine;

public class HellsGardenPath : AbstractPath
{
    public Player player;
    public PlayDialogueLines dialoguePlayer;
    public Enemy magnus;
    public Combat combat;
    public Magnus cameraShake;
    public string[] preFight;
    public string[] postFight;
    public GameObject magnusExplosion;
    WaitTimer waitTimer = new WaitTimer();
    public CharacterAnimation ponter;
    public BlackoutManager blackoutManager;

    override public IEnumerator StartPath()
    {
        yield return null;
        Vector2 ogPos = magnus.transform.position;
        player.isControllable = false;
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(preFight));
        
        combat.enemies[0] = magnus;
        combat.disableEscape = true;
        StartCoroutine(combat.StartBattle());
        while(!player.isControllable) yield return null;
        player.isControllable = false;
        player.GetComponent<SpriteRenderer>().flipX = false;
        cameraShake.cameraController.followPlayer = false;
        magnus.transform.position = ogPos;

        yield return StartCoroutine(dialoguePlayer.PlayDialogue(postFight));
        Coroutine coroutine = StartCoroutine(cameraShake.CameraShake());
        magnusExplosion.SetActive(true);
        yield return StartCoroutine(waitTimer.Wait(2.5f));
        StopCoroutine(coroutine);
        Destroy(magnus.gameObject);
        yield return StartCoroutine(waitTimer.Wait(2f));
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Ponter: We did it! Magnus is finally gone! We beat a demon!", "!<NAME>!: I can't believe we've done it!", 
        "!<NAME>!: Okay, Ponter. Let's get back to Volsen and rest up. We deserve it.", "Ponter: What about the other demons?", "!<NAME>!: That is a mission for another day. It's over.", 
        "!<NAME>!: We've won."}));
        yield return StartCoroutine(MoveCharacter());
        if (blackoutManager.currentCoroutine != null) StopCoroutine(blackoutManager.currentCoroutine);
        yield return blackoutManager.currentCoroutine = StartCoroutine(blackoutManager.Fade(false)); 
    }

    IEnumerator MoveCharacter()
    {
        float moveSpeed = 3f;
        Vector2 targetPos = new Vector2(-0.85f, -7.41f);;
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
