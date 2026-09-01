using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoadToRaggenfallEnemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2D;
    public Player player;
    public bool top;
    private Vector2 movement;
    private float moveSpeed;
    public RoadToRaggenfallState state;
    public InitInteraction initInteraction;
    bool isDead = false;
    void Start()
    {
        Reset();
    }
    IEnumerator Dead()
    {
       movement.y = top ? 1f : -1f;
       movement.x = -1f; 
       moveSpeed = UnityEngine.Random.Range(8,15);
       if (top)
        {
           while (transform.position.y < 7.57f) yield return null; 
        }
        else
        {
            while (transform.position.y > -6.4f) yield return null;
        }
       Reset();
    }
    void Update()
    {
        if (initInteraction.Interaction(true))
        {
            isDead = true;
            StartCoroutine(Dead());
            boxCollider2D.enabled = false;
        }
        if (!isDead) movement.x = 1f;
        if (state.miniGameStarted) rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        if (transform.position.x > player.transform.position.x + 10f) Reset();  
    }

    void Reset()
    {
        boxCollider2D.enabled = true;
        isDead = false;
        movement.y = 0f;
        movement.x = 1f;
        moveSpeed = UnityEngine.Random.Range(3, 7);
        transform.position = new Vector2(player.transform.position.x - UnityEngine.Random.Range(15,20), top ? -3.34f : -4.2f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            state.lives--;
            Reset();
        }
    }

}
