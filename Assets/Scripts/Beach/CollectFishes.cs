using System.Collections;
using UnityEditor.XR;
using UnityEngine;

public class CollectFishes : MonoBehaviour
{
    public GameObject[] fishes = new GameObject[5];
    public byte fishes_collected = 0;
    private bool fishesSpawned = false;
    void Start()
    {
        foreach (GameObject fish in fishes) fish.SetActive(false);
    }
    void Update()
    {
        if (WorldState.Instance.fish_questStarted)
        {
            if (!fishesSpawned) {
                fishesSpawned = true;
                SpawnFish();
            }
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
