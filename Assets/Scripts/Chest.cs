using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public string chest_id;
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;
    public Sprite openChestSprite;

    public string item;
    public int amount;

    public string equipment;
    public string type;
    public byte[] stats = new byte[4];
    InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public string[] possibleItems = {"DEX Up", "STR Up", "INT Up", "PER Up"};

    void Start()
    {
        initInteraction = GetComponent<InitInteraction>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (WorldState.Instance.IsChestOpen(chest_id)) setOpenChest();
    }

    void Update()
    {
        if (!isOpen && initInteraction.Interaction())
        {
            StartCoroutine(OpenChest());
        }
    }
    IEnumerator OpenChest()
    {
        isOpen = true;
        WorldState.Instance.MarkChestOpen(chest_id);
        spriteRenderer.sprite = openChestSprite;
        if (item != "NULL")
        {
            if (item == "Stat Up") {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {$"Found x{amount} of {item}!"}));
                string choices = "Which stat do you wish to upgrade?\n1. Dexterity\n2. Strength\n3. Intelligence\n4. Persuasion";
                dialoguePlayer.PlayCommand(choices, 4, OnChosenCommand);
            }
            else
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {$"Found x{amount} of {item}!"}));
                PlayerData.Instance.AddItem(item, amount);
            }
        }
        else
        {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {$"Found {type}: {equipment}!"}, initInteraction.CloseInteraction));
            PlayerData.Instance.AddEquipment(equipment, type, stats);
        }
        initInteraction.EnableInteraction(false);
    }

    void OnChosenCommand(int command)
    {
        item = possibleItems[command];
        PlayerData.Instance.AddItem(item, amount);
        StartCoroutine(informPlayer());
    }

    IEnumerator informPlayer()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {$"Obtained x{amount} of {item}!"}, initInteraction.CloseInteraction));
    }

    private void setOpenChest()
    {
        isOpen = true;
        initInteraction.EnableInteraction(false);
        spriteRenderer.sprite = openChestSprite;
    }
}
