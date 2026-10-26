using UnityEngine;

public class Climbing : MonoBehaviour
{
    public BoxCollider2D playerCollider;
    private Vector2 colOffset;
    private Vector2 colScale;
    private Vector2 newOffset;
    private Vector2 newScale;
    public Player player;
    public float[] xRange = new float[2];
    public FallingRock[] rocks = new FallingRock[3];
    public byte playerLives = 5;
    private bool reachedTop = false;
    public Vector2 startPosition;
    public float topY;

    void Start()
    {
        colOffset = playerCollider.offset;
        colScale = playerCollider.size;
        newOffset = new Vector2(0f, 0f);
        newScale = new Vector2(0.5f, 1f);
    }

    void Update()
    {
        if (player.transform.position.y >= topY) reachedTop = true;
        if (player.enableClimb && !reachedTop) foreach(var rock in rocks) rock.gameObject.SetActive(true); 
        else foreach(var rock in rocks) rock.gameObject.SetActive(false);
    }

    public void DecreaseLives()
    {
        playerLives--;
        if (playerLives <= 0) player.transform.position = startPosition;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.enableClimb = true;
            player.moveSpeed = 3f;
            playerCollider.offset = newOffset;
            playerCollider.size = newScale;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.enableClimb = false;
            player.moveSpeed = 5f;
            playerCollider.offset = colOffset;
            playerCollider.size = colScale;
        }
    }
}
