using UnityEngine;

public class VolsenInnState : MonoBehaviour
{
    public BlackoutManager blackoutManager;
    void Start()
    {
        WorldState.Instance.currentLevel = "VolsenInn";
        StartCoroutine(blackoutManager.Fade(true));
    }
}
