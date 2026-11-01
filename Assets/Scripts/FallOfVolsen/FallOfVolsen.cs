using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class FallOfVolsen : MonoBehaviour
{
    public PlayableDirector[] cutscenes = new PlayableDirector[4];
    public GameObject[] grids = new GameObject[4];

    public BlackoutManager blackoutManager;
    public DialogueManager dialogueManager;
    
    void Awake()
    {
        for (int i=0; i<4; i++)
        {
            cutscenes[i].gameObject.SetActive(false);
            grids[i].SetActive(false);
        }
        StartCoroutine(AllCutscenes());
    }

    IEnumerator AllCutscenes()
    {
        for (int i=0; i<4; i++)
        {
            cutscenes[i].gameObject.SetActive(true);
            grids[i].SetActive(true);
            dialogueManager.timelineDirector = cutscenes[i];
            cutscenes[i].Play();
            StartCoroutine(blackoutManager.Fade(true));
            while (cutscenes[i].gameObject.activeInHierarchy) yield return null;
            yield return StartCoroutine(blackoutManager.Fade(false));
            grids[i].SetActive(false);
        }
        WorldState.Instance.VolsenFell = true;
        SceneManager.LoadScene("Campfire");
    }
}
