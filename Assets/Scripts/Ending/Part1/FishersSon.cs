using System.Collections;
using UnityEngine;

public class FishersSon : MonoBehaviour
{

    public Enemy[] enemies = new Enemy[3];
    public PlayDialogueLines dialoguePlayer;
    public float[] targetPos = new float[2];
    public float[] targetPos2 = new float[2];
    public Player player;
    public MoveToCutscene ponter;
    public string[] introLines;
    public string[] perSuc;
    public string[] perFail;
    public Combat combat;
    public byte reqPer;
    public BoxCollider2D bc;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Interaction());
        }
    }

    void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            StartCoroutine(Combat());
        }
        else if (command == 1)
        {
            if (PlayerData.Instance.persuasion >= reqPer)
            {
                StartCoroutine(PerSuccess());
            }
            else
            {
                StartCoroutine(PerFail());
            }
        }
    }

    IEnumerator PerSuccess()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(perSuc));
        yield return ShowUp(true);
        if (!WorldState.Instance.ponterDead && !WorldState.Instance.ponterCaptured) yield return StartCoroutine(ponter.MoveForDialogue(true, false));
        player.isControllable = true;
        WorldState.Instance.fisherSonSaved = true;
        yield return null;
        Destroy(gameObject);

    }

    IEnumerator PerFail()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(perFail));
        StartCoroutine(Combat());
    }

    IEnumerator Combat()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: You're a lost cause! Get ready to fight!"}));
        combat.enemies = enemies;
        yield return StartCoroutine(combat.StartBattle());
        while (!player.isControllable) yield return null;
        player.isControllable = false;
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: The throne room is right up ahead!"}));
        player.isControllable = true;
        if (!WorldState.Instance.ponterDead && !WorldState.Instance.ponterCaptured) yield return StartCoroutine(ponter.MoveForDialogue(true, false));
        Destroy(gameObject);
    }

    IEnumerator Interaction()
    {
        bc.enabled = false;
        player.isControllable = false;
        player.StopMoving();
        player.GetComponent<CharacterAnimation>().isRunning = false;
        FlipCharacters();
        if (!WorldState.Instance.ponterDead && !WorldState.Instance.ponterCaptured) yield return StartCoroutine(ponter.MoveForDialogue(false, false));
        yield return StartCoroutine(ShowUp());
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines[..3]));
        if (WorldState.Instance.fisher_state == (byte)WorldState.Fisher.willHelp)
        {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines[3..6]));
            dialoguePlayer.PlayCommand(introLines[introLines.Length-1] + "\n1. I don't have time for any more of this\n" +
            $"2. [PER {PlayerData.Instance.persuasion}/{reqPer}] You don't have to do this", 2, OnChosenCommand);
        }
        else
        {
            enemies[0].enemyName = "Guard";
            StartCoroutine(Combat());
        }
    }

    IEnumerator ShowUp(bool perChoice = false)
    {
        Vector2 target = !perChoice ? new Vector2(targetPos[0], targetPos[1]) : new Vector2(targetPos2[0], targetPos2[1]);
        enemies[0].GetComponent<CharacterAnimation>().isRunning = true;
        while (Vector2.Distance(enemies[0].transform.position, target) > 0.05f)
        {
            enemies[0].transform.position = Vector3.MoveTowards(enemies[0].transform.position, target, 4f * Time.deltaTime);
            yield return null;
        }
        enemies[0].GetComponent<CharacterAnimation>().isRunning = false;
    }

    void FlipCharacters()
    {
        if (enemies[0] != null) {
            if (player.transform.position.x >= enemies[0].transform.position.x)
            {
                player.GetComponent<SpriteRenderer>().flipX = true;
                enemies[0].GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                player.GetComponent<SpriteRenderer>().flipX = false;
                enemies[0].GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
}
