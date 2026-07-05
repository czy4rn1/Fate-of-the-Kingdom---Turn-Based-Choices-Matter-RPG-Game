using UnityEngine;

public class PickUp : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public DialogueManager dialogueManager;
    public Player player;
    public string commandText;
    private bool interactionActive = false;

    void Update()
    {
        if (player.isControllable && playerDetection.isPlayerNearby &&
        !dialogueManager.dialogueActive && Input.GetKeyDown(KeyCode.F)
        && !interactionActive)
        {
            interactionActive = true;
            player.isControllable = false;
            dialogueManager.ShowDialogue(commandText, false, 2, false, OnCommandSelected);
        }
    }

    public void OnCommandSelected(int command)
    {
        if (command == 0)
        {
            dialogueManager.ShowDialogue("You've obtained the Jewel Blade", true, 0, true, CloseDialogue);
            byte[] stats = {5, 0, 2, 0, 2, 5};
            PlayerData.Instance.AddEquipment("Jewel Blade", "Melee", stats);
            Debug.Log("Obtained Jewel Blade");
            WorldState.Instance.jewelBladeObtained = true;
            gameObject.SetActive(false);
        }
        else if (command == 1)
        {
            StopAllCoroutines();
            StartCoroutine(dialogueManager.HideShowPanel("hide"));
            CloseDialogue(0);
        }
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        interactionActive = false;
    }
}
