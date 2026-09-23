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
        if (WorldState.Instance.currentLevel == "Beach" || WorldState.Instance.currentLevel == "KilmorQuest") {
            openingCutscene.gameObject.SetActive(false);
            dialogueManager.timelineDirector = null;
            if (WorldState.Instance.currentLevel == "Beach") {
                player.transform.position = new Vector2(58.94843f, 1.065027f);
                cameraController.transform.position = new Vector3(51.90147f, 1.065027f, -10f);
            }
            else if (WorldState.Instance.currentLevel == "KilmorQuest")
            {
                player.transform.position = new Vector2(17.74f, 10.64f);
                cameraController.transform.position = new Vector3(17.74f, 10.64f, -10f);
            }
        }
        WorldState.Instance.currentLevel = "DungeonEscape";        
        StartCoroutine(blackoutManager.Fade(true));
    }

}
