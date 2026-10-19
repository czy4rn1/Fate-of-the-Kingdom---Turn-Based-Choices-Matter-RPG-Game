using UnityEngine;

public class Climbing : MonoBehaviour
{
    public BoxCollider2D playerCollider;
    private Vector2 colOffset;
    private Vector2 colScale;
    private Vector2 newOffset;
    private Vector2 newScale;

    void Start()
    {
        colOffset = playerCollider.offset;
        colScale = playerCollider.size;
        newOffset = new Vector2(0f, 0f);
        newScale = new Vector2(0.5f, 1f);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null) {
                player.enableClimb = true;
                player.moveSpeed = 3f;
                playerCollider.offset = newOffset;
                playerCollider.size = newScale;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null) {
                player.enableClimb = false;
                player.moveSpeed = 5f;
                playerCollider.offset = colOffset;
                playerCollider.size = colScale;
            }
        }
    }
}
