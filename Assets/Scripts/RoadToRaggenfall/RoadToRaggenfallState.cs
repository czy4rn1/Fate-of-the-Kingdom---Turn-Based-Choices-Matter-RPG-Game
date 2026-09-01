using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoadToRaggenfallState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    public PlayDialogueLines dialoguePlayer;
    public Player player;
    public bool miniGameStarted = false;
    public TextMeshProUGUI textMeshPro;
    public byte lives = 5;
    void Start()
    {      
        WorldState.Instance.currentLevel = "RoadToRaggenfall";
        textMeshPro.text += $"\nLIVES LEFT: {lives}";
        StartCoroutine(StartMiniGame());
    }

    void Update()
    {
        textMeshPro.text = textMeshPro.text.Substring(0, textMeshPro.text.Length-1) + lives.ToString();
        if (lives <= 0) StartCoroutine(Lose());
    }

    IEnumerator Lose()
    {
        StartCoroutine(blackoutManager.Fade(false));
        while (blackoutManager.curAlpha < 1f) yield return null;
        StopAllCoroutines();
        SceneManager.LoadScene("RoadToRaggenfall");
    }

    public IEnumerator StartMiniGame()
    {
        player.isControllable = false;
        StartCoroutine(blackoutManager.Fade(true));
        while (blackoutManager.curAlpha > 0f) yield return null;
        string[] storyLines = {$"{PlayerData.Instance.playerName}: There are hoards of monsters coming our way!", 
        $"{PlayerData.Instance.playerName}: We have to get rid of them or avoid them! There is no going back!",
        "In this section you have to attack enemies at the right time or avoid them to survive!", "Don't let them get you more than 5 times!"};       
        
        StartCoroutine(dialoguePlayer.PlayDialogue(WorldState.Instance.roadtorag_instructions ? storyLines : 
        new[] {$"{PlayerData.Instance.playerName}: There are hoards of monsters coming our way!"}, CloseDialogue));
        WorldState.Instance.roadtorag_instructions = false;
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
        miniGameStarted = true;
    }
}
