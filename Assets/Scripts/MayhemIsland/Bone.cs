using System.Collections;
using UnityEngine;

public class Bone : MonoBehaviour
{
    public DevilStatueQuest devilStatueQuest;
    public InitInteraction interaction;
    public PlayDialogueLines dialoguePlayer;
    public Player player;
    public bool[] HeadRibsHandBone = new bool[4];

    void Update()
    {
        if (interaction.Interaction())
        {
            devilStatueQuest.curItems++;
            StartCoroutine(PickUp());
        }
    }

    IEnumerator PickUp()
    {   
        if (HeadRibsHandBone[0]) yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Oh... That's someone's head.", "!<NAME>!: I hope this will help."}));
        else if (HeadRibsHandBone[1]) yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Oh, yuck! Those are someone's ribs!", "!<NAME>!: Anyway... I hope this is what I need."}));
        else if (HeadRibsHandBone[2]) yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: That's a... Hand?", "!<NAME>!: Better than nothing I guess."}));
        else if (HeadRibsHandBone[3]) yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: Oh... That's just a bone.", "!<NAME>!: I hope this will suffice."}));
        if (devilStatueQuest.curItems < devilStatueQuest.allItems) yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {$"!<NAME>!: That's {devilStatueQuest.curItems} out of {devilStatueQuest.allItems}."}, interaction.CloseInteraction));
        else yield return StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"!<NAME>!: I think this is all I need.", "!<NAME>!: Let's get back to the statue."}, interaction.CloseInteraction));
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
