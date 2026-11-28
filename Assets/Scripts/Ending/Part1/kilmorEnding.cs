using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class kilmorEnding : MonoBehaviour
{
    
    BoxCollider2D bc;
    public PlayDialogueLines dialoguePlayer;
    public Enemy enemy;
    public Enemy kids;
    public Combat combat;
    public string[] enemyLines;
    public string[] postFightLines;
    public string[] friendsLines;
    public Player player;
    public Portal[] ratAndShadow = new Portal[2];
    WaitTimer waitTimer = new WaitTimer();
    public float[] cameraPos = new float[2];
    public CameraController cameraController;
    public PlayableDirector showsUp;
    public PlayableDirector kilmorDealsWithGuards;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        if (WorldState.Instance.ponter_state == (byte)WorldState.Ponter.Castle || 
        WorldState.Instance.hellsGarden_state > (byte)WorldState.HellsGarden.NotStarted ||
        WorldState.Instance.dungeon_state == (byte)WorldState.Dungeon.Dead ||
        WorldState.Instance.dungeon_state == (byte)WorldState.Dungeon.Ignored) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) StartCoroutine(Interaction());
    }

    IEnumerator Interaction()
    {
        bc.enabled = false;
        player.isControllable = false;
        player.StopMoving();
        player.GetComponent<CharacterAnimation>().isRunning = false;
        yield return StartCoroutine(MoveCamera());

        dialoguePlayer.dialogueManager.timelineDirector = showsUp;
        showsUp.Play();
        while (showsUp.gameObject.activeInHierarchy) yield return null;       
        
        if (WorldState.Instance.dungeon_state == (byte)WorldState.Dungeon.Saved)
        {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(friendsLines));
            dialoguePlayer.dialogueManager.timelineDirector = kilmorDealsWithGuards;
            kilmorDealsWithGuards.Play();
            while (kilmorDealsWithGuards.gameObject.activeInHierarchy) yield return null;
            player.isControllable = true;
            yield return null;
            Destroy(gameObject);
        }
        else if (WorldState.Instance.dungeon_state == (byte)WorldState.Dungeon.Attacked)
        {
            Vector2 ogPos = transform.position;
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(enemyLines));
            combat.disableEscape = true;
            combat.enemies[0] = enemy;
            combat.enemies[1] = kids;
            yield return StartCoroutine(combat.StartBattle());
            while (!player.isControllable) yield return null;
            player.isControllable = false;
            transform.position = ogPos;
            cameraController.followPlayer = false;
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(postFightLines));
            ratAndShadow[0].visible = ratAndShadow[1].visible = false;
            yield return StartCoroutine(waitTimer.Wait(1f));
            player.isControllable = true;
            cameraController.followPlayer = true;
            yield return null;
            Destroy(gameObject);
        }
        else
        {
            player.isControllable = true;
            yield return null;
            Destroy(gameObject);
        }       
    }

    IEnumerator MoveCamera()
    {
        cameraController.followPlayer = false;
        Vector3 targetPos = new Vector3(cameraPos[0], cameraPos[1], -10f);
        while (Vector3.Distance(cameraController.transform.position, targetPos) > 0.05f)
        {
            cameraController.transform.position = Vector3.MoveTowards(cameraController.transform.position, targetPos, 3f * Time.deltaTime);
            yield return null;
        }
    }
}
