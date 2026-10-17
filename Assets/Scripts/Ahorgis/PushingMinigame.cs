using UnityEngine;

public class PushingMinigame : MonoBehaviour
{
    public byte boxesLocked = 0;
    public GameObject bridge;
    private BoxCollider2D bc;
    public GameObject instructions;


    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        instructions.SetActive(false);
    }

    void Update()
    {
        if (boxesLocked >= 4)
        {
            if (bridge != null) bridge.SetActive(true);
            bc.enabled = false;
            WorldState.Instance.ahorgis_state = (byte)WorldState.Ahorgis.BridgeOpened;
            instructions.SetActive(false);
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (WorldState.Instance.ahorgis_state < (byte)WorldState.Ahorgis.BridgeOpened) instructions.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            instructions.SetActive(false);
        }
    }
}
