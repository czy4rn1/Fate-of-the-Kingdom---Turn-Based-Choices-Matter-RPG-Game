using UnityEngine;

public class MoveOnTouch : MonoBehaviour
{
    public Player player;
    public float newX;
    public float newY;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) player.transform.position = new Vector2(newX, newY);
    }
}
