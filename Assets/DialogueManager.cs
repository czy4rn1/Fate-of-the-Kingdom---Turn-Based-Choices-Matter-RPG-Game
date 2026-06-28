using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour, INotificationReceiver
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    private bool fastForward = false;
    public PlayableDirector timelineDirector;
    private bool isWaitingForPlayer = false;
    void Start()
    {
        HideShowPanel("hide");
        //ShowDialogue("Sample text, Sample text, Sample text, Sample text, Sample text, Sample text, Sample text, Sample text, Sample text");
    }
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is DialogueMarker marker)
        {
            ShowDialogue(marker.text);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (!isWaitingForPlayer) fastForward = true;
            else
            {
                isWaitingForPlayer = false;
                if(timelineDirector != null) timelineDirector.Play();
            }
        }
    }
    public void ShowDialogue(string text)
    {
        if(timelineDirector!=null) timelineDirector.Pause();       
        HideShowPanel("show");
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
            else yield return new WaitForSeconds(0.001f);
        }
        fastForward = false;
        isWaitingForPlayer = true;
    }
    public void HideShowPanel(string command)
    {
        if (command == "show")
        {
            if (dialoguePanel.TryGetComponent<CanvasGroup>(out CanvasGroup group))
            {
                group.alpha = 1;
            }
        }
        else if (command == "hide")
        {
            if (dialoguePanel.TryGetComponent<CanvasGroup>(out CanvasGroup group))
            {
                group.alpha = 0;
            }
        }
        
    }
}
