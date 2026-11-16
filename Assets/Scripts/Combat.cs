using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Player player;
    public CameraController cameraController;
    public BlackoutManager blackoutManager;
    public GameObject[] panels = new GameObject[5]; // 0 main 1 hp 2 item 3 magic 4 info
    public Enemy[] enemies = new Enemy[3];
    public GameObject[] cursors = new GameObject[4]; // 0 main 1 item 2 magic 3 target
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
    public TMP_Text[] Player_enemiesHP = new TMP_Text[4];
    public short[] hp = {100, 100, 100, 100};
    Color dmgColor = new Color(255, 0, 0, 160);
    enum Menu : byte
    {
        Main = 0,
        Item = 1,
        Magic = 2
    }
    byte menuType = (byte)Menu.Main;
    enum PlayerAction : byte
    {
        Attack = 10,
        Item = 20,
        Magic = 30
    }
    byte playerAction = 0;
    Vector2 ogPlayerPos;
    Vector3 ogCameraPos;
    WaitTimer waitTimer = new WaitTimer();
    Coroutine coroutine;
    Coroutine enemyCoroutine;
    public TMP_Text infoText;
    bool targeting = false;
    public float targetCursorOffset = 0f;
    int chosenMagic = 0;
    byte bonusAttack = 0;
    byte bonusMagic = 0;
    bool extraTurn = false;

    public float[] location = new float[2];

    void Awake()
    {
        HideAllPanels();
    }

    void Update()
    {
        if (combatActive)
        {
            if (!playerTurn && enemyCoroutine == null)
            {
                enemyCoroutine = StartCoroutine(EnemiesAction());
            }
            if (Input.GetKeyDown(KeyCode.F) && playerTurn) StartCoroutine(Action());
            if (Input.GetKeyDown(KeyCode.LeftShift)) OnlyMain();
            if (coroutine == null && enemyCoroutine == null) {
                if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerTurn) cursorInput = 1f;
                else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && playerTurn) cursorInput = -1f;
                else cursorInput = 0f;
            }
            if (menuType == (byte)Menu.Item || menuType == (byte)Menu.Magic) cursors[0].SetActive(false);
            else cursors[0].SetActive(true);
            ChangeCursorPos();
            DrawHP();
        }
    }

    IEnumerator ShowInfo(string text)
    {
        infoText.text = text;
        panels[4].SetActive(true);
        yield return StartCoroutine(waitTimer.Wait(2f));
        panels[4].SetActive(false);
        coroutine = null;
    }

    IEnumerator EnemiesAction()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        foreach(var e in enemies)
        {
            if (e == null) continue;
            e.Action();
            while (!e.dmgCalculated) yield return null;
            e.dmgCalculated = false;
            if (e.dmg == 255) {
                e.stop = true;
                coroutine = StartCoroutine(ShowInfo($"Critical miss from {e.enemyName}!"));
            }
            else if (e.dmg == 0) {
                coroutine = StartCoroutine(ShowInfo($"Miss from {e.enemyName}!"));
            }
            else {
                StartCoroutine(AttackPlayer(e.dmg));
                coroutine = StartCoroutine(ShowInfo($"{e.enemyName} dealt {e.dmg} damage to {PlayerData.Instance.playerName}!"));
            }
            e.dmg = 0;
            yield return StartCoroutine(waitTimer.Wait(2f));
        }
        playerTurn = true;
        enemyCoroutine = null;
    }

    IEnumerator AttackPlayer(byte dmg)
    {
        hp[0] -= dmg;
        Color ogPlayerColor = player.GetComponent<SpriteRenderer>().color;
        player.GetComponent<SpriteRenderer>().color = dmgColor;
        if (hp[0] <= 0) GameOver();
        yield return StartCoroutine(waitTimer.Wait(1f));
        player.GetComponent<SpriteRenderer>().color = ogPlayerColor;
    }

    IEnumerator AttackEnemy(byte dmg, int pos)
    {
        hp[pos+1] -= dmg;
        Color ogEnemyColor = enemies[pos].GetComponent<SpriteRenderer>().color;
        enemies[pos].GetComponent<SpriteRenderer>().color = dmgColor;
        if (hp[pos+1] <= 0) {
            Destroy(enemies[pos].gameObject);
            TargetEnemy(false);
            ChangeCursorPos();
            CheckIfWon();
        }
        yield return StartCoroutine(waitTimer.Wait(1f));
        if (enemies[pos] != null) enemies[pos].GetComponent<SpriteRenderer>().color = ogEnemyColor;


    }

    void GameOver()
    {
        
    }

    IEnumerator Action()
    {
        if (!targeting) {
            switch (menuType)
            {
                case (byte)Menu.Main:
                    if (cursorPos == 0)
                    {
                        TargetEnemy(true);
                        playerAction = (byte)PlayerAction.Attack;
                    }
                    else if (cursorPos == 1)
                    {
                        playerAction = (byte)PlayerAction.Magic;
                        ShowMagicPanel();
                    }
                    else if (cursorPos == 2)
                    {
                        ShowItemPanel();
                    }
                    else if (cursorPos == 3) StartCoroutine(EndBattle());
                    break;
                case (byte)Menu.Item:
                    Item curItem = PlayerData.Instance.playerItems.Find(o => o.itemName == possibleItems[cursorPos]);
                    if (curItem != null) UseItem(cursorPos);
                    else yield break;
                    break;
                case (byte)Menu.Magic:
                    chosenMagic = cursorPos;
                    TargetEnemy(true);
                    break;        
                default:
                    break;    
            }
        }
        else
        {
            if (coroutine != null) StopCoroutine(coroutine);
            if (playerAction == (byte)PlayerAction.Attack) {
                byte dmg = CalculatePlayerDmg();
                if (bonusAttack > 0) bonusAttack = 0;
                if (dmg == 0) {
                    coroutine = StartCoroutine(ShowInfo($"Miss from {PlayerData.Instance.playerName}!"));
                    targeting = false;
                    cursors[3].SetActive(false);
                    yield return StartCoroutine(EndPlayerTurn());
                }
                else if (dmg == 255)
                {
                    coroutine = StartCoroutine(ShowInfo($"Critical miss from {PlayerData.Instance.playerName}!"));
                    targeting = false;
                    cursors[3].SetActive(false);
                    yield return StartCoroutine(EndPlayerTurn());
                }
                else {
                    StartCoroutine(AttackEnemy(dmg, cursorPos));
                    coroutine = StartCoroutine(ShowInfo($"{PlayerData.Instance.playerName} dealt {dmg} damage to {enemies[cursorPos].enemyName}!"));
                    targeting = false;
                    cursors[3].SetActive(false);
                    yield return StartCoroutine(EndPlayerTurn());
                }
            }
            else if (playerAction == (byte)PlayerAction.Magic)
            {
                byte dmg = CalculatePlayerDmg(true, chosenMagic);
                if (bonusMagic > 0) bonusMagic = 0;
                StartCoroutine(AttackEnemy(dmg, cursorPos));
                coroutine = StartCoroutine(ShowInfo($"{PlayerData.Instance.playerName} dealt {dmg} to {enemies[cursorPos].enemyName}!"));
                targeting = false;
                cursors[3].SetActive(false);
                yield return StartCoroutine(EndPlayerTurn());
                chosenMagic = 0;
            }
            OnlyMain();
        }
        cursorPos = 0;
    }

    void DrawHP()
    {
        for(int i=0; i<Player_enemiesHP.Length; i++) {
            if (i==0) Player_enemiesHP[i].text = $"{PlayerData.Instance.playerName}";
            else {
                if (enemies[i-1] != null) Player_enemiesHP[i].text = enemies[i-1].enemyName;
            }
        }
        for(int i=0; i<allHP.Length; i++)
        {
            allHP[i].text = $"{hp[i]}%";
            if (i > 0 && enemies[i-1] == null) Player_enemiesHP[i].text = allHP[i].text = "";
        }
    }

    void TargetEnemy(bool show)
    {
        if (enemies[0] != null) cursorPos = 0;
        else if (enemies[1] != null) cursorPos = 1;
        else if (enemies[2] != null) cursorPos = 2;
        else return;
        targeting = show;
        cursors[3].SetActive(show);
        panels[3].SetActive(false);
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
        targeting = false;
        cursors[3].SetActive(false);
        for (int i=0; i<panels.Length; i++)
        {
            if (i == 2 || i == 3 || i == 4) panels[i].SetActive(false);
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
            magic[i].text = PlayerData.Instance.intelligence >= (i+1)*10 ? possibleMagic[i] : "";
            if (magic[i].text != "") availableMagic++; 
        }
        panels[3].SetActive(true);
    }

    public IEnumerator StartBattle()
    {
        player.isControllable = false;
        player.StopMoving();
        player.GetComponent<CharacterAnimation>().isRunning = false;
        cameraController.followPlayer = false;
        yield return StartCoroutine(blackoutManager.Fade(false));
        player.GetComponent<SpriteRenderer>().flipX = true;
        ogPlayerPos = player.transform.position;
        ogCameraPos = cameraController.transform.position;
        player.transform.position = new Vector2(location[0] + 4f, location[1]+2f);
        cameraController.transform.position = new Vector3(location[0], location[1], -10f);
        for(int i=0; i<enemies.Length; i++)
        {
            if (enemies[i] != null) enemies[i].transform.position = new Vector2(location[0]-4f, location[1]+2f - i*1.5f);
        }
        yield return StartCoroutine(waitTimer.Wait(1f));
        panels[0].SetActive(true);
        panels[1].SetActive(true);
        StartCoroutine(blackoutManager.Fade(true));
        menuType = (byte)Menu.Main;
        combatActive = true;
        playerTurn = true;
    }

    void HideAllPanels()
    {
        foreach (var panel in panels) panel.SetActive(false);
        cursors[3].SetActive(false);
    }

    void ChangeCursorPos(bool reset = false) 
    {
            int ogPos = cursorPos;
            if (reset) cursorPos = 0;
            else {
                if (cursorInput > 0) cursorPos--;
                else if (cursorInput < 0) cursorPos++;
            }
            if (!targeting) {
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
            else
            {
                if (enemies[0] != null) cursorPos = 0;
                else if (enemies[1] != null) cursorPos = 1;
                else  cursorPos = 2;
                cursorPos = Math.Clamp(cursorPos, 0, enemies.Length-1);
                if (enemies[cursorPos] == null)
                {
                    if (cursorInput > 0) cursorPos--;
                    else if (cursorInput < 0) cursorPos++;
                    cursorPos = Math.Clamp(cursorPos, 0, enemies.Length-1);
                    if (enemies[cursorPos] == null) cursorPos = ogPos;
                }
                
                if (enemies[cursorPos] != null) {
                    cursors[3].transform.position = new Vector2(cursors[3].transform.position.x, enemies[cursorPos].transform.position.y - targetCursorOffset);
                }
            }
    }

    void CheckIfWon()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        foreach(var e in enemies) if (e!=null) return;
        int dice = UnityEngine.Random.Range(1, 101);
        Debug.Log($"{dice}");
        if (dice <= 50)
        {
            string itName = "";
            dice = UnityEngine.Random.Range(1, 101);
            if (dice <= 33)
            {
                dice = UnityEngine.Random.Range(0, 4);                
                if (dice == 0) itName = "DEX Up";
                else if (dice == 1) itName = "STR Up";
                else if (dice == 2) itName = "INT Up";
                else itName = "PER Up";
            }
            else if (dice > 33 && dice <= 67)
            {
                dice = UnityEngine.Random.Range(0, 3);
                if (dice == 0) itName = "Attack Boost";
                else if (dice == 1) itName = "Magic Boost";
                else itName = "Extra Turn";
            }
            else
            {
                itName = "Potion";
            }
            PlayerData.Instance.AddItem(itName);
            StartCoroutine(EndBattle(true, itName));

        }
        else StartCoroutine(EndBattle());
    }
    IEnumerator EndBattle(bool gotItem = false, string itName = null)
    {
        if (gotItem) yield return StartCoroutine(ShowInfo($"Obtained x1 of {itName}"));
        yield return StartCoroutine(blackoutManager.Fade(false));
        combatActive = false;
        HideAllPanels();
        player.transform.position = ogPlayerPos;
        cameraController.transform.position = ogCameraPos;
        cameraController.followPlayer = true;
        player.isControllable = true;
        yield return StartCoroutine(waitTimer.Wait(1f));
        StartCoroutine(blackoutManager.Fade(true));
    }

    byte CalculatePlayerDmg(bool magic = false, int spell = 0)
    {
        byte dmg;
        int dice = UnityEngine.Random.Range(1,101);
        if (!magic) {
            if (dice <= 75)
            {
                dmg = (byte)Math.Clamp(PlayerData.Instance.strength + bonusAttack, 0, 100);
            }
            else if (dice > 75 && dice <= 80)
            {
                dmg = (byte)Math.Clamp((int)Math.Round((PlayerData.Instance.strength+bonusAttack)*1.5f), 0, 100);
            }
            else if (dice > 80 && dice <= 97) dmg = 0;
            else dmg = 255;
        }
        else
        {
            if (spell == 0)
            {
                dmg = (byte)Math.Clamp(PlayerData.Instance.intelligence + bonusMagic, 0, 100);
            }
            else if (spell == 1)
            {
                dmg = (byte)Math.Clamp((int)Math.Round((PlayerData.Instance.intelligence+bonusMagic)*1.2f), 0, 100);
            }
            else if (spell == 2)
            {
                dmg = (byte)Math.Clamp((int)Math.Round((PlayerData.Instance.intelligence+bonusMagic)*1.4f), 0, 100);
            }
            else if (spell == 3)
            {
                dmg = (byte)Math.Clamp((int)Math.Round((PlayerData.Instance.intelligence+bonusMagic)*1.6f), 0, 100);
            }
            else dmg = 0;           
        }
        return dmg;
    }

    IEnumerator EndPlayerTurn()
    {
        yield return StartCoroutine(waitTimer.Wait(2f));
        if (!extraTurn) playerTurn = false;
        else extraTurn = false;
        
    }
    void UseItem(int num)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        if (num == 0)
        {
            hp[0] = (byte)Math.Clamp(hp[0]+10,0, 100);
            StartCoroutine(ShowInfo($"Added 10HP to {PlayerData.Instance.playerName}"));
            StartCoroutine(EndPlayerTurn());
        }
        else if (num == 1)
        {
            bonusAttack = 5;
            StartCoroutine(ShowInfo($"Activated Attack Boost!"));
        }
        else if (num == 2)
        {
            bonusMagic = 5;
            StartCoroutine(ShowInfo($"Activated Magic Boost!"));
        }
        else if (num == 3)
        {
            extraTurn = true;
            StartCoroutine(ShowInfo($"Added extra turn to {PlayerData.Instance.playerName}"));
        }
        PlayerData.Instance.RemoveItem(possibleItems[num], 1);
        OnlyMain();
    }
}
