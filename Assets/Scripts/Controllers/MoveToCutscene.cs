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
}
