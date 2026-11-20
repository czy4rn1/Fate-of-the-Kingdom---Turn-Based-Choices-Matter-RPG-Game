using System.Collections;
using UnityEngine;

public class kid : MonoBehaviour
{
    CharacterAnimation characterAnimation;
    InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public GameObject HellsMountain;
    public string[] dialogue;
    public string[] postDialogue;
    bool stop = false;
    bool goRight = true;
    bool goUp = false;
    public float[] xRange = new float[2];
    public float[] yRange = new float[2];
    public float moveSpeed = 3f;
    void Start()
    {
        if (WorldState.Instance.raggenfall_state != (byte)WorldState.Raggenfall.JewelBlade) Destroy(gameObject);
        initInteraction = GetComponent<InitInteraction>();
        characterAnimation = GetComponent<CharacterAnimation>();
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            stop = true;
            StartCoroutine(Dialogue());
        }
        if (stop) {
            characterAnimation.isRunning = false;
        }
        else {
            characterAnimation.isRunning = true;
            if (goRight) {
                GetComponent<SpriteRenderer>().flipX = false;
                transform.position = new Vector2(transform.position.x + moveSpeed*Time.deltaTime, transform.position.y);
                if (transform.position.x >= xRange[1]) goRight = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
                transform.position = new Vector2(transform.position.x - moveSpeed*Time.deltaTime, transform.position.y);
                if (transform.position.x <= xRange[0]) goRight = true;
            }
            if (goUp)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed*Time.deltaTime);
                if (transform.position.y >= yRange[1]) goUp = false;
            }
            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed*Time.deltaTime);
                if (transform.position.y <= yRange[0]) goUp = true;
            }
        }
    }

    IEnumerator Dialogue()
    {
        if (WorldState.Instance.raggenfall_state != (byte)WorldState.Raggenfall.InfoFromKid)
        {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(dialogue, initInteraction.CloseInteraction));
            WorldState.Instance.raggenfall_state = (byte)WorldState.Raggenfall.InfoFromKid;
            HellsMountain.SetActive(true);
        }
        else yield return StartCoroutine(dialoguePlayer.PlayDialogue(postDialogue, initInteraction.CloseInteraction));
        stop = false;
    }
}
