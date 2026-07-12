using UnityEngine;

public class PickUp : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public DialogueManager dialogueManager;
    public Player player;
    public string commandText;
    private bool interactionActive = false;
    public string itemName;
    public byte numOfCommands;
    public string itemType;
    public byte[] stats = new byte[6];

    void Update()
    {
        if (player.isControllable && playerDetection.isPlayerNearby &&
        !dialogueManager.dialogueActive && Input.GetKeyDown(KeyCode.F)
        && !interactionActive)
        {
            interactionActive = true;
            player.isControllable = false;
            dialogueManager.ShowDialogue(commandText, false, numOfCommands, false, OnCommandSelected);
        }
    }

    public void OnCommandSelected(int command)
    {
        if (command == 0)
        {
            dialogueManager.ShowDialogue($"You've obtained the {itemName}", true, 0, true, CloseDialogue);
            if (itemType == "Weapon") {               
                PlayerData.Instance.AddEquipment(itemName, itemType, stats);
                Debug.Log("Obtained Jewel Blade");
                if (itemName == "Jewel Blade") WorldState.Instance.jewelBladeObtained = true;
            }
            else if (itemType == "Item")
            {
                PlayerData.Instance.AddItem(itemName, 1);
                if (itemName == "Red Gem") WorldState.Instance.redGemObtained = true;
                
            }
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
