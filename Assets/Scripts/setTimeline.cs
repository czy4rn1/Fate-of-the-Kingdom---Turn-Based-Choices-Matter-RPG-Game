using UnityEngine;
using UnityEngine.Playables;

public class setTimeline : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public DialogueManager dialogueManager;
    void Start()
    {
        dialogueManager.timelineDirector = playableDirector;
    }

}
