using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class OpenHiddenPath : MonoBehaviour
{
    public Button button;
    public Lever lever;
    private bool playing = false;

    public DialogueManager dialogueManager;
    public Player player;
    public PlayableDirector playableDirector;
    public BlackoutManager blackoutManager;

    void Update()
    {
        if (!playing) {
            if (WorldState.Instance.kilmor_questStarted)
            {
                if (lever.flipped && button.pressed && !dialogueManager.dialogueActive)
                {
                    if (button.startCutscene && lever.startCutscene) {
                        playing = true;
                        WorldState.Instance.secretPathOpened = true;
                        StartCoroutine(PlayBlackoutCutscene());      
                    }        
                }
            }
        }
    }

    IEnumerator PlayBlackoutCutscene()
    {
        yield return blackoutManager.Fade(false);
        yield return null;
        yield return new WaitForSeconds(2f);
        yield return PlayCutscene();
    }

    IEnumerator PlayCutscene()
    {
        if (playableDirector != null)
        {
            dialogueManager.timelineDirector = playableDirector;
            playableDirector.Play();
            while (playableDirector.state == PlayState.Playing) yield return null;
            playableDirector = null;
        }

    }
}
