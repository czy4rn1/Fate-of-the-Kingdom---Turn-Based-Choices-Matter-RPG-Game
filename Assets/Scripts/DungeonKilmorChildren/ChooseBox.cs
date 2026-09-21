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
        if (initInteraction.Interaction())
        {
            minigame.NextPhase(minigame.curPhase == correctPhase, initInteraction.CloseInteraction);
        }
    }
}
