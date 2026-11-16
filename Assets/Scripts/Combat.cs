using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Player player;
    public CameraController cameraController;
    public BlackoutManager blackoutManager;
    public GameObject[] panels = new GameObject[4]; // 0 main 1 hp 2 item 3 magic
    public GameObject[] enemies = new GameObject[3];
    public GameObject[] cursors = new GameObject[3]; // 0 main 1 item 2 magic
    public TMP_Text[] items = new TMP_Text[4];
    public TMP_Text[] magic = new TMP_Text[4];
    public TMP_Text[] mainButtons = new TMP_Text[4];
    public string[] possibleItems = {"Potion", "Attack Boost", "Magic Boost", "Extra Turn"};
    public string[] possibleMagic = {"Fire", "Blizzard", "Thunder", "Bubble"};
    public int cursorPos = 0;
    public bool combatActive = false;
    float cursorInput = 0f;
    public float cursorOffset;
    byte availableMagic = 0;
    bool playerTurn = false;
    public TMP_Text[] allHP = new TMP_Text[4];
    public TMP_Text[] enemiesHP = new TMP_Text[3];
    public byte[] hp = {100, 100, 100, 100};
    enum Menu : byte
    {
        Main = 0,
        Item = 1,
        Magic = 2
    }
    byte menuType = (byte)Menu.Main;
    Vector2 ogPlayerPos;
    Vector3 ogCameraPos;
    WaitTimer waitTimer = new WaitTimer();

    void Awake()
    {
        HideAllPanels();
    }

    void Update()
    {
        if (combatActive)
        {
            if (Input.GetKeyDown(KeyCode.F) && playerTurn) Action();
            if (Input.GetKeyDown(KeyCode.LeftShift)) OnlyMain();
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerTurn) cursorInput = 1f;
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && playerTurn) cursorInput = -1f;
            else cursorInput = 0f;
            ChangeCursorPos();
            DrawHP();
        }
    }

    void Action()
    {
        switch (menuType)
        {
            case (byte)Menu.Main:
                if (cursorPos == 0)
                {
                    
                }
                else if (cursorPos == 1)
                {
                    ShowMagicPanel();
                }
                else if (cursorPos == 2)
                {
                    ShowItemPanel();
                }
                else if (cursorPos == 3) StartCoroutine(EndBattle());
                break;
            case (byte)Menu.Item:
                OnlyMain();
                break;
            case (byte)Menu.Magic:
                OnlyMain();
                break;        
            default:
                break;    
        }
    }

    void DrawHP()
    {
        for(int i=0; i<allHP.Length; i++)
        {
            allHP[i].text = $"{hp[i]}%";
            if (i > 0 && enemies[i-1] == null) enemiesHP[i-1].text = allHP[i].text = "";
        }
    }

    void ShowItemPanel()
    {
        menuType = (byte)Menu.Item;
        for(int i=0; i<possibleItems.Length; i++)
        {
            Item curItem = PlayerData.Instance.playerItems.Find(o => o.itemName == possibleItems[i]);
            if (curItem == null) continue;
            items[i].text = curItem.count.ToString();
        }
        cursorPos = 0;
        panels[2].SetActive(true);
    }

    void OnlyMain()
    {
        for (int i=0; i<panels.Length; i++)
        {
            if (i == 1 || i == 2) panels[i].SetActive(false);
        }
        cursorPos = 0;
        menuType = (byte)Menu.Main;
    }

    void ShowMagicPanel()
    {
        menuType = (byte)Menu.Magic;
        availableMagic = 0;
        cursorPos = 0;
        for(int i=0; i<possibleMagic.Length; i++)
        {
            magic[i].text = PlayerData.Instance.intelligence >= (i+1)*10 ? possibleItems[i] : "";
            if (magic[i].text != "") availableMagic++; 
        }
        panels[3].SetActive(true);
    }

    public IEnumerator StartBattle()
    {
        player.isControllable = false;
        player.StopMoving();
        cameraController.followPlayer = false;
        yield return StartCoroutine(blackoutManager.Fade(false));
        ogPlayerPos = player.transform.position;
        ogCameraPos = cameraController.transform.position;
        player.transform.position = new Vector2(transform.position.x + 2f, transform.position.y+10f);
        cameraController.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        yield return StartCoroutine(waitTimer.Wait(1f));
        panels[0].SetActive(true);
        panels[1].SetActive(true);
        StartCoroutine(blackoutManager.Fade(true));
        combatActive = true;
    }

    void HideAllPanels()
    {
        foreach (var panel in panels) panel.SetActive(false);
    }

    void ChangeCursorPos(bool reset = false) 
    {
            if (reset) cursorPos = 0;
            else {
                if (cursorInput > 0) cursorPos--;
                else if (cursorInput < 0) cursorPos++;
            }
            if (menuType == (byte)Menu.Main) {
                cursorPos = Math.Clamp(cursorPos, 0, mainButtons.Length-1);
                cursors[0].transform.localPosition = new Vector2(cursors[0].transform.localPosition.x, mainButtons[cursorPos].transform.localPosition.y - cursorOffset);
            }
            else if (menuType == (byte)Menu.Item) {
                cursorPos = Math.Clamp(cursorPos, 0, items.Length-1);
                cursors[1].transform.localPosition = new Vector2(cursors[1].transform.localPosition.x, items[cursorPos].transform.localPosition.y - cursorOffset);
            }
            else if (menuType == (byte)Menu.Magic) {
                cursorPos = Math.Clamp(cursorPos, 0, availableMagic-1);
                cursors[2].transform.localPosition = new Vector2(cursors[2].transform.localPosition.x, magic[cursorPos].transform.localPosition.y - cursorOffset);
            }
    }

    IEnumerator EndBattle()
    {
        yield return StartCoroutine(blackoutManager.Fade(false));
        HideAllPanels();
        player.transform.position = ogPlayerPos;
        cameraController.transform.position = ogCameraPos;
        yield return StartCoroutine(waitTimer.Wait(1f));
        StartCoroutine(blackoutManager.Fade(true));
    }
}
