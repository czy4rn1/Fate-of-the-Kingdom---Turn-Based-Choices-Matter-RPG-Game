using System.Linq;
using Unity.Profiling;
using UnityEngine;

public class PentaTreeGame : MonoBehaviour
{
    public byte allTrees = 5;
    public byte curTrees = 0;
    public PentaTree[] pentaTrees = new PentaTree[5];
    public PlayDialogueLines dialoguePlayer;
    public bool allTreesChosen = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseTree(PentaTree callingTree)
    {
        if (!pentaTrees.Contains(callingTree)) {
            curTrees = 0;
            foreach (PentaTree tree in pentaTrees) StartCoroutine(tree.ChangeColor(false));
            StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"Something's wrong..."}, callingTree.initInteraction.CloseInteraction));
            return;
        }
        curTrees++;
        StartCoroutine(callingTree.ChangeColor(true));
        if (curTrees < allTrees) StartCoroutine(dialoguePlayer.PlayDialogue(new string[] {"The tree changed it's color!"}, callingTree.initInteraction.CloseInteraction));
        else allTreesChosen = true;
    }
}
