using UnityEngine;
using UnityEngine.Tilemaps;

public class Undigging : MonoBehaviour
{
    public InitInteraction initInteraction;
    public bool hasItem;
    private bool undigged = false;
    public TilemapRenderer tilemap;
    public PlayDialogueLines dialoguePlayer;
    void Start()
    {
        tilemap.sortingOrder = 0;
    }

    void Update()
    {
        if (undigged) return;
        if (initInteraction.Interaction())
            {
                tilemap.sortingOrder = 2;
                undigged = true;
                if (hasItem) StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Hey! I think I found something!"}, initInteraction.CloseInteraction));
                else StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Hmm... It's empty."}, initInteraction.CloseInteraction));
            }
    }
}
