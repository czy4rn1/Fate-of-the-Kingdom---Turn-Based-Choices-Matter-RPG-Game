using System.Linq;
using TMPro;
using UnityEngine;


public class Creation : MonoBehaviour
{
    private byte[] attributes = new byte[6];
    public TextMeshProUGUI[] categories = new TextMeshProUGUI[6];
    public TextMeshProUGUI pointsLeftText;
    public GameObject cursor;
    public GameObject confirmCursor;
    bool confirmCursorActive = false;
    private byte confirmCursorPos = 0;
    public GameObject confirmPanel;
    private byte cursorPos = 0;
    private byte pointsLeft = 10;
    private KeyCode[] upKeys = {KeyCode.W, KeyCode.UpArrow};
    private KeyCode[] downKeys = {KeyCode.S, KeyCode.DownArrow};
    private KeyCode[] leftKeys = {KeyCode.A, KeyCode.LeftArrow};
    private KeyCode[] rightKeys = {KeyCode.D, KeyCode.RightArrow};
    private const float cursorYJump = 0.75f;
    private const float cursorYStart = 1.25f;
    private const float confirmCursorYJump = 0.4f;
    private const float confirmCursorYStart = -0.5f;

    public OpeningCutsceneTimeline oct;
    void Start()
    {
        oct = FindAnyObjectByType<OpeningCutsceneTimeline>();
        confirmCursor.SetActive(false);
        confirmPanel.SetActive(false);
        for(int i=0; i<categories.Length; i++)
        {
            categories[i].text = categories[i].text.Substring(0, categories[i].text.Length-2) + 10.ToString();
        }
        pointsLeftText.text = "Points left - 10";
        for(int i=0; i<attributes.Length; i++)
        {
            attributes[i] = 10;
        }
    }

    void Update()
    {
        if (upKeys.Any(key => Input.GetKeyDown(key))) UpdateCursor(true);
        else if (downKeys.Any(key => Input.GetKeyDown(key))) UpdateCursor(false);
        else if (leftKeys.Any(key => Input.GetKeyDown(key))) UpdateAttribute(cursorPos, false);
        else if (rightKeys.Any(key => Input.GetKeyDown(key))) UpdateAttribute(cursorPos, true);
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if(!confirmCursorActive) {
                cursor.SetActive(false);
                confirmPanel.SetActive(true);           
                confirmCursor.SetActive(true);
                confirmCursorPos = 0;
                confirmCursorActive = true;
            }
            else
            {
                if (confirmCursorPos == 1) {
                    confirmCursorPos = 0;
                    confirmCursor.transform.localPosition = new Vector3(confirmCursor.transform.localPosition.x, 
                    confirmCursorYStart - confirmCursorPos * confirmCursorYJump);
                    confirmCursor.SetActive(false);
                    confirmPanel.SetActive(false);
                    cursor.SetActive(true);
                    confirmCursorActive = false;
                }
                else if (confirmCursorPos == 0)
                {
                    if (oct != null) {
                        Debug.Log("Cofam scene");
                        PlayerData.Instance.playerName = "Zorax";
                        PlayerData.Instance.strength = attributes[0];
                        PlayerData.Instance.intelligence = attributes[1];
                        PlayerData.Instance.persuasion = attributes[2];
                        PlayerData.Instance.dexterity = attributes[3];
                        PlayerData.Instance.vitality = attributes[4];
                        PlayerData.Instance.defense = attributes[5];
                        oct.UnloadNextScene("CreatorScene");
                    }
                }
            }
        }
    }

    void UpdateAttribute(byte cursorPos, bool addPoint)
    {
        if(!confirmCursorActive) {
            if(addPoint)
            {
                if(pointsLeft > 0)
                {
                    attributes[cursorPos]++;
                    pointsLeft--;
                }
            }
            else
            {
                if(attributes[cursorPos] > 1) {
                    attributes[cursorPos]--;
                    pointsLeft++;
                }
            }
            
            if (attributes[cursorPos] < 10) {
                categories[cursorPos].text = categories[cursorPos].text.Substring(0, categories[cursorPos].text.Length-2) 
                + "0" + attributes[cursorPos].ToString();
            }
            else
            {
                categories[cursorPos].text = categories[cursorPos].text.Substring(0, categories[cursorPos].text.Length-2) 
                + attributes[cursorPos].ToString();
            }
            if (pointsLeft < 10)
            {
                pointsLeftText.text = pointsLeftText.text.Substring(0, pointsLeftText.text.Length-2) + "0" + pointsLeft.ToString();
            }
            else pointsLeftText.text = pointsLeftText.text.Substring(0, pointsLeftText.text.Length-2) + pointsLeft.ToString();
        }
    }
    void UpdateCursor(bool moveUp)
    {
        if(!confirmCursorActive) {
            if (moveUp)
            {
                if (cursorPos == 0) cursorPos = 5;
                else cursorPos--;
            }
            else if (!moveUp)
            {

                if (cursorPos == 5) cursorPos = 0;
                else cursorPos++;

            }
            cursor.transform.localPosition = new Vector3(cursor.transform.localPosition.x, cursorYStart - cursorPos * cursorYJump);
        }
        else
        {
            if (moveUp)
            {
                if (confirmCursorPos == 0) confirmCursorPos = 1;
                else confirmCursorPos--;
            }
            else if (!moveUp)
            {

                if (confirmCursorPos == 1) confirmCursorPos = 0;
                else confirmCursorPos++;

            }
            confirmCursor.transform.localPosition = new Vector3(confirmCursor.transform.localPosition.x, 
            confirmCursorYStart - confirmCursorPos * confirmCursorYJump);
        }
    }
}
