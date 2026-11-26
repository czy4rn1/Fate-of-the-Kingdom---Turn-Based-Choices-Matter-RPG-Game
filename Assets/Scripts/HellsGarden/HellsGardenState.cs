using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class HellsGardenState : MonoBehaviour
{
    public GameObject garen;
    public Portal garenShadow;
    public PlayDialogueLines dialoguePlayer;
    public Player player;
    public GameObject ponter;
    public string[] garenIntro;
    public string[] ponterIntro;
    WaitTimer waitTimer = new WaitTimer();
    bool garenPresent = true;
    string command;
    public MoveToCutscene moveToCutscene;
    public PlayableDirector secretPath;
    public GameObject secretPathObject;
    public BlackoutManager blackoutManager;
    public CameraController cameraController;

    void Start()
    {
        garenPresent = !WorldState.Instance.killedGaren && !WorldState.Instance.sparedGaren;
        if (!garenPresent) Destroy(garen);
        else
        {
            if (WorldState.Instance.hellsGarden_state > (byte)WorldState.HellsGarden.NotStarted) garen.SetActive(false);
        }
        if (WorldState.Instance.hellsGarden_state <= (byte)WorldState.HellsGarden.NotStarted)
        {
            secretPathObject.SetActive(true);
            cameraController.followPlayer = false;
            StartCoroutine(Intro());
        }
        else if (WorldState.Instance.hellsGarden_state == (byte)WorldState.HellsGarden.SecretPath) Destroy(secretPathObject);
        else secretPathObject.SetActive(true);
        WorldState.Instance.currentLevel = "HellsGarden";
        blackoutManager.currentCoroutine = StartCoroutine(blackoutManager.Fade(true)); 
    }


    void FirstChoice(int command)
    {
        if (command == 0)
        {
            StartCoroutine(SecretPath());
        }
        else if (command == 1) {
            StartCoroutine(Standard());
        }
    }

    IEnumerator Standard()
    {
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Garen: Ahh, whatever, man. Are you scared of everything like that?", "Garen: Sure, I'll follow your lead, then."}));
        ShowHideGaren(false);
        yield return StartCoroutine(waitTimer.Wait(1f));
        WorldState.Instance.hellsGarden_state = (byte)WorldState.HellsGarden.Standard;
        player.isControllable = true;
        cameraController.followPlayer = true;
    }

    void ShowHideGaren(bool x)
    {
        garen.GetComponent<Portal>().visible = x;
        garenShadow.visible = x;
    }

    IEnumerator SecretPath()
    {
        dialoguePlayer.dialogueManager.timelineDirector = secretPath;
        secretPath.Play();
        while (secretPath.gameObject.activeInHierarchy) yield return null;
        ShowHideGaren(false);
        WorldState.Instance.hellsGarden_state = (byte)WorldState.HellsGarden.SecretPath;
    }

    IEnumerator Intro()
    {
        player.isControllable = false;
        if (garenPresent) {
            garen.transform.position = new Vector2(player.transform.position.x + 2f, player.transform.position.y);
            garen.SetActive(true);
        }
        else {
            ponter.transform.position = new Vector2(player.transform.position.x + 2f, player.transform.position.y);
            ponter.SetActive(true);
        }
        yield return StartCoroutine(waitTimer.Wait(2f));
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(garenPresent ? garenIntro : ponterIntro));
        if (garenPresent) {
            command = "Garen: So what do you say?\n1. Follow Garen\n2. Refuse";
            dialoguePlayer.PlayCommand(command, 2, FirstChoice);
        }
        else StartCoroutine(EndInteraction());

    }

    IEnumerator EndInteraction()
    {
        yield return StartCoroutine(moveToCutscene.MoveForDialogue(true, false));
        player.isControllable = true;
        cameraController.followPlayer = true;
    }
}
