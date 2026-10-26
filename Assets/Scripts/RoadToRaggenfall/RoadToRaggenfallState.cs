using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoadToRaggenfallState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    public PlayDialogueLines dialoguePlayer;
    public Player player;
    public CameraController cameraController;
    public bool miniGameStarted = false;
    public TextMeshProUGUI textMeshPro;
    public GameObject Controls;
    public byte lives = 5;
    public GameObject[] enemies = new GameObject[2];
    private bool disableGame = false;
    WaitTimer waitTimer = new WaitTimer();

    string[] storyLines = {"!<NAME>!: There are hoards of monsters coming our way!", 
        "!<NAME>!: We have to get rid of them or avoid them!",
        "In this section you have to attack enemies at the right time or avoid them to survive!", "Don't let them get you more than 5 times!"};

    void Start()
    {   
        if (WorldState.Instance.currentLevel == "RaggenfallPeak")
        {
            player.transform.position = new Vector2(-5f, player.transform.position.y);
            cameraController.transform.position = new Vector3(-5f, 0f, -10f);
            Controls.SetActive(false);
            enemies[0].SetActive(false);
            enemies[1].SetActive(false);
            disableGame = true;
        } 
        WorldState.Instance.currentLevel = "RoadToRaggenfall";
        textMeshPro.text += $"\nLIVES LEFT: {lives}";
        StartCoroutine(blackoutManager.Fade(true));
        if (!disableGame) StartCoroutine(StartMiniGame());
    }

    void Update()
    {
        textMeshPro.text = textMeshPro.text.Substring(0, textMeshPro.text.Length-1) + lives.ToString();
        if (lives <= 0) StartCoroutine(Lose());
    }

    IEnumerator Lose()
    {
        yield return StartCoroutine(blackoutManager.Fade(false));
        StopAllCoroutines();
        SceneManager.LoadScene("RoadToRaggenfall");
    }

    IEnumerator StartMiniGame()
    {
        player.isControllable = false;  
        yield return StartCoroutine(waitTimer.Wait(2f));
        if (WorldState.Instance.roadtorag_instructions) yield return StartCoroutine(dialoguePlayer.PlayDialogue(storyLines));
        else yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: There are hoards of monsters coming our way!"}));
        player.isControllable = true;
        miniGameStarted = true;
        WorldState.Instance.roadtorag_instructions = false;
    }
}
