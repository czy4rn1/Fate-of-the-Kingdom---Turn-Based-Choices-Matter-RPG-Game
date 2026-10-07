using UnityEngine;

public class SummonGardener : MonoBehaviour
{
    public InitInteraction initInteraction;
    public PlayDialogueLines dialoguePlayer;
    public byte reqInt;
    public GameObject gardener;

    void Update()
    {
        if (initInteraction.Interaction())
        {
            dialoguePlayer.PlayCommand($"[INT {PlayerData.Instance.intelligence}/{reqInt}] Summon the demon?\n1. Yes\n2. No", 2, OnChosenCommand);
        }
    }

    public void OnChosenCommand(int command)
    {
        if (command == 0)
        {
            if (PlayerData.Instance.intelligence >= reqInt) {
                gardener.SetActive(true);
                initInteraction.CloseInteraction(command);
                WorldState.Instance.gardenerSummoned = true;
                initInteraction.EnableInteraction(false);
            }
            else StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"ACTION FAILED"}, initInteraction.CloseInteraction));
        }
        else initInteraction.CloseInteraction(command);
    }
}
