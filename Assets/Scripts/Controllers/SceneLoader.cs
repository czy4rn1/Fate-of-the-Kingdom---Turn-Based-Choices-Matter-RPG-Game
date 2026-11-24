using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public BlackoutManager blackoutManager;
    public bool dontLoadOnTrigger;
    Player player;
    float timer = 3f;
    BoxCollider2D bc;
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (timer >= 0) {
            timer -= Time.deltaTime;
            if (bc != null) bc.isTrigger = false;
            return;
        }
        if (bc != null) bc.isTrigger = true;
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
        if (timer > 0) yield break;
        if (WorldState.Instance.deserter_state > (byte)WorldState.Deserter.NotStarted) {
            if (sceneName == "Volsen" && WorldState.Instance.currentLevel == "Forest") sceneName = "Deserter";
        }
        if (WorldState.Instance.ahorgis_state == (byte)WorldState.Ahorgis.Flood) {
            if (sceneName == "Volsen" && WorldState.Instance.currentLevel == "Forest") sceneName = "MayhemIsland";
            else if (sceneName == "Volsen" && WorldState.Instance.currentLevel == "MayhemIsland") sceneName = "Forest";
        }
        
        if (blackoutManager.curAlpha != 0f) yield break;
        if (player != null) {
            player.isControllable = false;
            player.StopMoving();
        }
        yield return StartCoroutine(blackoutManager.Fade(false));
        LoadScene(sceneName);
    }
}
