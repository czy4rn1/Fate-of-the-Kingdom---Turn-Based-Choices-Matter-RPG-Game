using System.Collections;
using UnityEngine;

public class AhorgisState : MonoBehaviour
{
    public GameObject[] boxPlaces = new GameObject[4];
    public GameObject[] boxes = new GameObject[4];
    public GameObject bridge;
    public BoxCollider2D bc;
    public PlayDialogueLines dialoguePlayer;
    public Player player;
    public string[] dialogue;
    public BlackoutManager blackoutManager;
    public MoveToCutscene ponter;
    public GameObject trident;
    public GameObject Volsen;

    void Start()
    {
        Volsen.SetActive(false);
        if (WorldState.Instance.ahorgis_state == (byte)WorldState.Ahorgis.Trident) trident.SetActive(false);
        WorldState.Instance.currentLevel = "Ahorgis";
        if (WorldState.Instance.ahorgis_state == (byte)WorldState.Ahorgis.NotStarted)
        {
            StartCoroutine(Entrance());
        }
        if (WorldState.Instance.ahorgis_state >= (byte)WorldState.Ahorgis.BridgeOpened)
        {
            foreach(GameObject place in boxPlaces) place.SetActive(false);
            boxes[0].transform.position = new Vector2(1.5f, -5f);
            boxes[1].transform.position = new Vector2(1.5f, 3f);
            boxes[2].transform.position = new Vector2(11.5f, 3f);
            boxes[3].transform.position = new Vector2(11.5f, -5f);
            foreach(GameObject box in boxes)
            {
                MovingBox curBox = box.GetComponent<MovingBox>();
                if (curBox != null) curBox.movable = false;
            }
            bc.enabled = false;
            bridge.SetActive(true);
        }
        StartCoroutine(blackoutManager.Fade(true));
    }

    IEnumerator Entrance()
    {
        player.isControllable = false;
        yield return ponter.MoveForDialogue(false);
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(dialogue));
        StartCoroutine(dialoguePlayer.dialogueManager.HideShowPanel("Hide"));
        yield return ponter.MoveForDialogue(true);
        player.isControllable = true;
    }

}
