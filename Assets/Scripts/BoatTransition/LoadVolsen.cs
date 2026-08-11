using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LoadVolsen : MonoBehaviour
{
    public PlayableDirector playableDirector;
    
    void Start()
    {
         WorldState.Instance.currentLevel = "Beach";
    }

    void Update()
    {       
        if (playableDirector.state != PlayState.Playing) SceneManager.LoadScene("Volsen");
    }
}
