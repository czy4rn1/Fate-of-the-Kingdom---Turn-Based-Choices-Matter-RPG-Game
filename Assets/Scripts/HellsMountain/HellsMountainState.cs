using UnityEngine;

public class HellsMountainState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    public Player player;
    public GameObject cam;
    void Start()
    {
        if (WorldState.Instance.currentLevel == "Volsen")
        {
            player.transform.position = new Vector2(-6.45f, 5.6f);
            cam.transform.position = new Vector3(-3.45f, 5.6f, -10f);
        }
        else if (WorldState.Instance.currentLevel == "HellsGarden")
        {
            player.transform.position = new Vector2(38.98f, 19.03f);
            cam.transform.position = new Vector3(38.98f, 17.03f, -10f);
        }
        WorldState.Instance.currentLevel = "HellsMountain";
        StartCoroutine(blackoutManager.Fade(true));
    }

    
}
