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

    void Update()
    {
        if (startRunning) {
            waitTimer -= Time.deltaTime;
            if (!WorldState.Instance.keyStolen && waitTimer <= 0)
            {
                if(character!=null && !cutsceneStarted && character.wait) {
                    dialogueManager.timelineDirector = playableDirector;
                    playableDirector.Play();
                    cutsceneStarted = true;
                }
                
            }
        }
    }

    public void StartCutscene(bool x)
    {
        startRunning = x;
    }
}
