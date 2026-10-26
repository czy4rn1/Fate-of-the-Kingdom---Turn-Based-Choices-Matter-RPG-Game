using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public BlackoutManager blackoutManager;
    public bool dontLoadOnTrigger;
    Player player;
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dontLoadOnTrigger) return;
        if (collision.CompareTag("Player")) {
            player = collision.GetComponent<Player>();
            StartCoroutine(LoadArea());
        }
    }

    public IEnumerator LoadArea()
    {
        if (player != null) player.isControllable = false;
        yield return StartCoroutine(blackoutManager.Fade(false));
        LoadScene(sceneName);
    }
}
