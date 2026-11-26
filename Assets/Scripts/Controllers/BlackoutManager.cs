using System.Collections;
using UnityEngine;

public class BlackoutManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeSpeed = 0.01f;
    public Coroutine currentCoroutine;
    public float curAlpha = 0;
    public bool startBlack = true;
    int fadeGeneration;
    WaitTimer waitTimer = new WaitTimer();

    void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
        if (startBlack) canvasGroup.alpha = 1f;
        else canvasGroup.alpha = 0f;
        curAlpha = canvasGroup.alpha;
    }

    public IEnumerator Fade(bool fadeIn)
    {
        int generation = ++fadeGeneration;
        if (canvasGroup == null) yield break;
        if (fadeIn)
        {
            while (canvasGroup.alpha > 0f && generation == fadeGeneration) {
                canvasGroup.alpha -= fadeSpeed;
                curAlpha = canvasGroup.alpha;
                yield return waitTimer.Wait(fadeSpeed);
            }
            if (generation != fadeGeneration) yield break;
            canvasGroup.alpha = 0f;
            curAlpha = canvasGroup.alpha;
        }
        else if (!fadeIn)
        {
            while (canvasGroup.alpha < 1f && generation == fadeGeneration)
            {
                canvasGroup.alpha += fadeSpeed;
                curAlpha = canvasGroup.alpha;
                yield return waitTimer.Wait(fadeSpeed);
            }
            if (generation != fadeGeneration) yield break;
            canvasGroup.alpha = 1f;
            curAlpha = canvasGroup.alpha;
        }
        if (generation == fadeGeneration) currentCoroutine = null;
    }

    public void SetFade(bool x)
    {
        if (currentCoroutine != null)
        {
            return;
        }
        currentCoroutine = StartCoroutine(Fade(x));
    }
}
