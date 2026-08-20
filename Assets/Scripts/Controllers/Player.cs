using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isControllable = true;
    public bool isRoadToRaggenfall = false;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    public CharacterAnimation characterAnimation;
    public DialogueManager dialogueManager;
    
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
            if (movement.x != 0 || movement.y != 0) characterAnimation.isRunning = true;
            else characterAnimation.isRunning = false;
        }
        if (dialogueManager != null) {
            if (dialogueManager.dialogueActive) {
                characterAnimation.isRunning = false;
            }
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
