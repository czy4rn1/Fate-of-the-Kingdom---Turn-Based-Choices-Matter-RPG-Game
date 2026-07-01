using NUnit.Framework;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{
    private bool isOpen = false;
    private bool isPlayerNearby = false;
    private SpriteRenderer spriteRenderer;
    public Sprite openChestSprite;

    public DialogueManager dialogueManager;

    public string item;
    public int amount;

    public GameObject interactIcon;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (interactIcon != null)
        {
            interactIcon.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && !isOpen && Input.GetKeyDown(KeyCode.F))
        {
            OpenChest();
        }
    }
    void OpenChest()
    {
        isOpen = true;
        spriteRenderer.sprite = openChestSprite;
        if (interactIcon != null)
        {
            interactIcon.SetActive(false);
        }
        dialogueManager.ShowDialogue($"Found x{amount} of {item}!", true, 0, true);
        PlayerData.Instance.AddItem(item, amount);
        //Debug.Log("Player opened a chest");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen) return;
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
}
