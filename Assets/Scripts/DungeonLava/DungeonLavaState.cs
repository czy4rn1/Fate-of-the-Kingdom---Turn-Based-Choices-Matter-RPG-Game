using UnityEngine;
using UnityEngine.Playables;

public class DungeonLavaState : MonoBehaviour
{
    public BoxCollider2D right1;
    public BoxCollider2D right2;
    public GameObject ponter;
    public GameObject kilmor;
    public Player player;
    public CameraController cameraController;
    public DialogueManager dialogueManager;
    public PlayableDirector playableDirector;
    public BlackoutManager blackoutManager;
    void Start()
    {
        if (WorldState.Instance.currentLevel == "BlossomGarden")
        {
            player.transform.position = new Vector2(50.14f, -1.01f);
            cameraController.transform.position = new Vector3(45.14f, -1.01f, -10f);
            ponter.SetActive(false);
            kilmor.transform.position = new Vector2(13.38f, -18.96f);
        }
        else
        {
            if (!WorldState.Instance.lavaEntryCutscenePlayed) {
                playableDirector.gameObject.SetActive(true);
                dialogueManager.timelineDirector = playableDirector;
                dialogueManager.timelineDirector.Play();
                WorldState.Instance.lavaEntryCutscenePlayed = true;
            }
            else
            {
                ponter.SetActive(false);
                player.transform.position = new Vector2(10.38f, -18.96f);
                cameraController.transform.position = new Vector3(5.67f, -18.96f, -10f);
                kilmor.transform.position = new Vector2(13.38f, -18.96f);
            }
        }
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
