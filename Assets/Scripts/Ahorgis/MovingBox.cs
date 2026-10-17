using System.Collections;
using UnityEngine;

public class MovingBox : MonoBehaviour
{
    public float pushSpeed = 0.2f;
    private Coroutine coroutine;
    private WaitTimer waitTimer = new WaitTimer();
    public Color color;
    private Color ogColor;
    public SpriteRenderer sr;
    private BoxCollider2D bc;
    public Player player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private RaycastHit2D[] hits = new RaycastHit2D[8];
    private ContactFilter2D filter;
    public bool movable = true;
    public Vector2 ogPos;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ogColor = sr.color;
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        filter.useTriggers = false;
        ogPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movable)
        {
            if (Input.GetKeyDown(KeyCode.X)) transform.position = ogPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            if (movable) {
                Debug.Log("Kontakt z graczem");
                if (coroutine != null) return;
                coroutine = StartCoroutine(Push());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            Debug.Log("Koniec kontaktu z graczem");
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = null;
            StopPushing();
        }
    }

    void FixedUpdate()
    {
        if (movement == Vector2.zero) return;
        float step = pushSpeed * Time.fixedDeltaTime;
        if (rb.Cast(movement, filter, hits, step + 0.01f) == 0 && movable)
            rb.MovePosition(rb.position + movement * step);
    }

    IEnumerator Push()
    {
        Bounds p = player.boxCollider2D.bounds;
        sr.color = color;
        yield return StartCoroutine(waitTimer.Wait(0.2f));
        if (p.max.x < bc.bounds.min.x)
        {
            movement.x = 1f;
            movement.y = 0f;
        }
        else if (p.min.x > bc.bounds.max.x)
        {
            movement.x = -1f;
            movement.y = 0f;
        }
        else if (p.min.y > bc.bounds.max.y)
        {
            movement.x = 0f;
            movement.y = -1f;
        }
        else if (p.max.y < bc.bounds.min.y)
        {
            movement.x = 0f;
            movement.y = 1f;
        }
    }

    void StopPushing()
    {
        movement = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        sr.color = ogColor;
    }
}
