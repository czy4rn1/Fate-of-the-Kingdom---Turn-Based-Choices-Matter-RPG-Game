using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaggenfallPath : AbstractPath
{
    public PlayDialogueLines dialoguePlayer;
    public Combat combat;
    public GameObject demon;
    public string[] moon;
    public string[] moonPostFight;
    public string[] demonMoon;
    public string[] demonMoonPost;

    public string[] posessedMoon;
    public string[] posessedPost;
    public Player player;
    public CharacterAnimation ponter;
    public Enemy magnus;
    public Portal flashScreen;
    public GameObject seal;
    public GameObject demonSeal;
    public Magnus cameraShake;
    WaitTimer waitTimer = new WaitTimer();
    public BlackoutManager blackoutManager;
    public GameObject glassBox;
    public GameObject magnusGlassBox;

    override public IEnumerator StartPath()
    {
        Vector2 ogPos = magnus.transform.position;
        yield return null;
        
        if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.Moon) yield return StartCoroutine(dialoguePlayer.PlayDialogue(moon));
        else if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.DemonMoon) yield return StartCoroutine(dialoguePlayer.PlayDialogue(demonMoon));
        else if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.PosessedMoon) yield return StartCoroutine(dialoguePlayer.PlayDialogue(posessedMoon));
        
        if (WorldState.Instance.raggenfall_state != (byte)WorldState.Raggenfall.DemonMoon) {
            combat.enemies[0] = magnus;
            combat.disableEscape = true;
            StartCoroutine(combat.StartBattle());
            while(!player.isControllable) yield return null;
            player.isControllable = false;
            player.GetComponent<SpriteRenderer>().flipX = false;
            cameraShake.cameraController.followPlayer = false;
            magnus.transform.position = ogPos;
        }

        if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.Moon)
        {          
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(moonPostFight));
            yield return StartCoroutine(MoveCharacter(0));      
            seal.SetActive(true);
            StartCoroutine(cameraShake.CameraShake());
            yield return StartCoroutine(waitTimer.Wait(2f));
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Magnus: No! Don't do this!", "!<NAME>!: See you in hell."}));          
        }
        else if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.DemonMoon)
        {
            player.isControllable = false;
            demon.SetActive(true);
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(demonMoonPost[..1]));
            glassBox.SetActive(true);
            magnusGlassBox.SetActive(true);
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(demonMoonPost[1..10]));
            demonSeal.SetActive(true);
            StartCoroutine(cameraShake.CameraShake());
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(demonMoonPost[10..]));
            yield return StartCoroutine(MoveCharacter(1));
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Demon: Nooo!!!"}));          
        }
        else if (WorldState.Instance.raggenfall_state == (byte)WorldState.Raggenfall.PosessedMoon)
        {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(posessedPost));    
            seal.SetActive(true);
            StartCoroutine(cameraShake.CameraShake());
            yield return StartCoroutine(waitTimer.Wait(2f));
        }
        
        flashScreen.gameObject.SetActive(true);
        yield return StartCoroutine(waitTimer.Wait(5f));
        if (blackoutManager.currentCoroutine != null) StopCoroutine(blackoutManager.currentCoroutine);
        yield return blackoutManager.currentCoroutine = StartCoroutine(blackoutManager.Fade(false)); 
        SceneManager.LoadScene("EndingPart3");  
    }

    IEnumerator MoveCharacter(byte character) // 0 - only ponter, 1 - player and ponter
    {
        float moveSpeed = 3f;
        Vector2 targetPos = new Vector2(-0.85f, -7.41f);;
        if (character == 0)
        {
            ponter.isRunning = true;
            while (Vector2.Distance(ponter.transform.position, targetPos) > 0.05f)
            {               
                ponter.transform.position = Vector2.MoveTowards(ponter.transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            ponter.isRunning = false;  
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
