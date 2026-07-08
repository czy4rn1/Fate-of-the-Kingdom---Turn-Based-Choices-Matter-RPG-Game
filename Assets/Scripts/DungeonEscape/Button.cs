using UnityEngine;

public class Button : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public Player player;
    public bool pressed = false;
    public DialogueManager dialogueManager;
    public bool startCutscene = false;

    void Start()
    {
        playerDetection.allowIcon = false;
    }

    void Update()
    {
        if (WorldState.Instance.kilmor_questStarted)
        {
            if (!pressed) {
                if (playerDetection.isPlayerNearby &&
                player.isControllable &&
                !dialogueManager.dialogueActive)
                {
                    pressed = true;
                    player.isControllable = false;
                    player.StopMoving();
                    dialogueManager.ShowDialogue("Something just switched", true, 0, true, CloseDialogue);
                }
            }
        }
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        startCutscene = true;
    }
}
