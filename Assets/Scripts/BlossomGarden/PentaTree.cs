using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PentaTree : MonoBehaviour
{
    public InitInteraction initInteraction;
    public SpriteRenderer spriteRenderer;
    public bool selectable = false;
    public PlayDialogueLines dialoguePlayer;
    public PentaTreeGame minigame;
    private Color originalColor;
    public Color targetColor;
    public bool compsEnabled = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        if (WorldState.Instance.treeGameEnded) initInteraction.EnableInteraction(false);
    }

    void Update()
    {
        if (initInteraction.enabled) {
            if (initInteraction.Interaction())
            {
                dialoguePlayer.PlayCommand("Interact with the apple tree?\n1. Yes\n2. No", 2, OnChosenCommand);
            }
        }
        if (minigame.allTreesChosen && compsEnabled) initInteraction.EnableInteraction(false);
    }


    public void OnChosenCommand(int command)
    {
        initInteraction.playerDetection.player.isControllable = false;
        if (command == 0)
        {
           minigame.ChooseTree(this);
        }
        else initInteraction.CloseInteraction(0);
    }

    public IEnumerator ChangeColor(bool correct)
    {
        if (correct) initInteraction.EnableInteraction(false);
        else {
            initInteraction.enabled = true;
            initInteraction.EnableInteraction(true);
        }    
        Color curColor = spriteRenderer.color;
        for (float time = 0f; time < 1f; time+=Time.deltaTime)
        {
            spriteRenderer.color = Color.Lerp(curColor, correct ? targetColor : originalColor, time / 1f);
            yield return null;
        }
        spriteRenderer.color = correct ? targetColor : originalColor;  
    }
}
