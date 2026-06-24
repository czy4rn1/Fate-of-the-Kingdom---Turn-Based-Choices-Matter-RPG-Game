using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    private bool fastForward = false;
    void Start()
    {
        HideDialogue();
        ShowDialogue("Sample text, Sample text, Sample text, Sample text, Sample text, Sample text, Sample text, Sample text, Sample text");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) fastForward = true;
    }

    void ShowDialogue(string text)
    {       
        dialoguePanel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        for(int i=0; i<text.Length; i++)
        {
            dialogueText.text += text[i];
            if (!fastForward) yield return new WaitForSeconds(0.1f);
            else yield return new WaitForSeconds(0.01f);
        }
        fastForward = false;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
