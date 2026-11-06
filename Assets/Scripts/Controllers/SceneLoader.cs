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
        if (WorldState.Instance.deserter_state > (byte)WorldState.Deserter.NotStarted) {
            if (sceneName == "Volsen" && WorldState.Instance.currentLevel == "Forest") sceneName = "Deserter";
        }
        if (WorldState.Instance.ahorgis_state == (byte)WorldState.Ahorgis.Flood) {
            if (sceneName == "Volsen" && WorldState.Instance.currentLevel == "Forest") sceneName = "MayhemIsland";
            else if (sceneName == "Volsen" && WorldState.Instance.currentLevel == "MayhemIsland") sceneName = "Forest";
        }
        
        if (blackoutManager.curAlpha != 0f) yield break;
        if (player != null) player.isControllable = false;
        yield return StartCoroutine(blackoutManager.Fade(false));
        LoadScene(sceneName);
    }
}
