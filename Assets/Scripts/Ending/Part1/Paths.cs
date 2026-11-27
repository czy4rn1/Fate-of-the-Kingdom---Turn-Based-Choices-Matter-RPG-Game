using System.Collections;
using UnityEngine;

public class Paths : MonoBehaviour
{
    public AbstractPath[] paths = new AbstractPath[5]; // rag, deserter, excalibur, ahorgis, blossom
    public BlackoutManager blackoutManager;

    void Start()
    {
        if (WorldState.Instance.raggenfall_state >= (byte)WorldState.Raggenfall.ChoiceMade && WorldState.Instance.raggenfall_state < (byte)WorldState.Raggenfall.InfoFromKid) DestroyAllBut(0);
        else if (WorldState.Instance.ahorgis_state >= (byte)WorldState.Ahorgis.ChoiceMade) DestroyAllBut(3);
        else if (WorldState.Instance.hellsGarden_state == (byte)WorldState.HellsGarden.Excalibur) DestroyAllBut(2);
        else if (WorldState.Instance.deserter_state >= (byte)WorldState.Deserter.ChoiceMade) DestroyAllBut(1);
        else if (WorldState.Instance.appleTaken || WorldState.Instance.gardenerJoins || WorldState.Instance.playerEnslaved || WorldState.Instance.blossomDarkness) DestroyAllBut(4);
        blackoutManager.currentCoroutine = StartCoroutine(blackoutManager.Fade(true));
    }


    void DestroyAllBut(int num)
    {
        for(int i=0; i<paths.Length; i++)
        {
            if (i == num || paths[i] == null) continue;
            Destroy(paths[i].gameObject);
        }
    }

    public void MainPaths()
    {
        foreach(var p in paths)
        {
            if (p != null) {
                StartCoroutine(p.StartPath());
                return;
            }
        }
    }
}
