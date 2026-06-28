using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StrengthCheck : MonoBehaviour
{
    public GameObject interactIcon;
    private bool isPlayerNearby;
    public GameObject barrel;
    public Player player;
    private bool dialogueActive = false;
    public DialogueManager dialogueManager;

    void Start()
    {
        if(interactIcon != null) interactIcon.SetActive(false);
    }
    void Update()
    {
        if (barrel == null || !barrel.activeInHierarchy)
        {
            if (isPlayerNearby && Input.GetKeyDown(KeyCode.F) && player.isControllable && !dialogueActive)
            {
                dialogueActive = true;
                player.isControllable = false;
                dialogueManager.ShowDialogue($"This part of the floor seems to be damaged!\n1. [{PlayerData.Instance.strength}/15] Smash the floor and escape from jail.\n2. Ignore", false, 2, OnCommandSelected);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            isPlayerNearby = true;
            if (interactIcon != null)
            {
                interactIcon.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            isPlayerNearby = false;
            if (interactIcon != null)
            {
                interactIcon.SetActive(false);
            }
        }
    }

    public void OnCommandSelected(int chosenCommand)
    {
        if (chosenCommand == 0)
        {
            if (PlayerData.Instance.strength >= 15)
            {
                SceneManager.LoadScene("DungeonEscape");
                gameObject.SetActive(false);
            }
            else 
            {
            dialogueManager.HideShowPanel("hide");
            }
        }
        else if (chosenCommand == 1)
        {
            dialogueManager.HideShowPanel("hide");
        }
        player.isControllable = true;
        dialogueActive = false;
    }
}
