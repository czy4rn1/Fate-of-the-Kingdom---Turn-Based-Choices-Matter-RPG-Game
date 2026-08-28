using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LoadVolsen : MonoBehaviour
{
    public PlayableDirector cutsceneUp;
    public PlayableDirector cutsceneDown;
    void Update()
    {       
        if (cutsceneUp.state != PlayState.Playing && cutsceneDown.state != PlayState.Playing) {
            if (WorldState.Instance.currentLevel == "Beach") SceneManager.LoadScene("Volsen");
            else if (WorldState.Instance.currentLevel == "Volsen") SceneManager.LoadScene("Beach");
        }
    }
}
