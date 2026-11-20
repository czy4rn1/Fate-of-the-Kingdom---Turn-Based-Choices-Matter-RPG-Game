using UnityEngine;

public class HellsMountainLoad : MonoBehaviour
{
    void Start()
    {
        if (WorldState.Instance.raggenfall_state != (byte)WorldState.Raggenfall.InfoFromKid) gameObject.SetActive(false);
    }

}
