using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class DeserterState : MonoBehaviour
{
    private WaitTimer waitTimer = new WaitTimer();
    public PlayDialogueLines dialgouePlayer;
    public BlackoutManager blackoutManager;
    public Player player;
    public SpriteRenderer ponter;
    public SpriteRenderer deserter;
    public string[] introLines;
    public PlayableDirector deserterComing;
    public GameObject sceneLoader;
    void Start()
    {
        if (WorldState.Instance.deserter_state <= (byte)WorldState.Deserter.NotStarted)
        {
            player.transform.position = new Vector2(-10.79f, -0.21f);
            player.GetComponent<SpriteRenderer>().flipX = false;
            ponter.transform.position = new Vector2(-4.46f, -0.21f);
            ponter.flipX = true; 
            deserter.transform.position = new Vector2(5.67f, -0.27f);
            deserter.flipX = true;
            StartCoroutine(Intro());
        }
        else
        {
            player.transform.position = new Vector2(-10.79f, -0.21f);
            player.GetComponent<SpriteRenderer>().flipX = false;
            deserter.gameObject.transform.position = new Vector2(-2.01f, 0.45f);
            deserterComing.gameObject.SetActive(false);
        } 
        if (WorldState.Instance.deserter_state < (byte)WorldState.Deserter.ChoiceMade) sceneLoader.SetActive(false);
        else sceneLoader.SetActive(true);
        WorldState.Instance.currentLevel = "Deserter";
        StartCoroutine(blackoutManager.Fade(true));
    }

    IEnumerator Intro()
    {
        player.isControllable = false;
        yield return StartCoroutine(waitTimer.Wait(2f));
        yield return StartCoroutine(dialgouePlayer.PlayDialogue(introLines[..13]));
        ponter.flipX = false;
        yield return StartCoroutine(waitTimer.Wait(1.5f));
        dialgouePlayer.dialogueManager.timelineDirector = deserterComing;
        deserterComing.Play();
        while (deserterComing.gameObject.activeInHierarchy) yield return null;
        yield return StartCoroutine(dialgouePlayer.PlayDialogue(introLines[13..]));
        player.isControllable = true;
    }
}
