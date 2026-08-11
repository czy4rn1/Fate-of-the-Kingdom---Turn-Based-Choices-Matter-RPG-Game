using UnityEngine;

public class Chest : MonoBehaviour
{
    public string chest_id;
    private bool isOpen = false;
    private bool isPlayerNearby = false;
    private SpriteRenderer spriteRenderer;
    public Sprite openChestSprite;
    public Player player;

    public DialogueManager dialogueManager;

    public string item;
    public int amount;

    public string equipment;
    public string type;
    public byte[] stats = new byte[6];

    public GameObject interactIcon;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (interactIcon != null)
        {
            interactIcon.SetActive(false);
        }
        if (WorldState.Instance.IsChestOpen(chest_id)) setOpenChest();
    }

    void Update()
    {
        if (isPlayerNearby && !isOpen && Input.GetKeyDown(KeyCode.F) && player.isControllable)
        {
            player.isControllable = false;
            OpenChest();
        }
    }
    void OpenChest()
    {
        isOpen = true;
        WorldState.Instance.MarkChestOpen(chest_id);
        spriteRenderer.sprite = openChestSprite;
        if (interactIcon != null)
        {
            interactIcon.SetActive(false);
        }
        if (item != "NULL")
        {
            dialogueManager.ShowDialogue($"Found x{amount} of {item}!", true, 0, true, CloseDialogue);
            PlayerData.Instance.AddItem(item, amount);
        }
        else
        {
            dialogueManager.ShowDialogue($"Found {type}: {equipment}!", true, 0, true, CloseDialogue);
            PlayerData.Instance.AddEquipment(equipment, type, stats);
        }
        //Debug.Log("Player opened a chest");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen) return;
        if (collision.CompareTag("Player")) {
            isPlayerNearby = true;
            if (interactIcon != null && player.isControllable)
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
    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
    }

    private void setOpenChest()
    {
        isOpen = true;

        spriteRenderer.sprite = openChestSprite;
    }
}
