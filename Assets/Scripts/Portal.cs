using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Portal : MonoBehaviour
{
    public bool visible = false;
    private bool curVisible = false;
    public float curAlpha = 0f;
    public SpriteRenderer spriteRenderer;
    public Tilemap tilemap;
    public float showSpeed = 0.01f;
    private WaitTimer wait = new WaitTimer();
    private Coroutine coroutine;
    public bool tilemapType = false;
    public bool startVisible;

    void Start()
    {
        if (!tilemapType && spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
            Color c = spriteRenderer.color;
            if (startVisible) {
                c.a = 1f;
                visible = true;
                curVisible = true;
            }
            else c.a = 0f;
            spriteRenderer.color = c;
        }
        else if (tilemapType && tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
            Color c = tilemap.color;
            if (startVisible) {
                c.a = 1f;
                visible = true;
                curVisible = true;
            }
            else c.a = 0f;
            tilemap.color = c;
        }
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

    void OnEnable()
    {
        visible = true;
    }
 

    private IEnumerator HideShow(bool show)
    {
        Color c = !tilemapType ? spriteRenderer.color : tilemap.color;
        c.a = curAlpha;
        if (show)
        {
           while (curAlpha < 1f) {
                curAlpha += showSpeed;
                c.a = curAlpha;
                if (!tilemapType) spriteRenderer.color = c;
                else tilemap.color = c;
                yield return wait.Wait(showSpeed);
           }
           curAlpha = 1f;
        }
        else
        {
            while (curAlpha > 0f) {
                curAlpha -= showSpeed;
                c.a = curAlpha;
                if (!tilemapType) spriteRenderer.color = c;
                else tilemap.color = c;
                yield return wait.Wait(showSpeed);
            }
            curAlpha = 0f;
        }
        coroutine = null;
    }
}
