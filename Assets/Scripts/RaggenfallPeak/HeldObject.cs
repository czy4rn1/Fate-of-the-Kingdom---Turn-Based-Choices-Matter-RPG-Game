using UnityEngine;

public class HeldObject : MonoBehaviour
{
    private SpriteRenderer sr;
    public SpriteRenderer player_sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (sr.flipX != player_sr.flipX) sr.flipX = player_sr.flipX;
        if (sr.name == "sword") {
            if (!sr.flipX)
            {
                transform.localPosition = new Vector2(-0.163f, transform.localPosition.y);
                transform.localRotation = Quaternion.Euler(0f, 0f, -30f);
            }
            else
            {
                transform.localPosition = new Vector2(0.161f, transform.localPosition.y);
                transform.localRotation = Quaternion.Euler(0f, 0f, 30f);
            }
            if (player_sr.sprite.name == "Player_0") transform.localPosition = new Vector2(transform.localPosition.x, -0.584f);
            else if (player_sr.sprite.name == "Player_1") transform.localPosition = new Vector2(transform.localPosition.x, -0.61f);
        }
    }
}
