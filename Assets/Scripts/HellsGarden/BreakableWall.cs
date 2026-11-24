using System.Collections;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{   
    InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public string[] garenLines;
    bool intro = false;
    public byte reqStr;
    bool choiceMade = false;
    public GameObject wall;
    public Portal garen;
    public Portal garenShadow;

    void Start()
    {
        initInteraction = GetComponent<InitInteraction>();
        if (WorldState.Instance.killedGaren || WorldState.Instance.sparedGaren) {
            initInteraction.EnableInteraction(false);
            enabled = false;
        }
        if (WorldState.Instance.destroyedWall) Destroy(gameObject);
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(Interaction());
        }
    }

    IEnumerator Interaction()
    {
        garen.transform.position = new Vector2(57.81f, 38.97f);
        garen.gameObject.SetActive(true);
        garen.visible = true;
        garenShadow.visible = true;
        if (!intro) {
            intro = true;
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(garenLines));
        }
        if (!choiceMade) dialoguePlayer.PlayCommand($"Garen: Help me out with this.\n1. We should leave it alone\n2. [{PlayerData.Instance.strength}/{reqStr}] Alright", 2, OnChosenCommand);


    }

    void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            StartCoroutine(DialogueAndHide("Garen: Eghh... Whatever, man."));
        }
        else if (command == 1)
        {
            if (PlayerData.Instance.strength >= reqStr) {
                Destroy(wall);
                initInteraction.CloseInteraction(command);
                choiceMade = true;
                WorldState.Instance.destroyedWall = true;
                garen.visible = false;
                garenShadow.visible = false;
                Destroy(gameObject);
            }
            else StartCoroutine(DialogueAndHide("ACTION FAILED"));
        }
    }

    IEnumerator DialogueAndHide(string line)
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {line}, initInteraction.CloseInteraction));
        garen.visible = false;
        garenShadow.visible = false;
    }


}
