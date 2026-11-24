using System.Collections;
using UnityEngine;

public class DemonCombat : MonoBehaviour
{
    public Combat combat;
    public PlayDialogueLines dialoguePlayer;
    public string[] lines;
    public string[] postFightLines;
    public Player player;
    public Enemy demon;
    public Portal[] garen;
    BoxCollider2D bc;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) StartCoroutine(BossFight());
    }

    IEnumerator BossFight()
    {
        bc.enabled = false;
        bool x = !WorldState.Instance.tookKid && !WorldState.Instance.killedGaren && !WorldState.Instance.sparedGaren;
        player.isControllable = false;
        player.StopMoving();
        player.GetComponent<CharacterAnimation>().isRunning = false;
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(lines));
        combat.disableEscape = true;
        combat.enemies[0] = demon;
        StartCoroutine(combat.StartBattle());
        while (!player.isControllable) yield return null;
        player.isControllable = false;
        if(x)ShowHideGaren(true);
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(postFightLines[0..2]));
        if (x) yield return StartCoroutine(dialoguePlayer.PlayDialogue(postFightLines[2..]));
        if(x)ShowHideGaren(false);
        player.isControllable = true;
        Destroy(gameObject);
    }

    void ShowHideGaren(bool x)
    {
        if (x) {
            garen[0].transform.position = new Vector2(47.61f, 54.9f);
            garen[0].GetComponent<SpriteRenderer>().flipX = true;
            garen[0].gameObject.SetActive(true);
        }
        foreach (var por in garen) por.visible = x;
    }
}
