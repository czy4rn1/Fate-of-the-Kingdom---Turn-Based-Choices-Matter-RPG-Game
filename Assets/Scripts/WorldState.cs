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

    public bool playerPosessed = false;
    public Color posessedColor;

    // --- KILMOR QUEST ---

    public enum Dungeon : byte
    {
        LevelIntroNotPlayed = 0,
        IntroNotPlayed = 1,
        QuestStarted = 10,
        SecretPathOpened = 20,
        QuestEnded = 30,
        Attacked = 31,
        Ignored = 32,
        Saved = 33,
        Dead = 34
    }

    public byte dungeon_state = (byte)Dungeon.LevelIntroNotPlayed;
    // --------------------

    // --- FOREST ENCOUNTER ---
    public enum Forest : byte
    {
        NotStarted = 0,
        PickedGem = 10,
        LostGem = 12
    }
    public byte forest_state = (byte)Forest.NotStarted;
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

    // -- VOLSEN INN -------
    public bool ponterQuestStarted = false;
    public bool ponterQuestEnded = false;
    public bool ponterInfoObtained = false;
    public bool ponterInfoFromForgan = false;
    public bool askSellerAboutJewelBlade = false;
    public bool learnedAboutHellsGarden = false;
    public bool fireFixing = false;
    public bool raggenfall = false;
    public bool castle = false;
    public bool mayhemIsland = false;
    public bool blossomGarden = false;

    public bool roadtorag_instructions = true;

    // -- MAYHEM ISLAND --
    public enum Mayhem : byte
    {
        NotEntered = 0,
        Entered = 5,
        Started = 10,
        Ended = 20
    }
    public byte mayhem_state = (byte)Mayhem.NotEntered;

    // -- BLOSSOM GARDEN -- 
    public bool gardenEntrancePlayed = false;
    public bool gardenerSummoned = false;
    public bool vaseMoved = false;
    public bool treeGameEnded = false;
    public bool gardenerIntroPlayed = false;
    public bool gardenerChoiceMade = false;
    public bool gardenerJoins = false;
    public bool ponterDead = false;
    public bool playerEnslaved = false;
    public bool gardenerStandard = false;
    public bool appleCutscenePlayed = false;
    public bool appleTaken = false;
    public bool blossomDarkness = false;
    public bool disableTrees = false;
    public bool ponterCaptured = false;

    // -- DUNGEON LAVA --
    public bool lavaEntryCutscenePlayed = false;

    // -- AHORGIS --
    public enum Ahorgis : byte
    {
        NotStarted = 0,
        BridgeOpened = 10,
        ChoiceMade = 20,
        Trident = 21,
        Posessed = 22,
        Flood = 23
    }
    public byte ahorgis_state = (byte)Ahorgis.NotStarted;

    public enum Raggenfall : byte
    {
        NotStarted = 0,
        ChoiceMade = 10,
        Moon = 11,
        JewelBlade = 12,
        PosessedMoon = 13,
        DemonMoon = 14
    }
    public byte raggenfall_state = (byte)Raggenfall.NotStarted;

    public bool VolsenFell = false;

    public enum Deserter : byte
    {
        NotStarted = 0,
        MetDeserter = 10,
        ChoiceMade = 20,
        JoinedDeserter = 21,
        KilledDeserter = 22
    }
    public byte deserter_state = (byte)Deserter.NotStarted;


    

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
