using UnityEngine;
using UnityEngine.Playables;

public class CastleMainPaths : MonoBehaviour
{
    public GameObject defaultEscape;
    public GameObject keyStolenPath;
    public PlayableDirector entryScene;
    public PlayableDirector playableDirector;
    public DialogueManager dialogueManager;
    public BlackoutManager blackoutManager;
    bool ended = false;

    void Start()
    {
        if (WorldState.Instance.keyStolen)
        {
            defaultEscape.SetActive(false);
            Destroy(defaultEscape);
            keyStolenPath.SetActive(true);
        }
        else {
            defaultEscape.SetActive(true);
            keyStolenPath.SetActive(false);
            Destroy(keyStolenPath);
        }
        StartCoroutine(blackoutManager.Fade(true));
    }

    void Update()
    {
        if (entryScene.state != PlayState.Playing && !ended)
        {
            dialogueManager.timelineDirector = playableDirector;
            ended = true;
            playableDirector.Play();
        }
    }

}
