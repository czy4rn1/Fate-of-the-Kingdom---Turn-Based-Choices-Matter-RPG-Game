using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public BlackoutManager blackoutManager;
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            StartCoroutine(LoadArea());
        }
    }

    public IEnumerator LoadArea()
    {
        yield return blackoutManager.Fade(false);
        while (blackoutManager.curAlpha < 1f) yield return null;
        LoadScene(sceneName);
    }
}
