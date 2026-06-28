using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour, INotificationReceiver
{
    public RectTransform dialoguePanel;
    public TextMeshProUGUI dialogueText;
    private bool fastForward = false;
    public PlayableDirector timelineDirector;
    private bool isWaitingForPlayer = false;
    private float currentAlpha = 0;
    public GameObject cursor;
    private byte cursorPos = 0;
    private byte commandPos = 0;
    private byte numOfCommands = 0;
    bool commandsOpen = false;
    private KeyCode[] upKeys = {KeyCode.W, KeyCode.UpArrow};
    private KeyCode[] downKeys = {KeyCode.S, KeyCode.DownArrow};
    private const float cursorYStart = 12.8f;
    private const float cursorYJump = 4.5f;
    private Action<int> currentCommandCallback;
    public bool cutscenePlaying = false;
    void Start()
    {
        HideShowPanel("hide");
        cursor.SetActive(false);
    }
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is DialogueMarker marker)
        {
            string text = marker.text;
            cutscenePlaying = marker.cutscene;
            if (marker.text.Contains("!<NAME>!"))
            {
                text = marker.text.Replace("!<NAME>!", PlayerData.Instance.playerName);
            }
            ShowDialogue(text, true, 0);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentAlpha == 1) {
            if (!isWaitingForPlayer && !fastForward) fastForward = true;
            else if(isWaitingForPlayer)
            {
                isWaitingForPlayer = false;
                fastForward = false;
                if(commandsOpen)
                {
                    commandsOpen = false;
                    if(cursor != null) cursor.SetActive(false);
                    if (currentCommandCallback != null)
                    {
                        currentCommandCallback.Invoke(commandPos);
                        currentCommandCallback = null;
                    }
                }
                else
                {
                    if(timelineDirector != null && cutscenePlaying) {
                        timelineDirector.Play();
                        cutscenePlaying = false;
                    }
                }
            }
        }
        if ((cursor != null || !cursor.activeInHierarchy) && upKeys.Any(key => Input.GetKeyDown(key))) UpdateCursor(true);
        else if ((cursor != null || !cursor.activeInHierarchy) && downKeys.Any(key => Input.GetKeyDown(key))) UpdateCursor(false);
    }
    public void ShowDialogue(string text, bool isItDialogue, byte numOfCommands, Action<int> onCommandSelected = null)
    {
        if(timelineDirector!=null) timelineDirector.Pause(); 
        this.numOfCommands = numOfCommands;  
        currentCommandCallback = onCommandSelected; 
        HideShowPanel("show");
        StopAllCoroutines();
        if (isItDialogue) dialoguePanel.sizeDelta = new Vector2(dialoguePanel.sizeDelta.x, 20f);
        else dialoguePanel.sizeDelta = new Vector2(dialoguePanel.sizeDelta.x, 30f);
        commandsOpen = !isItDialogue;
        StartCoroutine(TypeText(text));
        cursorPos = 0;
        commandPos = 0;
        if(!isItDialogue && cursor != null) cursor.SetActive(true);
        cursor.transform.localPosition = new Vector3(cursor.transform.localPosition.x, cursorYStart - cursorPos * cursorYJump);
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        for(int i=0; i<text.Length; i++)
        {
            dialogueText.text += text[i];
            if (!fastForward) yield return new WaitForSeconds(0.03f);
            else yield return new WaitForSeconds(0.001f);
        }
        fastForward = false;
        isWaitingForPlayer = true;
    }
    public void HideShowPanel(string command)
    {
        if (dialoguePanel.TryGetComponent<CanvasGroup>(out CanvasGroup group))
        {
            if (command == "show") group.alpha = 1;
            else if (command == "hide") group.alpha = 0;
            currentAlpha = group.alpha;
        }   
    }
    public void setFastForward(bool x)
    {
        fastForward = x;
        HideShowPanel("hide");
    }
    void UpdateCursor(bool moveUp)
    {
        if (moveUp)
        {
            if (cursorPos > 0) {
                cursorPos--;
                commandPos--;
            }
        }
        else if (!moveUp)
        {

            if (cursorPos < 2) {
                cursorPos++;
                if (cursorPos > numOfCommands) cursorPos = 1;
                if(commandPos < numOfCommands) commandPos++;
            }

        }
        cursor.transform.localPosition = new Vector3(cursor.transform.localPosition.x, cursorYStart - cursorPos * cursorYJump);
    }
        
}
