using UnityEngine;
using UnityEngine.Playables;

public class DefaultEscapeCutscene : MonoBehaviour
{
    public PlayableDirector pd;
    public float waitTimer = 45f;
    void Start()
    {
        if (pd != null) pd.Pause();
    }

    void Update()
    {
        waitTimer -= Time.deltaTime;
        Debug.Log($"{waitTimer}");
        if (!WorldState.Instance.keyStolen && waitTimer <= 0)
        {
            pd.Play();
        }
    }
}
