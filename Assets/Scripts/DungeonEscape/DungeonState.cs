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
        if (WorldState.Instance.currentLevel == "Beach" || WorldState.Instance.currentLevel == "KilmorQuest" || WorldState.Instance.currentLevel == "DungeonLava") {
            Destroy(openingCutscene.gameObject);
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
            else if (WorldState.Instance.currentLevel == "DungeonLava")
            {
                player.transform.position = new Vector2(42.02f, -4.66f);
                cameraController.transform.position = new Vector3(42.02f, -4.66f, -10f);
            }
        }
        else if (WorldState.Instance.currentLevel == "DungeonEscape")
        {
            player.transform.position = new Vector2(9.86f, 12.85f);
            cameraController.transform.position = new Vector3(9.86f, 12.85f, -10f);
        }
        else {
            if (WorldState.Instance.dungeon_state == (byte)WorldState.Dungeon.LevelIntroNotPlayed) {
                cameraController.followPlayer = false;
                dialogueManager.timelineDirector = openingCutscene;
                openingCutscene.Play();
                WorldState.Instance.dungeon_state = (byte)WorldState.Dungeon.IntroNotPlayed;
            }
        }
        WorldState.Instance.currentLevel = "DungeonEscape";        
        StartCoroutine(blackoutManager.Fade(true));
    }

}
