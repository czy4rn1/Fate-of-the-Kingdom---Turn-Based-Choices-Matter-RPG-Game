using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isControllable = true;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    
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
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void setIsControllable(bool x)
    {
        isControllable = x;
    }

    public void StopMoving()
    {
        movement = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

}
