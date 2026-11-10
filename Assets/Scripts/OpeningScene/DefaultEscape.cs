using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class DefaultEscape : MonoBehaviour
{
    public float waitTimer;
    public bool startRunning = false;
    public PlayableDirector playableDirector;
    public Character character;
    private bool cutsceneStarted = false;
    public DialogueManager dialogueManager;
    public BlackoutManager blackoutManager;

    void Start()
    {
        StartCoroutine(blackoutManager.Fade(true));
    }

    void Update()
    {
        if (startRunning) {
            if (waitTimer > 0) waitTimer -= Time.deltaTime;
            if (!WorldState.Instance.keyStolen && waitTimer <= 0)
            {
                if(character!=null && !cutsceneStarted && character.wait) {
                    StartCoroutine(Ponter());
                }             
            }
        }
    }

    IEnumerator Ponter()
    {
        cutsceneStarted = true;
        yield return StartCoroutine(blackoutManager.Fade(false));       
        dialogueManager.timelineDirector = playableDirector;
        playableDirector.Play();
        StartCoroutine(blackoutManager.Fade(true));
    }

    public void StartCutscene(bool x)
    {
        startRunning = x;
    }
}
