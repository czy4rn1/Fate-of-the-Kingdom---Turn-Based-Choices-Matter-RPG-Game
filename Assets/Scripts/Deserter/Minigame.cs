using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Playables;

public class Minigame : MonoBehaviour
{
    bool minigameActive = false;
    public DialogueManager dialogueManager;
    public GameObject[] diggingLayers = new GameObject[7];
    public PressBar pressBar;
    byte stage = 0;
    bool minigameEnded = false;
    float newBarScale = 0f;
    public GameObject barCanvas;
    public Portal[] timingBars = new Portal[3];
    public string[] endingLines;
    public string[] endingLines2;
    public string[] endingLines_2;
    public string[] endingLines2_2;
    public PlayDialogueLines dialoguePlayer;
    public GameObject fire;
    public PlayableDirector soulTaking;
    WaitTimer waitTimer = new WaitTimer();
    public BlackoutManager blackoutManager;
    public GameObject[] characters = new GameObject[3]; // player, ponter, deserter
    public CameraController cameraController;
    public GameObject sceneLoader;

    void Start()
    {
        barCanvas.SetActive(false);
        pressBar.gameObject.SetActive(false);
        Restart();
    }

    void Update()
    {
        if (minigameEnded) return;
        if (!dialogueManager.dialogueActive && minigameActive)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (pressBar.cursorOnBar) {
                    diggingLayers[stage++].SetActive(true);
                    if (stage >= diggingLayers.Length) {
                        minigameEnded = true;
                        StartCoroutine(DisableBars());
                        return;
                    }
                    NewBarPos();
                    newBarScale = pressBar.transform.localScale.x * 0.75f;
                    pressBar.transform.localScale = new Vector3(newBarScale, 0.8f, 1f);
                }
                else Restart();               
            }
        }
    }

    void Restart()
    {
        stage = 0;
        for (int i=0; i<diggingLayers.Length; i++) diggingLayers[i].SetActive(false);       
        pressBar.transform.localScale = new Vector3(0.2f, 0.8f, 1f);
        NewBarPos();
    }

    void NewBarPos()
    {
        pressBar.transform.localPosition = new Vector2(Random.Range(-0.36f, 0.36f), 0.512f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            minigameActive = true;
            if (!minigameEnded) {
                barCanvas.SetActive(true);
                pressBar.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            minigameActive = false;
            barCanvas.SetActive(false);
            pressBar.gameObject.SetActive(false);
        }
    }

    IEnumerator DisableBars()
    {
        timingBars[0].gameObject.SetActive(false);
        for (int i=1; i<3; i++)
        {
            timingBars[i].visible = false;
            if (i == 2) while (timingBars[i].curAlpha > 0) yield return null;
        }
        barCanvas.SetActive(false);
        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        sceneLoader.SetActive(true);
        if (WorldState.Instance.deserter_state == (byte)WorldState.Deserter.JoinedDeserter) yield return StartCoroutine(dialoguePlayer.PlayDialogue(endingLines));
        else if (WorldState.Instance.deserter_state == (byte)WorldState.Deserter.KilledDeserter) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(endingLines_2));
        }
        fire.SetActive(true);
        yield return StartCoroutine(waitTimer.Wait(3f));
        dialoguePlayer.dialogueManager.timelineDirector = soulTaking;
        soulTaking.Play();
        while (soulTaking.gameObject.activeInHierarchy) yield return null;
        yield return StartCoroutine(blackoutManager.Fade(false));
        if (WorldState.Instance.deserter_state == (byte)WorldState.Deserter.JoinedDeserter) characters[0].transform.position = new Vector2(-4.46f, -0.18f);
        else if (WorldState.Instance.deserter_state == (byte)WorldState.Deserter.KilledDeserter)
        {
            characters[0].transform.position = new Vector2(-2.01f, 1.09f);
            characters[0].GetComponent<SpriteRenderer>().flipX = true;
        }
        characters[1].transform.position = new Vector2(-4.46f, 1.09f);
        characters[1].GetComponent<SpriteRenderer>().flipX = false;
        characters[2].transform.position = new Vector2(-2.01f, 0.45f);
        cameraController.followPlayer = true;
        yield return StartCoroutine(waitTimer.Wait(1f));
        yield return StartCoroutine(blackoutManager.Fade(true));
        if (WorldState.Instance.deserter_state == (byte)WorldState.Deserter.JoinedDeserter) yield return StartCoroutine(dialoguePlayer.PlayDialogue(endingLines2, characters[2].GetComponent<Deserter>().initInteraction.CloseInteraction));
        else if (WorldState.Instance.deserter_state == (byte)WorldState.Deserter.KilledDeserter) {
            yield return StartCoroutine(dialoguePlayer.PlayDialogue(endingLines2_2, characters[2].GetComponent<Deserter>().initInteraction.CloseInteraction));
            characters[2].SetActive(false);
        }
    }
}
