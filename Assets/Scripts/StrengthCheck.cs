using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StrengthCheck : MonoBehaviour
{
    public GameObject barrel;
    public Player player;
    public DialogueManager dialogueManager;
    public PlayerDetection playerDetection;
    public byte req_lvl;

    
    void Update()
    {
        if (barrel == null || !barrel.activeInHierarchy)
        {
            playerDetection.allowIcon = true;
            if (playerDetection != null && playerDetection.isPlayerNearby && Input.GetKeyDown(KeyCode.F) && player.isControllable && !dialogueManager.dialogueActive)
            {
                player.isControllable = false;
                dialogueManager.ShowDialogue(
                    $"This part of the floor seems to be damaged!\n1. [{PlayerData.Instance.strength}/{req_lvl}] Smash the floor and escape from jail.\n2. Ignore", 
                    false, 
                    2, 
                    false, 
                    OnCommandSelected);
            }
        }
    }
    public void OnCommandSelected(int chosenCommand)
    {
        if (chosenCommand == 0)
        {
            if (PlayerData.Instance.strength >= req_lvl)
            {
                SceneManager.LoadScene("DungeonEscape");
                gameObject.SetActive(false);
            }
            else 
            {
                dialogueManager.ShowDialogue("ACTION FAILED", true, 0, true, CloseFailedDialogue);
            }
        }
        else if (chosenCommand == 1)
        {
            dialogueManager.HideShowPanel("hide");
            player.isControllable = true;
        }
    }

    public void CloseFailedDialogue(int nothing)
    {
        player.isControllable = true;
    }
}
