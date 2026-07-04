using UnityEngine;

public class OpenCell : MonoBehaviour
{
    public PlayerDetection pd;
    public Player player;
    public DialogueManager dialogueManager;

    void Start()
    {
        if (pd != null) pd.allowIcon = false;
    }

    void Update()
    {
        if (WorldState.Instance.keyStolen && WorldState.Instance.guardRanAway) {
            if (pd != null) pd.allowIcon = true;
            if (pd.isPlayerNearby && player.isControllable && Input.GetKeyDown(KeyCode.F))
            {
                player.isControllable = false;
                dialogueManager.ShowDialogue($"Opened the Cell using Key To The Cell", true, 0, true, CloseDialogue);
                PlayerData.Instance.RemoveItem("Key To The Cell");
                gameObject.SetActive(false);
            }
        }
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
    }
}
