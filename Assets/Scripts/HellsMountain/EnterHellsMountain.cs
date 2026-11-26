using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHellsMountain : MonoBehaviour
{
    InitInteraction initInteraction;
    public Player player;
    public PlayDialogueLines dialoguePlayer;
    public BlackoutManager blackoutManager;
    public Enemy garen;
    public Portal garenShadow;
    public float[] garenPos = new float[2];
    public Combat combat;
    public string commandEntry;
    public string[] combatLines;
    public string[] scareLines;
    public string[] introLines;
    public byte reqStr;
    WaitTimer waitTimer = new WaitTimer();
    void Start()
    {
        garen.gameObject.SetActive(false);
        initInteraction = GetComponent<InitInteraction>();
        if (WorldState.Instance.hellsGarden_state == (byte)WorldState.HellsGarden.Excalibur)
        {
            initInteraction.EnableInteraction(false);
            enabled = false;
        }
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            if (WorldState.Instance.raggenfall_state < (byte)WorldState.Raggenfall.EnteredMountain) StartCoroutine(Cutscene());
            else StartCoroutine(LoadMountain());
        }
    }

    IEnumerator Cutscene()
    {
        FlipCharacters();
        garen.gameObject.SetActive(true);
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines));
        byte numOfCommands = 2;
        string basicCommand = commandEntry + "\n1. Fine. Let's stay close and go together\n2. You are a threat to our mission\n";
        if (PlayerData.Instance.strength >= reqStr) {
            basicCommand += $"3. [{PlayerData.Instance.strength}/{reqStr}] Step aside before I slaughter you";
            numOfCommands++;
        }
        dialoguePlayer.PlayCommand(basicCommand, numOfCommands, OnChosenCommand);

    }

    void FlipCharacters()
    {
        if (player.transform.position.x >= garen.transform.position.x)
        {
            player.GetComponent<SpriteRenderer>().flipX = true;
            garen.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().flipX = false;
            garen.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            StartCoroutine(LoadMountain());
        }
        else if (command == 1)
        {
            StartCoroutine(Combat());
        }
        else if (command == 2)
        {
            StartCoroutine(ScareOff());
        }
    }

    void CombatChoice(int command)
    {
        if (command == 0)
        {
            StartCoroutine(KillGaren());
        }
        else if (command == 1)
        {
            StartCoroutine(SpareGaren());
        }
    }

    IEnumerator ScareOff()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(scareLines));
        garen.GetComponent<Portal>().visible = false;
        garenShadow.visible = false;
        yield return StartCoroutine(waitTimer.Wait(2f));
        PlayerData.Instance.money += 1000;
        WorldState.Instance.raggenfall_state = (byte)WorldState.Raggenfall.EnteredMountain;
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Obtained 1000 of Money!"}, initInteraction.CloseInteraction));
    }

    IEnumerator KillGaren()
    {
        garen.gameObject.SetActive(false);
        yield return StartCoroutine(waitTimer.Wait(2f));
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Alright. Let's get going."}));
        WorldState.Instance.killedGaren = true;
        StartCoroutine(LoadMountain());
    }

    IEnumerator SpareGaren()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: I'm not a killer. Get out of here before I change my mind", "Garen: Of course! I'm going! I'm so sorry and thank you!"}));
        garen.GetComponent<Portal>().visible = false;
        garenShadow.visible = false;
        WorldState.Instance.sparedGaren = true;
        yield return StartCoroutine(waitTimer.Wait(2f));
        StartCoroutine(LoadMountain());

    }

    IEnumerator LoadMountain()
    {
        WorldState.Instance.raggenfall_state = (byte)WorldState.Raggenfall.EnteredMountain;
        yield return blackoutManager.currentCoroutine = StartCoroutine(blackoutManager.Fade(false));
        SceneManager.LoadScene("HellsGarden");
    }

    IEnumerator Combat()
    {
        combat.disableEscape = true;
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(combatLines));
        combat.enemies[0] = garen;
        StartCoroutine(combat.StartBattle());
        while (!player.isControllable) yield return null;
        player.isControllable = false;
        garen.transform.position = new Vector2(garenPos[0], garenPos[1]);
        FlipCharacters();
        yield return StartCoroutine(waitTimer.Wait(2f));
        dialoguePlayer.PlayCommand("Garen: You beat me... Just finish me...\n1. End his life\n2. Spare him", 2, CombatChoice);

    }
}
