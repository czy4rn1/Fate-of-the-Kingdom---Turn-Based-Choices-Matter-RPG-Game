using UnityEngine;

public class PressBar : MonoBehaviour
{
    public bool cursorOnBar = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cursor")) cursorOnBar = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cursor")) cursorOnBar = false;
    }
}
