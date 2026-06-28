using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isControllable;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    public DialogueManager dialoguePanel;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(isControllable) {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x > 0) spriteRenderer.flipX = false;
            else if (movement.x < 0) spriteRenderer.flipX = true;

            if (Input.GetKeyDown(KeyCode.F)) PerformInteraction();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void PerformInteraction()
    {
        Debug.Log("Player pressed F");
    }

    public void setIsControllable(bool x)
    {
        isControllable = x;
    }

}
