using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour, INotificationReceiver
{
    public RectTransform dialoguePanel;
    public RectTransform backgroundPanel;
    public TextMeshProUGUI dialogueText;
    private float currentPanelScale = 0f;
    public float scaleFactor;
    
    private bool fastForward = false;
    private bool isWaitingForPlayer = false;

    public PlayableDirector timelineDirector;

    public GameObject cursor;
    private byte cursorPos = 0;
    private const float cursorYStart = 12.8f;
    private const float cursorYJump = 4.5f;
    
    private byte commandPos = 0;
    private byte numOfCommands = 0;
    private bool commandsOpen = false;
    private KeyCode[] upKeys = {KeyCode.W, KeyCode.UpArrow};
    private KeyCode[] downKeys = {KeyCode.S, KeyCode.DownArrow};
    
    private Action<int> currentCommandCallback;
    
    public bool dialogueActive = false;
    
    private bool hideIfLast = false;
    
    void Start()
    {
        StopAllCoroutines();
        StartCoroutine(HideShowPanel("hide"));
        cursor.SetActive(false);
    }
    
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is DialogueMarker marker)
        {
            string text = marker.text;
            hideIfLast = marker.isItLast;
            if (text.Contains("!<STOP_CUTSCENE>!"))
            {
                timelineDirector.Stop();
                timelineDirector.gameObject.SetActive(false);
                timelineDirector = null;
            }
            else {
                ShowDialogue(text, true, 0, hideIfLast, null);
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && dialogueActive) {
            if (!isWaitingForPlayer && !fastForward) fastForward = true;
            else if(isWaitingForPlayer)
            {
                isWaitingForPlayer = false;
                fastForward = false;
                bool hideNow = hideIfLast;
                hideIfLast = false;
                if(hideNow)
                {
                    StopAllCoroutines();
                    StartCoroutine(HideShowPanel("hide"));
                }
                if (currentCommandCallback != null)
                {
                    if(cursor!=null) cursor.SetActive(false);
                    Action<int> callbackToExecute = currentCommandCallback;
                    currentCommandCallback = null;
                    callbackToExecute.Invoke(commandPos);                    
                }
                else if (commandsOpen)
                {
                    commandsOpen = false;
                    if(cursor!=null) cursor.SetActive(false);
                }
                else
                {
                    if(timelineDirector != null)
                    {
                        timelineDirector.Play();
                    }
                }
            }
        }
        if (commandsOpen && isWaitingForPlayer)
        {
            if (cursor != null && cursor.activeInHierarchy && upKeys.Any(key => Input.GetKeyDown(key))) UpdateCursor(true);
            else if (cursor != null && cursor.activeInHierarchy && downKeys.Any(key => Input.GetKeyDown(key))) UpdateCursor(false);
        }
    }
    
    public void ShowDialogue(string text, bool isItDialogue, byte numOfCommands, bool isItLast, Action<int> onCommandSelected = null)
    {
        if(timelineDirector!=null) timelineDirector.Pause();
        if (text.Contains("!<NAME>!"))
            {
                text = text.Replace("!<NAME>!", PlayerData.Instance.playerName);
            } 
        this.numOfCommands = numOfCommands;  
        hideIfLast = isItLast;
        currentCommandCallback = onCommandSelected; 
        if(!isItDialogue) fastForward = true;

        if (isItDialogue) {
            dialoguePanel.sizeDelta = new Vector2(dialoguePanel.sizeDelta.x, 20f);
            backgroundPanel.sizeDelta = new Vector2(dialoguePanel.sizeDelta.x, 20f);
            backgroundPanel.transform.localPosition = new Vector3(0f, 10f);
        }
        else {
            dialoguePanel.sizeDelta = new Vector2(dialoguePanel.sizeDelta.x, 30f);
            backgroundPanel.sizeDelta = new Vector2(dialoguePanel.sizeDelta.x, 30f);
            backgroundPanel.transform.localPosition = new Vector3(0f, 15f);
        }
        
        StopAllCoroutines();
        StartCoroutine(HideShowPanel("show"));               
        StartCoroutine(TypeText(text));
        commandsOpen = !isItDialogue;
        
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
    
    public IEnumerator HideShowPanel(string command)
    {
        if (dialoguePanel.TryGetComponent<CanvasGroup>(out CanvasGroup group))
        {
            if (command == "show") {
                group.alpha = 1;
                while(currentPanelScale < 1f)
                {
                    currentPanelScale += scaleFactor * Time.deltaTime;
                    if (currentPanelScale > 1f) currentPanelScale = 1f;
                    dialoguePanel.localScale = new Vector3(currentPanelScale, currentPanelScale, 1f);
                    yield return null;
                }
                dialogueActive = true;
            }
            else if (command == "hide") {
                while (currentPanelScale > 0f)
                {
                    currentPanelScale -= scaleFactor * Time.deltaTime;
                    if (currentPanelScale < 0f) currentPanelScale = 0f;
                    dialoguePanel.localScale = new Vector3(currentPanelScale, currentPanelScale, 1f);
                    yield return null;
                }
                group.alpha = 0;
                dialogueActive = false;
            }
        }   
    }
    public void setFastForward(bool x)
    {
        fastForward = x;
    }
    
    void UpdateCursor(bool moveUp)
    {
        if (moveUp)
        {
            if (cursorPos > 0) cursorPos--;
            if (commandPos > 0) commandPos--;
        }
        else if (!moveUp)
        {

            if (cursorPos < 2) {
                cursorPos++;
                if (numOfCommands < 3) {
                    cursorPos = 1;
                }
                if (numOfCommands < 2)
                {
                    cursorPos = 0;
                }
                if(commandPos < numOfCommands - 1) commandPos++;
            }

        }
        cursor.transform.localPosition = new Vector3(cursor.transform.localPosition.x, cursorYStart - cursorPos * cursorYJump);
    }
        
}
