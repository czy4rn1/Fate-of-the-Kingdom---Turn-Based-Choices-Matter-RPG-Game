using UnityEngine;

public class HellsMountainState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    void Start()
    {
        WorldState.Instance.currentLevel = "HellsMountain";
        StartCoroutine(blackoutManager.Fade(true));
    }

    
}
