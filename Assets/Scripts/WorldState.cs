using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool ponterCaptured = false;
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

    public enum Fisher : byte
    {
        introNotPlayed = 0,
        questStarted = 10,
        questEnded = 20,
        willHelp = 21,
        killed = 22
    }
    public byte fisher_state = 0;
    // ----------------

    // -- VOLSEN ------
    public bool venardIntro = false;
    public bool venardRemorse = false;
    public bool venardEncounterEnded = false;

    // -- VOLSEN INN -------

    public enum Ponter : byte
    {
        NotStarted = 0,
        QuestStarted = 10,
        InfoFromForgan = 20,
        InfoObtained = 30,
        QuestEnded = 40,
        Raggenfall = 41,
        Castle = 42,
        MayhemIsland = 43,
        BlossomGarden = 44
    }
    public byte ponter_state = 0;
    public bool askSellerAboutJewelBlade = false;
    public bool learnedAboutHellsGarden = false;
    public bool fireFixing = false;

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
        DemonMoon = 14,
        InfoFromKid = 15,
        EnteredMountain = 16
    }
    public bool killedGaren = false;
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

    public enum HellsGarden : byte
    {
        NotStarted = 0,
        Standard = 10,
        SecretPath = 20,
        Excalibur = 30
    }
    public byte hellsGarden_state = (byte)HellsGarden.NotStarted; // ADD TO DTO
    public bool sparedGaren = false; // ADD TO DTO
    public bool destroyedWall = false; // ADD TO DTO
    public bool tookKid = false; // ADD TO DTO
    public bool kidIntro = false; // ADD TO DTO


    

    void Awake()
    {
        posessedColor.r = 0.104f;
        posessedColor.g = 0.932f;
        posessedColor.b = 0.816f;
        posessedColor.a = 1f;
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

    public IEnumerable<string> OpenChestIds => openChests;
    public void RestoreChests(List<string> ids)
    {
        openChests.Clear();
        if (ids != null) foreach(string id in ids) openChests.Add(id);
    }
}
