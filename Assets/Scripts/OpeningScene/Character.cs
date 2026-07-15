using UnityEngine;

public class Character : MonoBehaviour
{
    private SpriteRenderer sr;
    public Vector2 movement;
    private Rigidbody2D rb;
    public float moveSpeed = 3.5f;
    public DialogueManager dialoguePanel;
    private float leftX = -10.45f;
    public float rightX = 0.75f;
    public bool wait = false;
    private bool goLeft = false;
    private float waitTimer;
    private float waitingTime = 3f;
    public bool AI_active = false;
    public bool kill = false;
    public PlayerDetection playerDetection;
    void Start()
    {
        playerDetection.allowIcon = false;
        waitTimer = waitingTime;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (AI_active) {
            if (!wait) {
                playerDetection.allowIcon = false;
                movement.x = goLeft ? -1f : 1f;
                if(goLeft && rb.position.x <= leftX)
                {
                    goLeft = false;
                    wait = true;
                    movement = Vector2.zero;
                    rb.linearVelocity = Vector2.zero;
                }
                else if(!goLeft && rb.position.x >= rightX)
                {
                    goLeft = true;
                    wait = true;
                    movement = Vector2.zero;
                    rb.linearVelocity = Vector2.zero;
                    if (kill)
                    {
                        WorldState.Instance.guardRanAway = true;
                        gameObject.SetActive(false);
                    }
                }      
            }
            else
            {
                playerDetection.allowIcon = true;
                waitTimer -= Time.deltaTime;
                if(waitTimer <= 0) {
                    wait = false;
                    waitTimer = waitingTime;
                }
            }
            if (movement.x > 0) sr.flipX = false;
            else if (movement.x < 0) sr.flipX = true;
        else return;    
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
    public void activateAI(bool x)
    {
        AI_active = x;
    }
}
