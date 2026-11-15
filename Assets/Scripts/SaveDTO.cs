using System;
using System.Collections.Generic;

[Serializable]
public class SaveDTO
{
    public WorldDTO worldDTO;
    public PlayerDTO playerDTO;
}

[Serializable]
public class WorldDTO
{
    public string currentLevel = "";
    public bool keyStolen, guardRanAway, escapedThroughCave, jewelBladeObtained, castleFire, playerPosessed, ponterCaptured;
    public bool venardRemorse, venardIntro, venardEncounterEnded, askSellerAboutJewelBlade, learnedAboutHellsGarden, fireFixing, roadtorag_instructions, VolsenFell;
    public bool gardenEntrancePlayed, gardenerSummoned, vaseMoved, treeGameEnded, gardenerIntroPlayed, gardenerChoiceMade, gardenerJoins, ponterDead, playerEnslaved;
    public bool gardenerStandard, appleCutscenePlayed, appleTaken, blossomDarkness, disableTrees, lavaEntryCutscenePlayed;
    public float posessedR, posessedG, posessedB, posessedA;
    public byte dungeon_state, forest_state, fisher_state, ponter_state, mayhem_state, ahorgis_state, raggenfall_state, deserter_state;
    public List<string> openChests = new List<string>();
}

[Serializable]
public class PlayerDTO
{
    public string playerName = "";
    public short strength, intelligence, dexterity, persuasion, vitality, defense, money;
    public List<ItemDTO> items = new List<ItemDTO>();
    public List<EquipDTO> equipment = new List<EquipDTO>();
}

[Serializable]
public class ItemDTO
{
    public string itemName = "";
    public int count;
}

[Serializable]
public class EquipDTO
{
    public string name = "";
    public string type = "";
    public byte[] stats;
    public bool equipped;
}
