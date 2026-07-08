using UnityEngine;

public class Lever : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public Player player;
    public DialogueManager dialogueManager;
    public SpriteRenderer spriteRenderer;

    public bool flipped = false;
    public bool startCutscene = false;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (WorldState.Instance.kilmor_questStarted)
        {
            playerDetection.allowIcon = true;
            if (!flipped) {
                if (playerDetection.isPlayerNearby &&
                player.isControllable &&
                !dialogueManager.dialogueActive &&
                Input.GetKeyDown(KeyCode.F))
                {
                    player.isControllable = false;
                    Flip();
                    dialogueManager.ShowDialogue("Something just switched", true, 0, true, CloseDialogue);
                }
            }
        }
        else playerDetection.allowIcon = false;
    }

    void Flip()
    {
        flipped = true;
        spriteRenderer.flipY = true;
        transform.localPosition = new Vector3(transform.localPosition.x, 0.4f);
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        startCutscene = true;
    }

    
}
