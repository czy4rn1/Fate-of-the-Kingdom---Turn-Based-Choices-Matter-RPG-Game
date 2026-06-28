using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    private SpriteRenderer sr;
    public Vector2 movement;
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public DialogueManager dialoguePanel;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
    void Update()
    {
        if (movement.x > 0) sr.flipX = false;
        else if (movement.x < 0) sr.flipX = true;
    }
}
