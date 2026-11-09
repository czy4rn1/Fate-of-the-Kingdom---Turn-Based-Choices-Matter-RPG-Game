using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    InitInteraction initInteraction;
    public GameObject[] shopPanels;
    public string[] itemNames = {"Stat Boost", "DEX Up", "STR Up", "INT Up", "PER Up" , "EQU"};
    public  byte[] eqStats = new byte[4];
    int itemInShop = 0;
    public string eqType;
    public short[] itemCost = {500, 100, 1500};
    public TMP_Text[] mainButtons;
    public TMP_Text[] actionButtons;
    public TMP_Text infoText;
    bool itemChosen = false;
    WaitTimer waitTimer = new WaitTimer();
    bool eqBought = false;

    int cursorPos = 0;
    int chosenItem = -10;
    float cursorInput = 0f;
    public float cursorOffset;
    public GameObject cursor;
    public GameObject actionCursor;
    public float actionCursorOffset;
    bool shopActive = false;
    Coroutine coroutine;
    
    void Start()
    {
        initInteraction = GetComponent<InitInteraction>();
    }

    
    void Update()
    {
        if (!shopActive) return;
        if (Input.GetKeyDown(KeyCode.LeftShift)) GoBackToMain();
        if(Input.GetKeyDown(KeyCode.F)) Action();
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) cursorInput = 1f;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) cursorInput = -1f;
        else cursorInput = 0f;
        ChangeCursorPos();

    }

    void Action()
    {
        if (!itemChosen)
        {
            itemChosen = true;
            chosenItem = cursorPos;
            ChangeCursorPos(true);
            if (chosenItem <= 2) {                              
                cursor.SetActive(false);
                actionCursor.SetActive(true);
                shopPanels[1].SetActive(true);
            }
            else DeactivateMenu();
        }
        else
        {
            if (cursorPos  == 0) {
                if (coroutine != null) {
                    StopCoroutine(coroutine);
                    coroutine = null;
                    shopPanels[2].SetActive(false);
                }
                short cost = itemCost[chosenItem];
                if (PlayerData.Instance.money >= cost)
                {
                    if (chosenItem == 2) {
                        PlayerData.Instance.AddEquipment(itemNames[itemNames.Length-1], eqType, eqStats);
                        eqBought = true;
                        mainButtons[2].text = " ";
                        coroutine = StartCoroutine(ShowInfo($"Bought {itemNames[itemNames.Length-1]}!"));
                    }
                    else if (chosenItem == 0) {
                        PlayerData.Instance.AddItem(itemNames[chosenItem]);
                        coroutine = StartCoroutine(ShowInfo($"Bought {itemNames[chosenItem]}!"));
                    }
                    else if (chosenItem == 1) {
                        PlayerData.Instance.AddItem(itemNames[itemInShop]);
                        coroutine = StartCoroutine(ShowInfo($"Bought {itemNames[itemInShop]}!"));
                    }
                    PlayerData.Instance.money -= cost;
                    mainButtons[4].text = $"Player Money: {PlayerData.Instance.money}";
                }
                else coroutine = StartCoroutine(ShowInfo("Not enough money!"));
            }
            else GoBackToMain();
        }
    }

    void GoBackToMain()
    {
        shopPanels[1].SetActive(false);
        shopPanels[2].SetActive(false);
        chosenItem = -10;
        itemChosen = false;
        actionCursor.SetActive(false);
        cursor.SetActive(true);
        ChangeCursorPos(true);
    }

    public void ActivateShop()
    {
        foreach(var button in mainButtons) button.gameObject.SetActive(true);
        mainButtons[0].text = itemNames[0] + $" {itemCost[0]}";
        itemInShop = UnityEngine.Random.Range(1, 5);
        mainButtons[1].text = itemNames[itemInShop] + $" {itemCost[1]}";
        if (PlayerData.Instance.equipment.Find(o => o.name == itemNames[itemNames.Length-1]) == null) {
            mainButtons[2].text = itemNames[itemNames.Length-1] + $" {itemCost[2]}";
        }
        else
        {
            eqBought = true;
            mainButtons[2].text = "";
        }
        mainButtons[4].text = $"Player Money: {PlayerData.Instance.money}";
        mainButtons[3].text = "Leave";

        actionButtons[0].text = "Buy";
        actionButtons[1].text = "Leave";
        ChangeCursorPos(true);
        shopPanels[0].SetActive(true);
        itemChosen = false;
        cursor.SetActive(true);
        infoText.fontSize = 4;
        StartCoroutine(finalizeActivation());
    }

    IEnumerator finalizeActivation()
    {
        yield return StartCoroutine(waitTimer.Wait(0.16f));
        shopActive = true;
    }

    void ChangeCursorPos(bool reset = false) 
    {
            if (reset) cursorPos = 0;
            else {
                if (cursorInput > 0) {
                    cursorPos--;
                    if (eqBought && cursorPos == 2) cursorPos--;
                }
                else if (cursorInput < 0) {
                    cursorPos++;
                    if (eqBought && cursorPos == 2) cursorPos++;
                }
            }
            if (!itemChosen) {
                cursorPos = Math.Clamp(cursorPos, 0, mainButtons.Length-2);
                cursor.transform.localPosition = new Vector2(cursor.transform.localPosition.x, mainButtons[cursorPos].transform.localPosition.y + cursorOffset);
            }
            else
            {
                cursorPos = Math.Clamp(cursorPos, 0, actionButtons.Length-1);
                actionCursor.transform.localPosition = new Vector2(actionCursor.transform.localPosition.x, actionButtons[cursorPos].transform.localPosition.y + actionCursorOffset);
            }

    }

    IEnumerator ShowInfo(string info)
    {
        shopPanels[1].SetActive(false);
        foreach(var button in mainButtons) button.gameObject.SetActive(true);
        ChangeCursorPos(true);
        itemChosen = false;
        cursor.SetActive(true);
        ChangeCursorPos(true);
        shopPanels[2].SetActive(true);
        infoText.text = info;
        yield return StartCoroutine(waitTimer.Wait(3f));
        shopPanels[2].SetActive(false);
        coroutine = null;
    }

    void DeactivateMenu()
    {
        foreach (var button in mainButtons) button.gameObject.SetActive(false);
        foreach (var panel in shopPanels) panel.SetActive(false);
        itemChosen = false;
        chosenItem = -10;
        ChangeCursorPos(true);
        initInteraction.CloseInteraction(0);
        shopActive = false;
    }


}
