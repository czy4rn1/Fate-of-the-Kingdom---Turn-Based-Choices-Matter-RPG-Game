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
    int chosenEq = -10;
    WaitTimer waitTimer = new WaitTimer();
    public TMP_Text infoText;
    Coroutine coroutine;
    public string[] itemNames = {"Stat Boost", "DEX Up", "STR Up", "INT Up", "PER Up"};
    public string[] ogItemButtonsText = new string[5];
    string[] mainButtonsText = new string[5];
    public string[] equipmentNames = new string[5];

    enum buttons : byte
    {
        Main = 0,
        Items = 1,
        Equipment = 2,
        Stats = 3
    }
    void Awake()
    {
        for (int i=0; i<mainButtons.Length; i++)
        {
            mainButtonsText[i] = mainButtons[i].text;
        }
        for (int i=0; i<itemButtons.Length; i++)
        {
            ogItemButtonsText[i] = itemButtons[i].text;
        }
        cursorPos = 0;
        menuType = (byte)buttons.Main;
        panelActive = false;
        foreach(var panel in panels) panel.gameObject.SetActive(false);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!panelActive && player.isControllable && player.movement.x == 0f && player.movement.y == 0f) ActivateMenu();
            else if (panelActive && !player.isControllable) DeactivateMenu();
        }
        if (panelActive) {
            DrawMenu();
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (menuType == (byte)buttons.Main) MainMenuAction();
                else if (menuType == (byte)buttons.Items || menuType == (byte)buttons.Equipment) ItemAction();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && menuType != (byte)buttons.Main) {
                ActivateMenu();
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) cursorInput = 1f;
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) cursorInput = -1f;
            else cursorInput = 0f;
            ChangeCursorPos();
        }
    }

    void MainMenuAction()
    {
        if (cursorPos + 1 == (byte)buttons.Items || cursorPos + 1 == (byte)buttons.Equipment || cursorPos + 1 == (byte)buttons.Stats) {
            menuType = (byte)(cursorPos+1);
            ChangeCursorPos(true);
        }
        else if (cursorPos + 1 == 4) cursorPos = cursorPos; // add Saving
        else if (cursorPos + 1 == 5) Application.Quit();

    }

    void ItemAction()
    {
        if (menuType == (byte)buttons.Items) {
            Item item = null;
            if (mainCursorActive) {
            chosenItem = cursorPos;
            item = PlayerData.Instance.playerItems.Find(o => o.itemName == itemNames[chosenItem]);
            if (item == null) return;
            mainCursorActive = false;
            cursor.SetActive(false);
            itemActionCursor.SetActive(true);
            itemActionButtons[0].text = "Use";
            itemActionButtons[1].text = "Leave";
            panels[1].SetActive(true);
            ChangeCursorPos(true);  
            }
            else
            {
                if (cursorPos == 0)
                {
                    item = PlayerData.Instance.playerItems.Find(o => o.itemName == itemNames[chosenItem]);
                    if (item != null)
                    {
                        string info = item.Use();
                        if(coroutine != null) StopCoroutine(coroutine);
                        coroutine = StartCoroutine(ShowInfo(info));
                        infoText.fontSize = 3;
                    }
                    else return;
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
        else if (menuType == (byte)buttons.Equipment)
        {
            if (mainCursorActive)
            {
                chosenEq = cursorPos;
                Equipment equipment = PlayerData.Instance.equipment.Find(o => o.name == equipmentNames[chosenEq]);
                if (equipment != null) infoText.text = $"+{equipment.stats[0]} DEX\n+{equipment.stats[1]} STR\n+{equipment.stats[2]} INT\n+{equipment.stats[3]} PER\n";
                else {
                    return;
                }
                if (coroutine != null) StopCoroutine(coroutine);
                mainCursorActive = false;
                cursor.SetActive(false);
                panels[1].SetActive(true);
                itemActionButtons[0].text = "Equip";
                itemActionButtons[1].text = "Unequip";
                ChangeCursorPos(true);
                panels[2].SetActive(true);
                infoText.fontSize = 4; 
            }
            else
            {
                Equipment equipment = PlayerData.Instance.equipment.Find(o => o.name == equipmentNames[chosenEq]);
                if (equipment != null) {
                    if (cursorPos == 0)
                    {                    
                        string info = equipment.Equip();
                        if(coroutine != null) StopCoroutine(coroutine);
                        coroutine = StartCoroutine(ShowInfo(info));
                    }
                    else if (cursorPos == 1)
                    {
                        string info = equipment.Unequip();
                        if(coroutine != null) StopCoroutine(coroutine);
                        coroutine = StartCoroutine(ShowInfo(info));
                    }
                    infoText.fontSize = 3;
                }
                else return;               
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
                else if (menuType == (byte)buttons.Items || menuType == (byte)buttons.Equipment)
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
            if (menuType == (byte)buttons.Items)
            {
                ShowMainButtons(false);
                ShowItemButtons(true);
            }
            else if (menuType == (byte)buttons.Equipment)
            {
                ShowMainButtons(false);
                Equipment equipment = null;
                for (int i=0; i<equipmentNames.Length; i++)
                {
                    equipment = PlayerData.Instance.equipment.Find(o => o.name == equipmentNames[i]);
                    if (equipment == null) itemButtons[i].text = "";
                    else itemButtons[i].text = equipmentNames[i];
                    foreach (var button in itemButtons) button.gameObject.SetActive(true);
                }
            }
            else if (menuType == (byte)buttons.Stats)
            {
                cursor.SetActive(false);
                mainButtons[0].text = $"Name: {PlayerData.Instance.playerName}";
                mainButtons[1].text = $"DEX: {PlayerData.Instance.dexterity}";
                mainButtons[2].text = $"STR: {PlayerData.Instance.strength}";
                mainButtons[3].text = $"INT: {PlayerData.Instance.intelligence}";
                mainButtons[4].text = $"PER: {PlayerData.Instance.persuasion}";
            }
        }
        
    }

    void ShowItemButtons(bool show)
    {
        if (!show)
        {
            foreach (var button in itemButtons) button.gameObject.SetActive(show);
        }
        else {
            string itemName = "";
            for (int i=0; i<itemButtons.Length; i++)
            {
                itemName = itemNames[i];
                Item item = PlayerData.Instance.playerItems.Find(o => o.itemName == itemName);
                if (item != null) {
                    string amount = "";
                    if (item.count >= 10) amount = Math.Clamp(item.count, 10, 99).ToString();
                    else amount = $"0{item.count}";
                    itemButtons[i].text = ogItemButtonsText[i].Substring(0, ogItemButtonsText[i].Length-2) + amount;
                }
                else itemButtons[i].text = ogItemButtonsText[i].Substring(0, ogItemButtonsText[i].Length-2) + "00";
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
        for (int i=0; i<mainButtons.Length; i++)
        {
            mainButtons[i].text = mainButtonsText[i];
        }
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
        foreach(var button in mainButtons) button.gameObject.SetActive(false);
        foreach(var button in itemButtons) button.gameObject.SetActive(false);
        foreach(var panel in panels) panel.gameObject.SetActive(false);
        player.isControllable = true;
    }
        
}
