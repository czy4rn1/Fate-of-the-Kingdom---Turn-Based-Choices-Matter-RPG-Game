using UnityEngine;
using UnityEngine.Playables;

public class SceneState : MonoBehaviour
{
    public GameObject fisherman;
    public PlayableDirector cutsceneDown;
    public PlayableDirector cutsceneUp;
    void Start()
    {
        if (WorldState.Instance.fish_killed) fisherman.SetActive(false);
        if (WorldState.Instance.currentLevel == "Volsen") cutsceneUp.Play();
        else if (WorldState.Instance.currentLevel == "Beach") cutsceneDown.Play();
    }
}
