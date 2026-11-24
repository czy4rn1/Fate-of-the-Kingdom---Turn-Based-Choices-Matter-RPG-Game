using System.Collections;
using UnityEngine;

public class Excalibur : MonoBehaviour
{
    public PlayDialogueLines dialoguePlayer;
    public MoveToCutscene moveToCutscene;
    InitInteraction initInteraction;
    public GameObject sword;
    public CameraController cameraController;
    WaitTimer waitTimer = new WaitTimer();
    SceneLoader sceneLoader;
    public 
    void Start()
    {
        initInteraction = GetComponent<InitInteraction>();
        sceneLoader = GetComponent<SceneLoader>();
    }

    void Update()
    {
        if (initInteraction.Interaction())
        {
            StartCoroutine(Dialogue());
        }
    }

    IEnumerator Dialogue()
    {
        yield return StartCoroutine(moveToCutscene.MoveForDialogue(false, false));
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Ponter: Finally we're here. We can finally beat Magnus once and for all.", "Ponter: Do you want to do the honors?", 
        "!<NAME>!: Yeah. Let's take the sword and bring the battle right to him!"}));
        PlayerData.Instance.AddEquipment("Excalibur", "Melee", new byte[] {40, 40, 40, 40});
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Obtained the Excalibur!"}));
        sword.SetActive(false);
        WorldState.Instance.hellsGarden_state = (byte)WorldState.HellsGarden.Excalibur;
        yield return StartCoroutine(waitTimer.Wait(2f));
        StartCoroutine(CameraShake());
        yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Ponter: Woah! This sword must have been holding the whole place together!", 
        "!<NAME>!: Let's get out of here as soon as possible and go to the castle! It's time to destroy Magnus!"}));
        StartCoroutine(sceneLoader.LoadArea());
    }

    IEnumerator CameraShake()
    {
        cameraController.followPlayer = false;
        Vector3 targetUp = new Vector3(cameraController.transform.position.x, cameraController.transform.position.y + 0.2f, -10f);
        Vector3 targetDown = new Vector3(cameraController.transform.position.x, cameraController.transform.position.y - 0.2f, -10f);
        while (true)
        {
            while (Vector3.Distance(cameraController.transform.position, targetUp) > 0.05f)
            {
                cameraController.transform.position = Vector3.MoveTowards(cameraController.transform.position, targetUp, 5f * Time.deltaTime);
                yield return null;
            }
            while (Vector3.Distance(cameraController.transform.position, targetDown) > 0.05f)
            {
                cameraController.transform.position = Vector3.MoveTowards(cameraController.transform.position, targetDown, 5f * Time.deltaTime);
                yield return null;
            }
        }
    }
}
