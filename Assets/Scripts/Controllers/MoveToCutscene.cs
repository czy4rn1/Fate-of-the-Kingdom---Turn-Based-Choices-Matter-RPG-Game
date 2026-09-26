using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class MoveToCutscene : MonoBehaviour
{
    public Player player;
    public DialogueManager dialogueManager;
    public PlayableDirector cutscene;
    public Transform targetPosition;
    public float moveSpeed = 5f;
    public CharacterAnimation character;
    public SpriteRenderer characterSR;
    public bool characterFinished;
    
    public void StartCutscene() => StartCoroutine(Move());

    public IEnumerator Move()
    {
        player.isControllable = false;
        while (Vector2.Distance(player.transform.position, targetPosition.position) > 0.05f)
        {
            player.characterAnimation.isRunning = true;
            player.transform.position = Vector2.MoveTowards(player.transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        player.transform.position = targetPosition.position;
        player.characterAnimation.isRunning = false;
        dialogueManager.timelineDirector = cutscene;
        cutscene.Play();
    }

    public IEnumerator MoveForDialogue(bool exit)
    {
        character.gameObject.SetActive(true);
        character.isRunning = true;
        Transform characterTransform = character.gameObject.transform;
        Vector2 playerPos = player.transform.position;
        if (!exit) characterTransform.position = playerPos;
        Vector2 target = new Vector2(exit ? playerPos.x : playerPos.x + 2f, playerPos.y);
        characterSR.flipX = exit;
        while (Vector2.Distance(character.gameObject.transform.position, target) > 0.05f)
        {    
            characterTransform.position = Vector2.MoveTowards(characterTransform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        characterTransform.position = target;
        characterSR.flipX = !characterSR.flipX;
        character.isRunning = false;
        if (exit) character.gameObject.SetActive(false);
    }
}
