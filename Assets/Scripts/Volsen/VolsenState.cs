using UnityEngine;

public class VolsenState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    public Player player;
    public CameraController cameraController;
    void Start()
    {
        if (WorldState.Instance.currentLevel == "Forest") {
            player.transform.position = new Vector2(-16.5f, 8.5f);
            cameraController.transform.position = new Vector3(-10.78756f, 4.53726f, -10f);
        }
        else if (WorldState.Instance.currentLevel == "Beach") player.transform.position = new Vector2(8f, -12.2f);
        else if (WorldState.Instance.currentLevel == "VolsenInn") player.transform.position = new Vector2(0f, 2f);
        else player.transform.position = new Vector2(0f, 0f);
        WorldState.Instance.currentLevel = "Volsen";
        StartCoroutine(blackoutManager.Fade(true));
    }

}
