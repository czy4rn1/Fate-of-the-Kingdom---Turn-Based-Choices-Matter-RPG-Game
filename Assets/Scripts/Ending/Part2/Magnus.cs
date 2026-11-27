using System.Collections;
using UnityEngine;

public class Magnus : MonoBehaviour
{
    
    public Enemy[] golems = new Enemy[3];
    public Enemy venard;
    public BlackoutManager blackoutManager;
    public Player player;
    public Combat combat;
    public PlayDialogueLines dialoguePlayer;
    public string[] golemsDialogue;
    public string[] golemsLostGem;
    public string[] venardBad;
    public string[] venardPostGolems;
    public string[] venardGood;
    public string[] venardSacrifice;
    public CameraController cameraController;
    WaitTimer waitTimer = new WaitTimer();
    public MoveToCutscene ponter;
    public GameObject magnusExplosion;
    public GameObject venardExplosion;
    bool golemsPresent = true;

    void Start()
    {
        if (WorldState.Instance.forest_state == (byte)WorldState.Forest.PickedGem) golemsPresent = false;
        if (!golemsPresent) foreach (var e in golems) if (e != null) Destroy(e.gameObject);
        StartCoroutine(Intro());
        cameraController.followPlayer = false;
        blackoutManager.currentCoroutine = StartCoroutine(blackoutManager.Fade(true));
    }

    IEnumerator Intro()
    {
        player.isControllable = false;
        yield return StartCoroutine(waitTimer.Wait(1f));
        if (!WorldState.Instance.ponterDead && !WorldState.Instance.ponterCaptured) yield return StartCoroutine(ponter.MoveForDialogue(false, false));
        yield return StartCoroutine(MoveCharacter(3));
        //cameraController.followPlayer = true;
        if (golemsPresent)
        {  
            if (WorldState.Instance.forest_state == (byte)WorldState.Forest.LostGem) yield return StartCoroutine(dialoguePlayer.PlayDialogue(golemsLostGem));         
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(golemsDialogue[..4]));
            StartCoroutine(MoveCharacter(0));
            yield return StartCoroutine(MoveCharacter(1));
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(golemsDialogue[4..]));
            if (!WorldState.Instance.venardRemorse) {
                combat.enemies = golems;
                combat.disableEscape = true;
                StartCoroutine(combat.StartBattle());
                while (!player.isControllable) yield return null;
                player.isControllable = false;
                cameraController.followPlayer = false;
                player.GetComponent<SpriteRenderer>().flipX = false;
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardPostGolems[..4]));
                yield return StartCoroutine(MoveCharacter(2));
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardPostGolems[4..]));
                combat.enemies[0] = venard;
                combat.disableEscape = true;
                StartCoroutine(combat.StartBattle());
                while (!player.isControllable) yield return null;
                player.isControllable = false;
                cameraController.followPlayer = false;
                player.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardGood[..1]));
                yield return StartCoroutine(MoveCharacter(2));
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardGood[1..]));
                StartCoroutine(MoveCharacter(0, true));
                StartCoroutine(MoveCharacter(1, true));
                yield return StartCoroutine(MoveCharacter(2, true));
                Destroy(golems[0].gameObject);
                Destroy(golems[1].gameObject);
                Destroy(venard.gameObject);
            }
        }
        else {
            if (!WorldState.Instance.venardRemorse)
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardBad[..4]));
                yield return StartCoroutine(MoveCharacter(2));
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardBad[4..]));
                combat.enemies[0] = venard;
                combat.disableEscape = true;
                StartCoroutine(combat.StartBattle());
                while (!player.isControllable) yield return null;
                cameraController.followPlayer = false;
                player.isControllable = false;
                player.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {                
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardSacrifice[..2]));
                yield return StartCoroutine(MoveCharacter(2));
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardSacrifice[2..6]));
                magnusExplosion.SetActive(true);
                yield return StartCoroutine(waitTimer.Wait(2.5f));
                magnusExplosion.SetActive(false);
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardSacrifice[6..9]));
                venardExplosion.SetActive(true);
                yield return StartCoroutine(waitTimer.Wait(2.5f));
                Destroy(venard.gameObject);
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(venardSacrifice[9..]));
            }
        }
    }
    
    IEnumerator MainPaths()
    {
        yield return null;
    }

    IEnumerator MoveCharacter(byte character, bool venardGood = false) // 0 - golem1, 1 - golem2, 2 - venard, 3 - player
    {
        float moveSpeed = 3f;
        Vector2 targetPos;
        if (character == 0)
        {
            if (!venardGood) targetPos = new Vector2(-1.58f, 0.95f);
            else {
                moveSpeed = 5f;
                targetPos = new Vector2(12.2f, 0.95f);
            }
            while (Vector2.Distance(golems[0].transform.position, targetPos) > 0.05f)
            {
                golems[0].transform.position = Vector3.MoveTowards(golems[0].transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else if (character == 1)
        {
            if (!venardGood) targetPos = new Vector2(2.11f, 0.95f);
            else {
                targetPos = new Vector2(12.2f, 0.95f);  
                moveSpeed = 5f;
            }  
            while (Vector2.Distance(golems[1].transform.position, targetPos) > 0.05f)
            {
                golems[1].transform.position = Vector3.MoveTowards(golems[1].transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }      
        }
        else if (character == 2)
        {
            
            if (venardGood) {
                targetPos = new Vector2(12.2f, 0.95f);
                moveSpeed = 5f;
            }
            else targetPos = new Vector2(-5.16f, 1.58f);

            venard.GetComponent<CharacterAnimation>().isRunning = true;
            while (Vector2.Distance(venard.transform.position, targetPos) > 0.05f)
            {               
                venard.transform.position = Vector2.MoveTowards(venard.transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }   
            venard.GetComponent<CharacterAnimation>().isRunning = false;
        }
        else if (character == 3)
        {
            player.StopMoving();
            targetPos = new Vector2(-1.05f, 0.45f);
            player.GetComponent<CharacterAnimation>().isRunning = true;
            if (!WorldState.Instance.ponterDead && !WorldState.Instance.ponterCaptured) ponter.character.isRunning = true;
            while (Vector2.Distance(player.transform.position, targetPos) > 0.05f)
            {               
                player.transform.position = Vector2.MoveTowards(player.transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }   
            player.GetComponent<CharacterAnimation>().isRunning = false;
            if (!WorldState.Instance.ponterDead && !WorldState.Instance.ponterCaptured) ponter.character.isRunning  = false;

        }
    }

    IEnumerator CameraShake()
    {
        cameraController.followPlayer = false;
        Vector3 targetUp = new Vector3(cameraController.transform.position.x, cameraController.transform.position.y + 0.2f, -10f);
        Vector3 targetDown = new Vector3(cameraController.transform.position.x, cameraController.transform.position.y - 0.2f, -10f);
        while (true)
        {
            while (Vector3.Distance(cameraController.transform.position, targetUp) > 0.05f)
            {
                cameraController.transform.position = Vector3.MoveTowards(cameraController.transform.position, targetUp, 5f * Time.deltaTime);
                yield return null;
            }
            while (Vector3.Distance(cameraController.transform.position, targetDown) > 0.05f)
            {
                cameraController.transform.position = Vector3.MoveTowards(cameraController.transform.position, targetDown, 5f * Time.deltaTime);
                yield return null;
            }
        }
    }
}
