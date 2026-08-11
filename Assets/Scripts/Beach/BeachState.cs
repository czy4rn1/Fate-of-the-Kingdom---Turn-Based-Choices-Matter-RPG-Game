using UnityEngine;

public class BeachState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    public Player player;
    public CameraController cameraController;
    void Start()
    {
        if (WorldState.Instance.currentLevel == "DungeonEscape") {
            player.transform.position = new Vector2(-12.12f, -2.37f);
            cameraController.transform.position = new Vector3(-7.092278f, -0.09005211f, -10f);
        }
        else if (WorldState.Instance.currentLevel == "Volsen") {
            player.transform.position = new Vector2(44.96027f, -17.92511f);
            cameraController.transform.position = new Vector3(40.83683f, -17.92512f, -10f);
        }
        else player.transform.position = new Vector2(-12.12f, -2.37f);
        WorldState.Instance.currentLevel = "Beach";        
        StartCoroutine(blackoutManager.Fade(true));
    }
}
