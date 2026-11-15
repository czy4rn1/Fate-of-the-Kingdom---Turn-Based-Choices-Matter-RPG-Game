using UnityEngine;
using UnityEngine.Playables;

public class SceneState : MonoBehaviour
{
    public GameObject fisherman;
    public PlayableDirector cutsceneDown;
    public PlayableDirector cutsceneUp;
    void Start()
    {
        if (WorldState.Instance.fisher_state == (byte)WorldState.Fisher.killed) Destroy(fisherman);
        if (WorldState.Instance.currentLevel == "Volsen") cutsceneUp.Play();
        else if (WorldState.Instance.currentLevel == "Beach") cutsceneDown.Play();
    }
}
