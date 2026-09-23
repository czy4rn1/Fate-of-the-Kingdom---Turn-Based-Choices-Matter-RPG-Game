using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool visible = false;
    private bool curVisible = false;
    private float curAlpha = 0f;
    public SpriteRenderer spriteRenderer;
    public float showSpeed = 0.01f;
    private WaitForSeconds wait;
    private Coroutine coroutine;
    void Start()
    {
        Color c = spriteRenderer.color;
        c.a = 0f;
        spriteRenderer.color = c;
        wait = new WaitForSeconds(showSpeed);
    }
    void Update()
    {
        if (curVisible != visible)
        {
            curVisible = visible;
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(HideShow(curVisible));
        }
    }

    private IEnumerator HideShow(bool show)
    {
        Color c = spriteRenderer.color;
        c.a = curAlpha;
        if (show)
        {
           while (curAlpha < 1f) {
                curAlpha += showSpeed;
                c.a = curAlpha;
                spriteRenderer.color = c;
                yield return wait;
           }
           curAlpha = 1f;
        }
        else
        {
            while (curAlpha > 0f) {
                curAlpha -= showSpeed;
                c.a = curAlpha;
                spriteRenderer.color = c;
                yield return wait;
            }
            curAlpha = 0f;
        }
        coroutine = null;
    }
}
