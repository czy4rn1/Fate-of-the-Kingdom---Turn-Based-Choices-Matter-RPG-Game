using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingRock : MonoBehaviour
{
    public Player player;
    public Climbing climbingGame;
    Rigidbody2D rb;
    WaitTimer waitTimer = new WaitTimer();
    Coroutine coroutine;
    Vector2 startPosition;
    public float moveSpeed = 3f;
    Vector2 movement = new Vector2(0f, -1f);
    bool firstFall = true;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    void OnEnable()
    {
        GameObject tilemapToIgnore = GameObject.FindGameObjectWithTag("Pnacza");
        Physics2D.IgnoreCollision(tilemapToIgnore.GetComponent<TilemapCollider2D>(), GetComponent<BoxCollider2D>());
        if (!firstFall) ResetPosition();
        firstFall = false;
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(StartMoving());
    }

    void Update()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        if (rb.position.y <= player.transform.position.y - 7f) {
            ResetPosition();
            if (coroutine != null) StopCoroutine(coroutine);          
            coroutine = StartCoroutine(StartMoving());
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            climbingGame.DecreaseLives();
            ResetPosition();
        }
    }

    IEnumerator StartMoving()
    {
        if (rb != null) {
            movement.x = movement.y = 0f;
            yield return waitTimer.Wait(Random.Range(0f, 2f));
            movement.y = -1f;
        }
    }

    void ResetPosition()
    {
        bool collidedWithRock;
        byte maxAttempts = 5;
        byte curAttempt = 0;
        do {
            if(++curAttempt > maxAttempts)
            {
                rb.position = startPosition;
                break;
            }
            collidedWithRock = false;
            rb.position = new Vector2(Random.Range(climbingGame.xRange[0], climbingGame.xRange[1]), climbingGame.topY-0.5f);
            Collider2D[] hits = Physics2D.OverlapCircleAll(rb.position, 0.5f);
            foreach(var hit in hits)
            {
                if (hit.gameObject == gameObject) continue;
                if (hit.CompareTag("Rock"))
                {
                    collidedWithRock = true;
                    break;
                }
            }
        } while (collidedWithRock);
    }
}
