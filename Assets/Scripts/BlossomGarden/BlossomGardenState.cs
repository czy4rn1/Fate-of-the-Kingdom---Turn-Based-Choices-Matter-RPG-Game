using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BlossomGardenState : MonoBehaviour
{
    public GameObject gardener;
    public PentaTreeGame pentaTreeGame;
    public GameObject flowerVase;
    public GameObject devilStatue;
    public BlackoutManager blackoutManager;
    public PlayableDirector cutscene;
    public GameObject apple;
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
        if (WorldState.Instance.appleCutscenePlayed && !WorldState.Instance.appleTaken) apple.SetActive(true);
        StartCoroutine(blackoutManager.Fade(true));
    }

    void Update()
    {
        if (pentaTreeGame.allTreesChosen && !WorldState.Instance.appleCutscenePlayed) {
            StartCoroutine(ShowUpCutscene());
            WorldState.Instance.appleCutscenePlayed = true;
        }
    }

    public IEnumerator ShowUpCutscene()
    {
        WorldState.Instance.treeGameEnded = true;
        yield return StartCoroutine(blackoutManager.Fade(false));
        cutscene.Play();
        yield return StartCoroutine(blackoutManager.Fade(true));
        while(cutscene.state == PlayState.Playing) yield return null;
        if (!WorldState.Instance.gardenerSummoned)
        {
            gardener.transform.position = new Vector2(49.71f, 6.75f);
            gardener.SetActive(true);
        }
    }

}
