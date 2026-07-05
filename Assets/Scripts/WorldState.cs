using UnityEngine;

public class WorldState : MonoBehaviour
{
    public static WorldState Instance {get; private set;}
    public bool keyStolen = false; // if player stole the key to the cell from jail guard in opening scene
    public bool guardRanAway = false; // if player can open the jail cell now
    public bool escapedThroughCave = false; // if player escaped opening jail through a dungeon
    public bool jewelBladeObtained = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
