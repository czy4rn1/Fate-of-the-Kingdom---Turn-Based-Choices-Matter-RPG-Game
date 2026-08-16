using UnityEngine;
using UnityEngine.Playables;

public class ForestState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    public Player player;
    public CameraController cameraController;
    public GameObject[] golems = new GameObject[2];
    void Start()
    {
        if (WorldState.Instance.golemEncounterEnded)
        {
            foreach(GameObject golem in golems) golem.SetActive(false);
        }
        if (WorldState.Instance.currentLevel == "CastleForest") {
            player.transform.position = new Vector2(-12.2464f, 3.365173f);
            cameraController.transform.position = new Vector3(-4.480146f, 3.365172f, -10f);
        }
        else if (WorldState.Instance.currentLevel == "Volsen") {
            player.transform.position = new Vector2(34.59998f, 2.15f);
            cameraController.transform.position = new Vector3(27.44072f, 2.15f, -10f);
        }
        else player.transform.position = new Vector2(-7.21f, 2.94f);
        WorldState.Instance.currentLevel = "Forest";        
        StartCoroutine(blackoutManager.Fade(true));
    }
}
