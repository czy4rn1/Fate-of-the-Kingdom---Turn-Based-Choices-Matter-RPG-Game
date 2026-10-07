using UnityEngine;

public class BlossomGardenState : MonoBehaviour
{
    public GameObject gardener;
    public PentaTreeGame pentaTreeGame;
    public GameObject flowerVase;
    public GameObject devilStatue;
    public BlackoutManager blackoutManager;
    void Start()
    {
        if (WorldState.Instance.gardenerSummoned) gardener.SetActive(true);
        if (WorldState.Instance.treeGameEnded)
        {
            foreach (PentaTree tree in pentaTreeGame.pentaTrees) pentaTreeGame.ChooseTree(tree);
            pentaTreeGame.allTreesChosen = true;
        }
        if (WorldState.Instance.vaseMoved) {
            flowerVase.transform.position = new Vector2(35.19f, 11.86f);
            devilStatue.SetActive(true);
        }
        StartCoroutine(blackoutManager.Fade(true));
    }

}
