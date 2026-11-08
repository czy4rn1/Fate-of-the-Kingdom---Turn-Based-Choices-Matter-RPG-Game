using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public GameObject[] panels = new GameObject[3];
    public Player player;
    bool panelActive = false; 
    public GameObject cursor;
    int cursorPos = 0;
    byte menuType = (byte)buttons.Main;
    float cursorInput = 0f;
    public float cursorOffset = 0f;
    public TMP_Text[] mainButtons;
    public TMP_Text[] itemButtons;
    public float itemCursorOffet;
    bool mainCursorActive = true;
    public GameObject itemActionCursor;
    public TMP_Text[] itemActionButtons;
    public float itemActionCursorOffset;
    int chosenItem = -10;
    WaitTimer waitTimer = new WaitTimer();
    public TMP_Text infoText;
    Coroutine coroutine;

    enum buttons : byte
    {
        Main = 0,
        Items = 1,
        Equipment = 2,
    }
    void Awake()
    {
        PlayerData.Instance.AddItem("DEX Up", 2);
        cursorPos = 0;
        menuType = (byte)buttons.Main;
        panelActive = false;
        foreach(var panel in panels) panel.gameObject.SetActive(false);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!panelActive && player.isControllable) ActivateMenu();
            else if (panelActive && !player.isControllable) DeactivateMenu();
        }
        if (panelActive) {
            DrawMenu();
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (menuType == (byte)buttons.Main) MainMenuAction();
                else if (menuType == (byte)buttons.Items) ItemAction();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && menuType != (byte)buttons.Main) {
                menuType = (byte)buttons.Main;
                cursorPos = 0;
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) cursorInput = 1f;
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) cursorInput = -1f;
            else cursorInput = 0f;
            ChangeCursorPos();
        }
    }

    void MainMenuAction()
    {
        if (cursorPos + 1 == (byte)buttons.Items || cursorPos + 1 == (byte)buttons.Equipment) {
            menuType = (byte)(cursorPos+1);
            ChangeCursorPos(true);
        }
        else if (cursorPos + 1 == 3) cursorPos = cursorPos; // add Saving
        else if (cursorPos + 1 == 4) Application.Quit();

    }

    void ItemAction()
    {
        if (mainCursorActive) {
            chosenItem = cursorPos;
            mainCursorActive = false;
            cursor.SetActive(false);
            panels[1].SetActive(true);
            ChangeCursorPos(true);  
        }
        else
        {
            if (cursorPos == 0)
            {
                Item item = null;
                string itemName = "";
                if (chosenItem == 0) itemName = "Stat Boost";
                else if (chosenItem == 1) itemName = "DEX Up";
                else if (chosenItem == 2) itemName = "STR Up";
                else if (chosenItem == 3) itemName = "INT Up";
                else if (chosenItem == 4) itemName = "PER Up";
                item = PlayerData.Instance.playerItems.Find(o => o.itemName == itemName);
                if (item != null)
                {
                    string info = item.Use();
                    if(coroutine != null) StopCoroutine(coroutine);
                    coroutine = StartCoroutine(ShowInfo(info));
                }
            }
            if (cursorPos == 1)
            {
                ChangeCursorPos(true);
                mainCursorActive = true;
                cursor.SetActive(true);
                ChangeCursorPos(true);
                panels[1].SetActive(false);
                panels[2].SetActive(false);
            }
        }      
    }

    IEnumerator ShowInfo(string info)
    {
        panels[1].SetActive(false);
        ChangeCursorPos(true);
        mainCursorActive = true;
        cursor.SetActive(true);
        ChangeCursorPos(true);
        panels[2].SetActive(true);
        infoText.text = info;
        yield return StartCoroutine(waitTimer.Wait(3f));
        panels[2].SetActive(false);
        coroutine = null;
    }

    void ChangeCursorPos(bool reset = false) 
    {
            if (reset) cursorPos = 0;
            else {
                if (cursorInput > 0) cursorPos--;
                else if (cursorInput < 0) cursorPos++;
            }
            if (mainCursorActive) {
                if (menuType == (byte)buttons.Main) {
                    cursorPos = Math.Clamp(cursorPos, 0, mainButtons.Length-1);
                    cursor.transform.localPosition = new Vector2(cursor.transform.localPosition.x, mainButtons[cursorPos].transform.localPosition.y - cursorOffset);
                }
                else if (menuType == (byte)buttons.Items)
                {
                    cursorPos = Math.Clamp(cursorPos, 0, itemButtons.Length-1);
                    cursor.transform.localPosition = new Vector2(cursor.transform.localPosition.x, itemButtons[cursorPos].transform.localPosition.y + itemCursorOffet);
                }
            }
            else
            {
                cursorPos = Math.Clamp(cursorPos, 0, itemActionButtons.Length-1);
                itemActionCursor.transform.localPosition = new Vector2(itemActionCursor.transform.localPosition.x, itemActionButtons[cursorPos].transform.localPosition.y + itemActionCursorOffset);
            }

    }

    void ResetMenu()
    {
        menuType = (byte)buttons.Main;
        cursorPos = 0;
    }

    void DrawMenu()
    {
        if (menuType == (byte)buttons.Main)
        {
            ShowItemButtons(false);
            ShowMainButtons(true);
        }
        else
        {
            ShowMainButtons(false);
            if (menuType == (byte)buttons.Items)
            {
                ShowItemButtons(true);
            }
            else if (menuType == (byte)buttons.Equipment)
            {
                ShowItemButtons(false);
            }
        }
        
    }

    void ShowItemButtons(bool show)
    {
        if (!show)
        {
            foreach (var button in itemButtons) button.gameObject.SetActive(show);
        }
            else {string itemName = "";
            for (int i=0; i<itemButtons.Length; i++)
            {
                if (i == 0) itemName = "Stat Boost";
                else if (i == 1) itemName = "DEX Up";
                else if (i == 2) itemName = "STR Up";
                else if (i == 3) itemName = "INT Up";
                else if (i == 4) itemName = "PER Up";
                Item item = PlayerData.Instance.playerItems.Find(o => o.itemName == itemName);
                if (item != null) {
                    string amount = "";
                    if (item.count >= 10) amount = Math.Clamp(item.count, 10, 99).ToString();
                    else amount = $"0{item.count}";
                    itemButtons[i].text = itemButtons[i].text.Substring(0, itemButtons[i].text.Length-2) + amount;
                }
                else itemButtons[i].text = itemButtons[i].text.Substring(0, itemButtons[i].text.Length-2) + "00";
                itemButtons[i].gameObject.SetActive(show);
            }
        }
    }

    void ShowMainButtons(bool show)
    {
        foreach(var button in mainButtons) button.gameObject.SetActive(show);
    }

    void ActivateMenu()
    {
        mainCursorActive = true;
        cursor.SetActive(true);
        panels[1].SetActive(false);
        panels[2].SetActive(false);
        player.isControllable = false;
        panelActive = true;
        ResetMenu();
        panels[0].SetActive(true);
        menuType = (byte)buttons.Main;
    }

    void DeactivateMenu()
    {
        panelActive = false;
        foreach(var panel in panels) panel.gameObject.SetActive(false);
        player.isControllable = true;
    }
        
}
