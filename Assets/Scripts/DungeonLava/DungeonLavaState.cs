using UnityEngine;
using UnityEngine.Playables;

public class DungeonLavaState : MonoBehaviour
{
    public BoxCollider2D right1;
    public BoxCollider2D right2;
    public Player player;
    public CameraController cameraController;
    public DialogueManager dialogueManager;
    public PlayableDirector playableDirector;
    public BlackoutManager blackoutManager;
    void Start()
    {
        playableDirector.gameObject.SetActive(true);
        dialogueManager.timelineDirector = playableDirector;
        WorldState.Instance.currentLevel = "DungeonLava";
        StartCoroutine(blackoutManager.Fade(true));       
    }

    void Update()
    {
        if (player.transform.position.y > -9f) {
            if (cameraController.rightBorder != right2) cameraController.UpdateBorder(right2, 3);
        }
        else
        {
            if (cameraController.rightBorder != right1) cameraController.UpdateBorder(right1, 3);
        }
    }
}
