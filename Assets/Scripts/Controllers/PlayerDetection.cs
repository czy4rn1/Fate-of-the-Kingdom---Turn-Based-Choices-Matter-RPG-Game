using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public GameObject interactIcon;
    public bool isPlayerNearby = false;
    public bool allowIcon = true;
    void Start()
    {
        if(interactIcon != null) interactIcon.SetActive(false);
    }

    void Update()
    {
        if(!allowIcon && interactIcon != null) interactIcon.SetActive(false);
        if (allowIcon && interactIcon != null && isPlayerNearby) interactIcon.SetActive(true);
        else if (allowIcon && interactIcon != null && !isPlayerNearby) interactIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            isPlayerNearby = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            isPlayerNearby = false;
        }
    }
    
}
