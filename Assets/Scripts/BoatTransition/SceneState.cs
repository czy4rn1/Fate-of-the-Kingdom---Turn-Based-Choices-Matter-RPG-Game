using UnityEngine;

public class SceneState : MonoBehaviour
{
    public GameObject fisherman;
    void Start()
    {
        if (WorldState.Instance.fish_killed) fisherman.SetActive(false);
    }
}
