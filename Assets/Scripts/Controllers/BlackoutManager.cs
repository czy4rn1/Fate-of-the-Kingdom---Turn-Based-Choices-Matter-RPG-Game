using System.Collections;
using UnityEngine;

public class BlackoutManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeSpeed = 0.01f;
    private Coroutine currentCoroutine;

    void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    public IEnumerator Fade(bool fadeIn)
    {
        if (canvasGroup == null) yield break;
        if (fadeIn)
        {
            while (canvasGroup.alpha > 0f) {
                canvasGroup.alpha -= fadeSpeed;
                yield return new WaitForSeconds(fadeSpeed);
            }
            canvasGroup.alpha = 0f;
        }
        else if (!fadeIn)
        {
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += fadeSpeed;
                yield return new WaitForSeconds(fadeSpeed);
            }
            canvasGroup.alpha = 1f;
        }
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
