using UnityEngine;
using UnityEngine.Playables;

public class DungeonState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    public Player player;
    public CameraController cameraController;
    public PlayableDirector openingCutscene;
    public DialogueManager dialogueManager;
    void Start()
    {
        if (WorldState.Instance.currentLevel == "Beach") {
            openingCutscene.gameObject.SetActive(false);
            dialogueManager.timelineDirector = null;
            player.transform.position = new Vector2(58.94843f, 1.065027f);
            cameraController.transform.position = new Vector3(51.90147f, 1.065027f, -10f);
        }
        WorldState.Instance.currentLevel = "DungeonEscape";        
        StartCoroutine(blackoutManager.Fade(true));
    }

}
