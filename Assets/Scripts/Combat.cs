using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    public GameObject combatArena;
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
    bool playerTurn = true;
    public TMP_Text[] allHP = new TMP_Text[4];
    public TMP_Text[] Player_enemiesHP = new TMP_Text[4];
    public short[] hp = {100, 100, 100, 100};
    Color dmgColor = new Color(255, 0, 0, 255);
    bool playerBusy = false;
    public bool disableEscape = false;
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
    bool battleEnding = false;
    bool gameOver = false;

    public float[] location = new float[2];

    void Awake()
    {
        combatArena.SetActive(false);
        playerTurn = true;
        HideAllPanels();
    }

    void Update()
    {
        if (combatActive)
        {
            if (!playerTurn && enemyCoroutine == null && blackoutManager.curAlpha == 0f)
            {
                enemyCoroutine = StartCoroutine(EnemiesAction());
            }
            if (Input.GetKeyDown(KeyCode.F) && playerTurn && !playerBusy) StartCoroutine(Action());
            if (Input.GetKeyDown(KeyCode.LeftShift) && !playerBusy) OnlyMain();
            if (coroutine == null && enemyCoroutine == null) {
                if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerTurn && !playerBusy) cursorInput = 1f;
                else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && playerTurn && !playerBusy) cursorInput = -1f;
                else cursorInput = 0f;
            }
            else cursorInput = 0f;
            if (menuType == (byte)Menu.Item || menuType == (byte)Menu.Magic || battleEnding) cursors[0].SetActive(false);
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
                if (!battleEnding) coroutine = StartCoroutine(ShowInfo($"{e.enemyName} dealt {e.dmg} damage to {PlayerData.Instance.playerName}!"));
            }
            e.dmg = 0;
            yield return StartCoroutine(waitTimer.Wait(2f));
            if (gameOver) {
                enemyCoroutine = null;
                playerTurn = false;
                yield break;
            }
        }
        playerTurn = true;
        enemyCoroutine = null;
    }

    IEnumerator AttackPlayer(short dmg)
    {
        hp[0] -= dmg;
        Color ogPlayerColor = player.GetComponent<SpriteRenderer>().color;
        player.GetComponent<SpriteRenderer>().color = dmgColor;
        if (hp[0] <= 0) {
            player.GetComponent<SpriteRenderer>().color = ogPlayerColor;
            GameOver();
            yield break;
        }
        yield return StartCoroutine(waitTimer.Wait(1f));
        player.GetComponent<SpriteRenderer>().color = ogPlayerColor;
    }

    IEnumerator AttackEnemy(short dmg, int pos)
    {
        hp[pos+1] -= dmg;
        Color ogEnemyColor = enemies[pos].GetComponent<SpriteRenderer>().color;
        enemies[pos].GetComponent<SpriteRenderer>().color = dmgColor;
        if (hp[pos+1] <= 0) {
            Enemy dead = enemies[pos];
            enemies[pos] = null;
            if (dead.enemyName != "Garen") Destroy(dead.gameObject);
            else
            {
                dead.GetComponent<SpriteRenderer>().color = ogEnemyColor;
            }
            TargetEnemy(false);
            ChangeCursorPos();
        }
        yield return StartCoroutine(waitTimer.Wait(1f));
        if (enemies[pos] != null) enemies[pos].GetComponent<SpriteRenderer>().color = ogEnemyColor;


    }

    void GameOver()
    {
        if (battleEnding) return;
        battleEnding = true;
        gameOver = true;
        playerBusy = true;
        combatActive = false;
        StopAllCoroutines();
        HideAllPanels();
        cursors[0].SetActive(false);
        cursors[3].SetActive(false);
        player.gameObject.SetActive(false);
        StartCoroutine(Reload());
        
    }

    IEnumerator Reload()
    {
        yield return coroutine = StartCoroutine(ShowInfo("You Died"));
        yield return StartCoroutine(blackoutManager.Fade(false));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                        if (PlayerData.Instance.intelligence >= 10) {
                            playerAction = (byte)PlayerAction.Magic;
                            ShowMagicPanel();
                        }
                    }
                    else if (cursorPos == 2)
                    {
                        ShowItemPanel();
                    }
                    else if (cursorPos == 3) {
                        playerBusy = true;
                        if (disableEscape)
                        {
                            playerBusy = false;
                            cursorPos = 0;
                            if (coroutine != null) StopCoroutine(coroutine);
                            coroutine = StartCoroutine(ShowInfo("Cant's escape this battle!"));
                        }
                        else
                        {
                            int dice = UnityEngine.Random.Range(1,101);
                            if (dice <= 33) {
                                StartCoroutine(EndBattle());
                                yield break;
                            }
                            else {
                                cursorPos = 0;
                                if (coroutine != null) StopCoroutine(coroutine);
                                coroutine = StartCoroutine(ShowInfo("Escape failed!"));
                            }
                            yield return StartCoroutine(EndPlayerTurn());
                        }    
                    }
                    break;
                case (byte)Menu.Item:
                    Item curItem = PlayerData.Instance.playerItems.Find(o => o.itemName == possibleItems[cursorPos]);
                    if (curItem != null) StartCoroutine(UseItem(cursorPos));
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
                short dmg = CalculatePlayerDmg(cursorPos);
                if (bonusAttack > 0) bonusAttack = 0;
                if (dmg == 0) {
                    cursorPos = 0;
                    coroutine = StartCoroutine(ShowInfo($"Miss from {PlayerData.Instance.playerName}!"));
                    targeting = false;
                    cursors[3].SetActive(false);
                    yield return StartCoroutine(EndPlayerTurn());
                }
                else if (dmg == 255)
                {
                    cursorPos = 0;
                    coroutine = StartCoroutine(ShowInfo($"Critical miss from {PlayerData.Instance.playerName}!"));
                    targeting = false;
                    cursors[3].SetActive(false);
                    yield return StartCoroutine(EndPlayerTurn());
                }
                else {
                    string enemyName = enemies[cursorPos].enemyName;
                    StartCoroutine(AttackEnemy(dmg, cursorPos));
                    coroutine = StartCoroutine(ShowInfo($"{PlayerData.Instance.playerName} dealt {dmg} damage to {enemyName}!"));
                    targeting = false;
                    cursors[3].SetActive(false);
                    cursorPos = 0;
                    yield return StartCoroutine(EndPlayerTurn());
                }
            }
            else if (playerAction == (byte)PlayerAction.Magic)
            {
                short dmg = CalculatePlayerDmg(cursorPos, true, chosenMagic);
                if (bonusMagic > 0) bonusMagic = 0;
                string enemyName = enemies[cursorPos].enemyName;
                StartCoroutine(AttackEnemy(dmg, cursorPos));
                coroutine = StartCoroutine(ShowInfo($"{PlayerData.Instance.playerName} dealt {dmg} to {enemyName}!"));
                targeting = false;
                cursors[3].SetActive(false);
                cursorPos = 0;
                yield return StartCoroutine(EndPlayerTurn());
                chosenMagic = 0;
            }
            OnlyMain();
            if (!HasAliveEnemy()) {
                playerBusy = true;
                StartCoroutine(CheckIfWon());
            }
        }
        if (!targeting) cursorPos = 0;
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
        targeting = show;
        cursors[3].SetActive(show);
        panels[3].SetActive(false);
        if (show)
        {
            cursorPos = FirstAlive();
            cursorPos = Math.Clamp(cursorPos, 0, enemies.Length-1);
        }
    }

    void ShowItemPanel()
    {
        menuType = (byte)Menu.Item;
        for(int i=0; i<possibleItems.Length; i++)
        {
            Item curItem = PlayerData.Instance.playerItems.Find(o => o.itemName == possibleItems[i]);
            if (curItem == null) {
                items[i].text = "0"; 
                continue;
            }
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
        for(int i=0; i<hp.Length; i++)
        {
            hp[i] = 100;
        }
        int rand = UnityEngine.Random.Range(1, 101);
        if (rand <= 50) playerTurn = true;
        else playerTurn = false;
        targeting = false;
        extraTurn = false;
        bonusAttack = 0;
        bonusMagic = 0;
        playerBusy = false;
        gameOver = false;

        player.isControllable = false;
        player.StopMoving();
        player.GetComponent<CharacterAnimation>().isRunning = false;
        cameraController.followPlayer = false;
        yield return StartCoroutine(blackoutManager.Fade(false));
        combatArena.SetActive(true);
        player.GetComponent<SpriteRenderer>().flipX = true;
        ogPlayerPos = player.transform.position;
        ogCameraPos = cameraController.transform.position;
        player.transform.position = new Vector2(location[0] + 4f, location[1]+0.8f);
        cameraController.transform.position = new Vector3(location[0], location[1], -10f);
        switch (HowManyEnemies())
        {
            case 1:
            enemies[0].transform.position = new Vector2(location[0] - 4f, location[1]-0.25f);
                break;
            case 2:
            enemies[0].transform.position = new Vector2(location[0]-4f, location[1] + 0.4f);
            enemies[1].transform.position = new Vector2(location[0]-4f, location[1]-1.5f);
                break;
            case 3:
            enemies[0].transform.position = new Vector2(location[0]-4f, location[1]+0.85f);
            enemies[1].transform.position = new Vector2(location[0]+location[0], location[1]-0.5f);
            enemies[2].transform.position = new Vector2(location[0]-4f, location[1]-2.4f);
                break;
            default: break;
        }
        foreach (var e in enemies) {
            if (e != null) {
                e.GetComponent<SpriteRenderer>().flipX = false;
                e.gameObject.SetActive(true);
            }
        }
        yield return StartCoroutine(waitTimer.Wait(1f));
        panels[0].SetActive(true);
        panels[1].SetActive(true);
        StartCoroutine(blackoutManager.Fade(true));
        menuType = (byte)Menu.Main;
        combatActive = true;
        cursors[0].SetActive(true);
    }

    void HideAllPanels()
    {
        foreach (var panel in panels) panel.SetActive(false);
        cursors[3].SetActive(false);
    }

    void ChangeCursorPos(bool reset = false) 
    {
        if (reset) cursorPos = 0;
        else {
            if (!targeting) {
                if (cursorInput > 0) cursorPos--;
                else if (cursorInput < 0) cursorPos++;
            }
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
            if (cursorInput != 0f)
            {
                if (cursorInput > 0)
                {
                    if (cursorPos == 0)
                    {
                        cursorPos = 0;
                    }
                    else if (cursorPos == 1)
                    {
                        if (enemies[0] != null) cursorPos = 0;
                    }
                    else if (cursorPos == 2)
                    {
                        if (enemies[1] != null) cursorPos = 1;
                        else if (enemies[0] == null) cursorPos = 0;
                    }
                }
                else if (cursorInput < 0)
                {
                    if (cursorPos == 0)
                    {
                        if (enemies[1] != null) cursorPos = 1;
                        else if (enemies[2] != null) cursorPos = 2;
                    }
                    else if (cursorPos == 1)
                    {
                        if (enemies[2] != null) cursorPos = 2;
                    }
                    else if (cursorPos == 2)
                    {
                        cursorPos = 2;
                    }
                }
            }
            cursorPos = Math.Clamp(cursorPos, 0, enemies.Length-1);
            if (enemies[cursorPos] != null)
            {
                cursors[3].transform.position = new Vector2(cursors[3].transform.position.x, enemies[cursorPos].transform.position.y - targetCursorOffset);
            }
        }
    }

    IEnumerator CheckIfWon()
    {
        foreach(var e in enemies) if (e!=null) yield break;
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
            while (coroutine != null) yield return null;
            StartCoroutine(EndBattle(true, itName));

        }
        else StartCoroutine(EndBattle());
    }
    IEnumerator EndBattle(bool gotItem = false, string itName = null)
    {
        battleEnding = true;
        playerBusy = true;
        disableEscape = false;
        cursors[0].SetActive(false);
        if (gotItem) yield return StartCoroutine(ShowInfo($"Obtained x1 of {itName}"));
        yield return StartCoroutine(blackoutManager.Fade(false));
        foreach (var e in enemies) if (e!=null) Destroy(e.gameObject);
        combatArena.SetActive(false);
        combatActive = false;
        HideAllPanels();
        player.transform.position = ogPlayerPos;
        cameraController.transform.position = ogCameraPos;
        cameraController.followPlayer = true;
        player.isControllable = true;
        yield return StartCoroutine(waitTimer.Wait(1f));
        StartCoroutine(blackoutManager.Fade(true));
        playerBusy = false;
        battleEnding = false;
        gameOver = false;
    }

    short CalculatePlayerDmg(int enemy, bool magic = false, int spell = 0)
    {
        short dmg;
        int dice = UnityEngine.Random.Range(1,101);
        if (!magic) {
            if (dice <= 75)
            {
                dmg = (short)Math.Clamp(PlayerData.Instance.strength + bonusAttack, 0, 100);
                Debug.Log($"{dmg}");
                float divider = (100.0f - enemies[enemy].stats[0])/100.0f;
                Debug.Log($"{divider}");
                dmg = (short)Math.Round((float)(dmg*divider));
                Debug.Log($"{dmg}");
            }
            else if (dice > 75 && dice <= 80)
            {
                dmg = (short)Math.Clamp((int)Math.Round((PlayerData.Instance.strength+bonusAttack)*1.5f), 0, 30);
            }
            else if (dice > 80 && dice <= 97) dmg = 0;
            else dmg = 255;
        }
        else
        {
            if (spell == 0)
            {
                dmg = (short)Math.Clamp(PlayerData.Instance.intelligence + bonusMagic, 0, 30);
            }
            else if (spell == 1)
            {
                dmg = (short)Math.Clamp((int)Math.Round((PlayerData.Instance.intelligence+bonusMagic)*1.2f), 0, 30);
            }
            else if (spell == 2)
            {
                dmg = (short)Math.Clamp((int)Math.Round((PlayerData.Instance.intelligence+bonusMagic)*1.4f), 0, 30);
            }
            else if (spell == 3)
            {
                dmg = (short)Math.Clamp((int)Math.Round((PlayerData.Instance.intelligence+bonusMagic)*1.6f), 0, 30);
            }
            else dmg = 0;           
        }
        return dmg;
    }

    IEnumerator EndPlayerTurn()
    {
        playerBusy = true;
        yield return StartCoroutine(waitTimer.Wait(2f));
        if (!extraTurn) playerTurn = false;
        else extraTurn = false;
        playerBusy = false;
        
    }
    IEnumerator UseItem(int num)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        OnlyMain();
        if (num == 0)
        {
            hp[0] = (byte)Math.Clamp(hp[0]+10,0, 100);
            StartCoroutine(ShowInfo($"Added 10HP to {PlayerData.Instance.playerName}"));
            yield return StartCoroutine(EndPlayerTurn());
        }
        else if (num == 1)
        {
            bonusAttack = 10;
            playerBusy = true;
            yield return StartCoroutine(ShowInfo($"Activated Attack Boost!"));
            playerBusy = false;
        }
        else if (num == 2)
        {
            bonusMagic = 10;
            playerBusy = true;
            yield return StartCoroutine(ShowInfo($"Activated Magic Boost!"));
            playerBusy = false;
        }
        else if (num == 3)
        {
            extraTurn = true;
            playerBusy = true;
            yield return StartCoroutine(ShowInfo($"Added Extra Turn to {PlayerData.Instance.playerName}"));
            playerBusy = false;
        }
        PlayerData.Instance.RemoveItem(possibleItems[num], 1);
    }

    int FirstAlive()
    {
        for (int i=0; i<enemies.Length; i++) if (enemies[i] != null) return i;
        return 0;
    }

    bool HasAliveEnemy()
    {
        for (int i=0; i<enemies.Length; i++) if (enemies[i] != null) return true;
        return false;
    }

    int HowManyEnemies()
    {
        int count = 0;
        for(int i=0; i<enemies.Length; i++)
        {
            if (enemies[i] != null) count++;
        }
        return count;
    }
}
