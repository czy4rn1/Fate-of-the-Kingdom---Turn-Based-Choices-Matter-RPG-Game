using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public BoxCollider2D boxCollider2D;
    bool inAir = false;
    public bool enableJump = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(isControllable) {
            movement.x = Input.GetAxisRaw("Horizontal");
            if (!inAir) movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x > 0) spriteRenderer.flipX = false;
            else if (movement.x < 0) spriteRenderer.flipX = true;
            if (movement.x != 0 || movement.y != 0) {
                if (!inAir) characterAnimation.isRunning = true;
            }
            else characterAnimation.isRunning = false;
            if (enableJump) if (Input.GetKeyDown(KeyCode.Space) && !inAir) {               
                StartCoroutine(Jump(movement.y == -1, movement.y == 1));
            }
        }
        if (dialogueManager != null) {
            if (dialogueManager.dialogueActive) {
                characterAnimation.isRunning = false;
            }
        }
    }

    IEnumerator Jump(bool down, bool up)
    {
        if (boxCollider2D != null) boxCollider2D.enabled = false;
        inAir = true;
        float curY = transform.position.y;
        movement.y = 1;
        characterAnimation.isRunning = false;
        characterAnimation.isJumping = true;
        if (!down && !up) {
            while (transform.position.y < curY + 1.5f) yield return null;
            movement.y = -1;
            while (transform.position.y > curY) yield return null;
        }
        else if (down && !up)
        {
            while (transform.position.y < curY + 0.5f) yield return null;
            movement.y = -1;
            while(transform.position.y > curY - 2.5f) yield return null;
        }
        else if (!down && up)
        {
            while (transform.position.y < curY + 2.5f) yield return null;
            movement.y = -1;
            while(transform.position.y > curY + 2.0f) yield return null;
        }
        inAir = false;
        characterAnimation.isJumping = false;
        if (boxCollider2D != null) boxCollider2D.enabled = true;
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
