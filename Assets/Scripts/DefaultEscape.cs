using System.Threading;
using UnityEngine;
using UnityEngine.Playables;

public class DefaultEscape : MonoBehaviour
{
    public float waitTimer = 10f;
    public bool startRunning = false;
    public PlayableDirector playableDirector;
    public Character character;
    private bool cutsceneStarted = false;
    void Start()
    {
        if (playableDirector != null) playableDirector.Pause();
    }

    void Update()
    {
        if (startRunning) {
            waitTimer -= Time.deltaTime;
            Debug.Log($"{waitTimer}");
            if (!WorldState.Instance.keyStolen && waitTimer <= 0)
            {
                if(character!=null && !cutsceneStarted && character.wait) {
                    playableDirector.Play();
                    cutsceneStarted = true;
                }
                
            }
        }
    }

    public void StartCutscene(bool x)
    {
        startRunning = x;
    }
}
