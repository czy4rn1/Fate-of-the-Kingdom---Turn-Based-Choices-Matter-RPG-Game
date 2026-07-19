using UnityEngine;

public class WorldState : MonoBehaviour
{
    public static WorldState Instance {get; private set;}
    public bool keyStolen = false; // if player stole the key to the cell from jail guard in opening scene
    public bool guardRanAway = false; // if player can open the jail cell now
    public bool escapedThroughCave = false; // if player escaped opening jail through a dungeon
    public bool jewelBladeObtained = false;
    public bool castleFire = false;

    // --- KILMOR QUEST ---
    public bool kilmor_questStarted = false;
    public bool kilmor_questEnded = false;
    public bool secretPathOpened = false;
    public bool attackedKilmor = false;
    public bool ignoredKilmor = false;
    public bool savedChildren = false;
    // --------------------

    // --- FOREST ENCOUNTER ---
    public bool redGemObtained = false;
    public bool golemsHaveGem = false;
    // ------------------------

    // -- FISHERMAN ---
    public bool fish_questStarted = false;
    public bool fish_questEnded = false;
    public bool fish_willHelp = false;
    public bool fish_killed = false;
    // ----------------


    

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
