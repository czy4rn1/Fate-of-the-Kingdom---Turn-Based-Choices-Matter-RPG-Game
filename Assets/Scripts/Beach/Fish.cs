using UnityEngine;

public class Fish : MonoBehaviour
{
    public CollectFishes collectFishes;
    public PlayerDetection playerDetection;
    public Player player;

    void Update()
    {
        if (player.isControllable &&
        playerDetection.isPlayerNearby &&
        Input.GetKeyDown(KeyCode.F))
        {
            collectFishes.fishes_collected++;
            gameObject.SetActive(false);
        }
    }
}
