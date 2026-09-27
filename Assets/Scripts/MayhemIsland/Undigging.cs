using UnityEngine;
using UnityEngine.Tilemaps;

public class Undigging : MonoBehaviour
{
    public InitInteraction initInteraction;
    public bool hasItem;
    private bool undigged = false;
    public TilemapRenderer tilemap;
    public PlayDialogueLines dialoguePlayer;
    public GameObject bone;
    void Start()
    {
        tilemap.sortingLayerName = "Default";
        tilemap.sortingOrder = 0;
    }

    void Awake()
    {
        tilemap.sortingLayerName = "Default";
        tilemap.sortingOrder = 0;
        if (bone != null) bone.SetActive(false);
    }

    void Update()
    {
        if (undigged) return;
        if (initInteraction.Interaction())
            {
                tilemap.sortingLayerName = "Background";
                tilemap.sortingOrder = 1;
                undigged = true;
                if (hasItem) StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Hey! I think I found something!"}, initInteraction.CloseInteraction));
                else StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Hmm... It's empty."}, initInteraction.CloseInteraction));
                initInteraction.playerDetection.allowIcon = false;
                initInteraction.enabled = false;
                if (hasItem && bone != null) bone.SetActive(true);
            }
    }
}
