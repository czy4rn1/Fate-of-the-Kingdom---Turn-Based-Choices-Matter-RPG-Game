using UnityEngine;

public class EnableJump : MonoBehaviour
{
    public Player player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) player.enableJump = true;   
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) player.enableJump = false;
    }

}
