using UnityEngine;

public class Player : MonoBehaviour
{
    
    private byte level;
    private int exp;
    private short maxHealth = 255;
    private short curHealth;

    // --- CHARACTER ATTRIBUTES ---
    public float moveSpeed = 5f;
    private short magic_points;
    private byte strength;
    private byte defense;
    private byte intelligence;
    private byte persuasion;
    private byte vitality;
    private byte luck;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    public DialogueManager dialoguePanel;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        curHealth = maxHealth;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x > 0) spriteRenderer.flipX = false;
        else if (movement.x < 0) spriteRenderer.flipX = true;

        if (Input.GetKeyDown(KeyCode.F)) PerformInteraction();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void PerformInteraction()
    {
        Debug.Log("Player pressed F");
    }



}
