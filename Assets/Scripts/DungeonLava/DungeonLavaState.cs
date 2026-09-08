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
    void Start()
    {
        playableDirector.gameObject.SetActive(true);
        dialogueManager.timelineDirector = playableDirector;
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
