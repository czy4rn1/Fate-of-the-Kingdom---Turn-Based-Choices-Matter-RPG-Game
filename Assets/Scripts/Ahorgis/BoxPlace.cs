using UnityEngine;

public class BoxPlace : MonoBehaviour
{
    public BoxCollider2D[] colliders = new BoxCollider2D[2];
    public PushingMinigame minigame;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            MovingBox box = collision.GetComponent<MovingBox>();
            if (box == null) return;
            foreach (BoxCollider2D col in colliders)
            {
                if (!col.IsTouching(collision)) return;
            }
            box.movable = false;
            box.transform.position = new Vector2(transform.position.x, transform.position.y);
            minigame.boxesLocked++;
            gameObject.SetActive(false);
        }
    }
}
