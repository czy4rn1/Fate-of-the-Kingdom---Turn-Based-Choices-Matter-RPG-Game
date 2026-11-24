using UnityEngine;

public class ToggleWall : MonoBehaviour
{
    public GameObject wall;
    public bool open = false;

    void Start()
    {
        wall.SetActive(open);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wall.SetActive(!open);
        }
    }
}
