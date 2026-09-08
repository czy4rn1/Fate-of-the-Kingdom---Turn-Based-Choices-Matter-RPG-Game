using UnityEngine;

public class MoveOnTouch : MonoBehaviour
{
    public Player player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) player.transform.position = new Vector2(-8.56f, -0.82f);
    }
}
