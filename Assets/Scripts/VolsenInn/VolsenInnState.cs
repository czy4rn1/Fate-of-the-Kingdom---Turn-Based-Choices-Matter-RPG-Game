using UnityEngine;

public class VolsenInnState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    void Start()
    {
        WorldState.Instance.currentLevel = "VolsenInn";
        blackoutManager.currentCoroutine = StartCoroutine(blackoutManager.Fade(true));
    }
}
