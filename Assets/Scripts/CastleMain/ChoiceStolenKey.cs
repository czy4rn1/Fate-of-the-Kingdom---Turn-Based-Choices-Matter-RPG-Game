using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChoiceStolenKey : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Player player;
    bool choiceEnded = false;
    public byte req_str = 20;
    public byte req_dex = 17;
    public byte req_dex2 = 16;
    public PlayableDirector failedOutcome;
    public PlayableDirector str_success;
    public PlayableDirector dex_success;
    public PlayableDirector dex2_success;
    public PlayableDirector str_fail;
    public GameObject[] guards;

    void Start()
    {
        player.isControllable = false;
    }

    void Update()
    {
        if (!choiceEnded && !dialogueManager.dialogueActive)
        {
            dialogueManager.ShowDialogue("There is someone coming! What will you do?\n" + 
            $"1. [STR {PlayerData.Instance.strength}/{req_str}] Scare them off\n" + 
            $"2. [DEX {PlayerData.Instance.dexterity}/{req_dex}] Quickly hide in a barrel\n" +
            $"3. [DEX {PlayerData.Instance.dexterity}/{req_dex2}] Throw a torch\n" + 
            "4. Attack them with your bare hands", 
            false, 4, true, OnChosenCommand);
        }   
    }

    public void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            if (PlayerData.Instance.strength >= req_str)
            {
                dialogueManager.timelineDirector = str_success;
                str_success.Play();
                str_success = null;
            }
            else
            {
                dialogueManager.timelineDirector = str_fail;
                str_fail.Play();
                str_fail = null;
                //SceneManager.LoadScene("CombatCastle", LoadSceneMode.Additive);
            }
        }
        else if (command == 1)
        {
            if (PlayerData.Instance.dexterity >= req_dex)
            {
                StartCoroutine(DexSuccess());
            }
            else
            {
                dialogueManager.timelineDirector = failedOutcome;
                failedOutcome.Play();
                failedOutcome = null;
                //SceneManager.LoadScene("CombatCastle", LoadSceneMode.Additive);
            }
        }
        else if (command == 2)
        {
            if (PlayerData.Instance.dexterity >= req_dex2)
            {
                WorldState.Instance.castleFire = true;
                dialogueManager.timelineDirector = dex2_success;
                dex2_success.Play();
                dex2_success = null;
            }
            else
            {
                dialogueManager.timelineDirector = failedOutcome;
                failedOutcome.Play();
                failedOutcome = null;
                //SceneManager.LoadScene("CombatCastle", LoadSceneMode.Additive);
            }
        }
        else if (command == 3)
        {
            //SceneManager.LoadScene("CombatCastle", LoadSceneMode.Additive);
        }
        foreach (GameObject gob in guards)
        {
            gob.SetActive(false);
        }
        str_success = null;
        dex2_success = null;
        failedOutcome = null;
        choiceEnded = true;
        gameObject.SetActive(false);
    }

    public void CloseDialogue(int nothing)
    {
        player.isControllable = true;
    }

    private IEnumerator DexSuccess()
    {
        dialogueManager.timelineDirector = dex_success;
        if (dex_success != null)
        {
            dex_success.Play();
        }
        yield return null;
        while (dex_success.state == PlayState.Playing)
        {
            yield return null;
        }
        PlayerData.Instance.AddItem("Potion", 5);
        dialogueManager.ShowDialogue("Found x5 of Potion", true, 0, true, CloseDialogue);
        dex_success = null;
    }
}
