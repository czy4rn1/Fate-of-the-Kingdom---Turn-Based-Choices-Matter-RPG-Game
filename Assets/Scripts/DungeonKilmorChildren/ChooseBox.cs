using UnityEngine;

public class ChooseBox : MonoBehaviour
{
    private InitInteraction initInteraction;
    public DungeonKilmorChildrenMinigame minigame;
    public byte correctPhase = 255;

    void Start()
    {
        initInteraction = GetComponent<InitInteraction>();
    }

    void Update()
    {
        if (!minigame.gameEnded) {
            if (initInteraction.Interaction())
            {
                minigame.NextPhase(minigame.curPhase == correctPhase, initInteraction.CloseInteraction);
            }
        }
        else initInteraction.playerDetection.allowIcon = false;
    }
}
