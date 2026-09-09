using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoDialogue : MonoBehaviour
{
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public BlackoutManager blackoutManager;
    public string[] lines;
    public bool infoThenLoad;
    public string sceneName;

    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(PlayInfo());
        }
    }
    IEnumerator PlayInfo()
    {
        dialoguePlayer.dialogueEnded = false;
        StartCoroutine(dialoguePlayer.PlayDialogue(lines, initInteraction.CloseInteraction));
        while (!dialoguePlayer.dialogueEnded) yield return null;
        if (infoThenLoad && sceneName != null)
        {
            StartCoroutine(blackoutManager.Fade(false));
            while (blackoutManager.curAlpha < 1f) yield return null;
            SceneManager.LoadScene(sceneName); 
        }
    } 
}
