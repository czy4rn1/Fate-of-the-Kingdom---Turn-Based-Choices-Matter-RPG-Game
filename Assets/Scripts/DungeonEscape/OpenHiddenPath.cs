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
            if (WorldState.Instance.dungeon_state >= (byte)WorldState.Dungeon.QuestStarted)
            {
                if (lever.flipped && button.pressed && !dialogueManager.dialogueActive)
                {
                    if (button.startCutscene && lever.startCutscene) {
                        playing = true;
                        WorldState.Instance.dungeon_state = (byte)WorldState.Dungeon.SecretPathOpened;
                        StartCoroutine(PlayCutscene());      
                    }        
                }
            }
        }
    }

    IEnumerator PlayCutscene()
    {
        if (playableDirector != null)
        {
            yield return StartCoroutine(blackoutManager.Fade(false));
            player.isControllable = false;
            dialogueManager.timelineDirector = playableDirector;
            playableDirector.Play();
            StartCoroutine(blackoutManager.Fade(true));
            while (playableDirector.state == PlayState.Playing) yield return null;
            playableDirector = null;
        }

    }
}
