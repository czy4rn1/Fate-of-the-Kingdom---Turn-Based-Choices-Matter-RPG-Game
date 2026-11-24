using System.Collections;
using UnityEngine;

public class DemonKid : MonoBehaviour
{
    InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public Portal garen;
    public Portal garenShadow;
    public Portal shadow;
    WaitTimer waitTimer = new WaitTimer();

    public string[] introLines;

    void Start()
    {
        initInteraction = GetComponent<InitInteraction>();
        if (WorldState.Instance.tookKid) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(Dialogue());
        }
    }

    IEnumerator Dialogue()
    {
        initInteraction.player.GetComponent<SpriteRenderer>().flipX = false;
        garen.transform.position = new Vector2(58.57f, 31.61f);
        garen.gameObject.SetActive(true);
        garen.visible = true;
        garenShadow.visible = true;
        if (!WorldState.Instance.kidIntro) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines));
            WorldState.Instance.kidIntro = true;
        }
        dialoguePlayer.PlayCommand("Garen: I mean it, man. I'll take care of her.\n1. Take her and go to Volsen.\n2. No. She stays here.", 2, OnChosenCommand);
    }

    void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            StartCoroutine(ChoiceDialogue(true));
        }
        else if (command == 1)
        {
            StartCoroutine(ChoiceDialogue(false));
        }
    }

    IEnumerator ChoiceDialogue(bool takeHer)
    {
        if (takeHer)
        {
            initInteraction.EnableInteraction(false);
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: I'll continue my search here. I'll see you in town", "!<NAME>!: Now go!"}, initInteraction.CloseInteraction));
            GetComponent<Portal>().visible = garen.visible = shadow.visible = garenShadow.visible = false;
            WorldState.Instance.tookKid = true;
            yield return StartCoroutine(waitTimer.Wait(2f));
            Destroy(garen.gameObject);
            Destroy(gameObject);
        }
        else
        {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: I don't trust any of this one bit.", "!<NAME>!: She's staying in here. At least for now."}, initInteraction.CloseInteraction));
            garen.visible = garenShadow.visible = false;
        }
    }
}
