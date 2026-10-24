using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class DevilStatueQuest : MonoBehaviour
{
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public byte allItems = 4;
    public byte curItems = 0;
    public string[] introLines;
    public MoveToCutscene moveToCutscene;
    public GameObject digging;
    public BlackoutManager blackoutManager;
    public PlayableDirector cutscene;
    public string[] unlockAtlantis;
    public PlayableDirector entryCutscene;
    public GameObject devilPlatform;
    public Player player;
    public GameObject cameraController;

    void Start()
    {
        if (WorldState.Instance.currentLevel == "Ahorgis")
        {
            player.transform.position = new Vector2(-0.53f, -0.35f);
            cameraController.transform.position = new Vector3(-0.53f, -0.35f, -10f);
        }
        else if (WorldState.Instance.currentLevel == "Volsen")
        {
            player.transform.position = new Vector2(-1.11f, -16.31f);
            cameraController.transform.position = new Vector3(-1.11f, -16.31f, -10f);
        }
        WorldState.Instance.currentLevel = "MayhemIsland";
        if (WorldState.Instance.mayhem_state >= (byte)WorldState.Mayhem.Started && WorldState.Instance.mayhem_state < (byte)WorldState.Mayhem.Ended) digging.SetActive(true);
        else digging.SetActive(false);
        if (WorldState.Instance.mayhem_state <= (byte)WorldState.Mayhem.NotEntered){
            WorldState.Instance.mayhem_state = (byte)WorldState.Mayhem.Entered;
            dialoguePlayer.dialogueManager.timelineDirector = entryCutscene;
            entryCutscene.Play();
        }
        if (WorldState.Instance.mayhem_state >= (byte)WorldState.Mayhem.Ended)
        {
            devilPlatform.SetActive(true);
            for (int i=0; i<devilPlatform.transform.childCount; i++)
            {
                Portal portal = devilPlatform.transform.GetChild(i).GetComponent<Portal>();
                if (portal != null) portal.visible = true;
            }
        }
        StartCoroutine(blackoutManager.Fade(true));
    }

    
    void Update()
    {
        if (!initInteraction.player.isControllable) initInteraction.playerDetection.allowIcon = false;
        else initInteraction.playerDetection.allowIcon = true;
        if (initInteraction.Interaction())
        {
            StartCoroutine(InteractWithPonter());
        }
    }

    IEnumerator InteractWithPonter()
    {
        yield return StartCoroutine(moveToCutscene.MoveForDialogue(false));
        if (WorldState.Instance.mayhem_state < (byte)WorldState.Mayhem.Started) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(introLines));
            WorldState.Instance.mayhem_state = (byte)WorldState.Mayhem.Started;
            digging.SetActive(true);
        }
        else if (WorldState.Instance.mayhem_state >= (byte)WorldState.Mayhem.Started && WorldState.Instance.mayhem_state < (byte)WorldState.Mayhem.Ended)
        {
            if (curItems < allItems)
            {
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[]
                {
                    "!<NAME>!: Hmm... Nothing happened.",
                    "!<NAME>!: I suppose that's not enough."
                }));
            }
            else
            {
                WorldState.Instance.mayhem_state = (byte)WorldState.Mayhem.Ended;
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[]
                {
                    "!<NAME>!: Woah! You feel it?",
                    "Ponter: Yes, I do! Something big is happening!"
                }));
                yield return StartCoroutine(blackoutManager.Fade(false));
                cutscene.Play();
                StartCoroutine(blackoutManager.Fade(true));
                while(cutscene.state == PlayState.Playing) yield return null;
                yield return StartCoroutine(dialoguePlayer.PlayDialogue(unlockAtlantis));

            }
        }
        yield return StartCoroutine(moveToCutscene.MoveForDialogue(true));
        initInteraction.CloseInteraction(0);
    }
}
