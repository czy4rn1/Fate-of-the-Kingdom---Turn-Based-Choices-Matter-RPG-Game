using UnityEngine;

public class CollectFishes : MonoBehaviour
{
    public GameObject[] fishes = new GameObject[7];
    public byte fishes_collected = 0;
    private bool fishesSpawned = false;
    void Start()
    {
        foreach (GameObject fish in fishes) fish.SetActive(false);
    }
    void Update()
    {
        if (WorldState.Instance.fisher_state == (byte)WorldState.Fisher.questStarted)
        {
            if (!fishesSpawned) {
                fishesSpawned = true;
                SpawnFish();
            }
        }
        if (WorldState.Instance.fisher_state >= (byte)WorldState.Fisher.questEnded) {
            Destroy(gameObject);
        }
    }

    void SpawnFish()
    {
        foreach(GameObject fish in fishes)
        {
            fish.SetActive(true);
        }
    }
}
