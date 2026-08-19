using UnityEngine;
using UnityEngine.Playables;

public class CastleMainPaths : MonoBehaviour
{
    public GameObject defaultEscape;
    public GameObject keyStolenPath;
    public PlayableDirector entryScene;
    public PlayableDirector playableDirector;
    public DialogueManager dialogueManager;
    bool ended = false;

    void Start()
    {
        if (WorldState.Instance.keyStolen)
        {
            defaultEscape.SetActive(false);
            keyStolenPath.SetActive(true);
        }
        else {
            defaultEscape.SetActive(true);
            keyStolenPath.SetActive(false);
        }
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
