using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class OpeningCutsceneTimeline : MonoBehaviour
{
    public void OnEnable(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
