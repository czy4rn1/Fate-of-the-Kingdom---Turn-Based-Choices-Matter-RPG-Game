using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    public static WorldState Instance {get; private set;}
    private HashSet<string> openChests = new HashSet<string>();
    public string currentLevel ="";
    public bool keyStolen = false; // if player stole the key to the cell from jail guard in opening scene
    public bool guardRanAway = false; // if player can open the jail cell now
    public bool escapedThroughCave = false; // if player escaped opening jail through a dungeon
    public bool jewelBladeObtained = false;
    public bool castleFire = false;

    // --- KILMOR QUEST ---
    public bool kilmor_intro_ended = false;
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
    public bool golemEncounterEnded = false;
    // ------------------------

    // -- FISHERMAN ---
    public bool fish_questStarted = false;
    public bool fish_questEnded = false;
    public bool fish_willHelp = false;
    public bool fish_killed = false;
    // ----------------

    // -- VOLSEN ------
    public bool venardRemorse = false;
    public bool venardKilled = false;
    public bool venardEncounterEnded = false;


    

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

    public void MarkChestOpen(string chest_id)
    {
        if(!openChests.Contains(chest_id)) openChests.Add(chest_id);
    }
    public bool IsChestOpen(string chest_id)
    {
        return openChests.Contains(chest_id);
    }
}
