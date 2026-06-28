using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class OpeningCutsceneTimeline : MonoBehaviour
{
    public PlayableDirector director;
    public string mainScene;

    public void Start()
    {
        mainScene = SceneManager.GetActiveScene().name;
    }
    public void LoadNextScene(string sceneName)
    {
        if (director != null) director.Pause();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public void UnloadNextScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainScene));
        director.Play();
    }
}
