using UnityEngine;

public class CastleForestState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    public Player player;
    public CameraController cameraController;
    void Start()
    {
        if (WorldState.Instance.currentLevel == "Forest") {
            player.transform.position = new Vector2(16.87698f, -13.37693f);
            cameraController.transform.position = new Vector3(9.049997f, -11.99f, -10f);
        }
        else {
            player.transform.position = new Vector2(-6.3f, -0.1f);
            cameraController.transform.position = new Vector3(-6.299999f, -3.5f, -10f);
        }
        WorldState.Instance.currentLevel = "CastleForest";        
        StartCoroutine(blackoutManager.Fade(true));
    }
}
