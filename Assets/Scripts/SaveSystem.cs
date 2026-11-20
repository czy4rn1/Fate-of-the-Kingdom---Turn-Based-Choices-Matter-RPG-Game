using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem
{
    const string filename = "savefile.json";
    public static string filePath => Path.Combine(Application.persistentDataPath, filename);
    public static bool exists => File.Exists(filePath);

    public static bool Save()
    {
        if (WorldState.Instance == null || PlayerData.Instance == null) return false;
        try
        {
            var file = new SaveDTO{worldDTO = CaptureWorld(), playerDTO = CapturePlayer()};
            string tmp = filePath + ".tmp";
            File.WriteAllText(tmp, JsonUtility.ToJson(file, true));
            if (File.Exists(filePath)) File.Delete(filePath);
            File.Move(tmp, filePath);
            return true;
        } catch
        {
            Debug.Log("Błąd zapisu");
            return false;
        }
    }

    static WorldDTO CaptureWorld()
    {
        var w = WorldState.Instance;
        return new WorldDTO
        {
            currentLevel = w.currentLevel,
            keyStolen = w.keyStolen, guardRanAway = w.guardRanAway, escapedThroughCave = w.escapedThroughCave, jewelBladeObtained = w.jewelBladeObtained,
            castleFire = w.castleFire, playerPosessed = w.playerPosessed, ponterCaptured = w.ponterCaptured, posessedR = w.posessedColor.r, posessedG = w.posessedColor.g, posessedB = w.posessedColor.b,
            posessedA = w.posessedColor.a, venardRemorse = w.venardRemorse, venardIntro = w.venardIntro, venardEncounterEnded = w.venardEncounterEnded, askSellerAboutJewelBlade = w.askSellerAboutJewelBlade,
            learnedAboutHellsGarden = w.learnedAboutHellsGarden, fireFixing = w.fireFixing, roadtorag_instructions = w.roadtorag_instructions, VolsenFell = w.VolsenFell,
            gardenEntrancePlayed = w.gardenEntrancePlayed, gardenerSummoned = w.gardenerSummoned, vaseMoved = w.vaseMoved, treeGameEnded = w.treeGameEnded, gardenerIntroPlayed = w.gardenerIntroPlayed,
            gardenerChoiceMade = w.gardenerChoiceMade, gardenerJoins = w.gardenerJoins, ponterDead = w.ponterDead, playerEnslaved = w.playerEnslaved, gardenerStandard = w.gardenerStandard, appleCutscenePlayed = w.appleCutscenePlayed,
            appleTaken = w.appleTaken, blossomDarkness = w.blossomDarkness, disableTrees = w.disableTrees, lavaEntryCutscenePlayed = w.lavaEntryCutscenePlayed,
            dungeon_state = w.dungeon_state, forest_state = w.forest_state, fisher_state = w.fisher_state, ponter_state = w.ponter_state, mayhem_state = w.mayhem_state, ahorgis_state = w.ahorgis_state, raggenfall_state = w.raggenfall_state,
            killedGaren = w.killedGaren, deserter_state = w.deserter_state, openChests = new List<string>(w.OpenChestIds)
        };
    }

    public static void Delete()
    {
        if (exists)
        {
            File.Delete(filePath);
        }
    }

    static PlayerDTO CapturePlayer()
    {
        var p = PlayerData.Instance;
        var dto =  new PlayerDTO
        {
            playerName = p.playerName,
            strength = p.strength, intelligence = p.intelligence, dexterity = p.dexterity, persuasion = p.persuasion, defense = p.defense, vitality = p.vitality, money = p.money
        };
        foreach (var items in p.playerItems) dto.items.Add(new ItemDTO{itemName = items.itemName, count = items.count});
        foreach (var eq in p.equipment) dto.equipment.Add(new EquipDTO {name = eq.name, type = eq.type, stats = eq.stats, equipped = eq.equipped});
        return dto;
    }

    public static bool Load()
    {
        if (!exists) return false;
        try
        {
            var file = JsonUtility.FromJson<SaveDTO>(File.ReadAllText(filePath));
            if (file == null || file.worldDTO == null || file.playerDTO == null) return false;
            Apply(file);
            string level = file.worldDTO.currentLevel;
            if (string.IsNullOrEmpty(level)) return false;
            if(!Application.CanStreamedLevelBeLoaded(level)) return false;
            SceneManager.LoadScene(level);
            return true;
        } catch { return false;}
    }

    static void Apply(SaveDTO save)
    {
        var w = WorldState.Instance;
        var s = save.worldDTO;
        w.currentLevel = s.currentLevel;
        w.keyStolen = s.keyStolen; w.guardRanAway = s.guardRanAway; w.escapedThroughCave = s.escapedThroughCave; w.jewelBladeObtained = s.jewelBladeObtained;
        w.castleFire = s.castleFire; w.playerPosessed = s.playerPosessed; w.ponterCaptured = s.ponterCaptured;
        w.posessedColor = new Color(s.posessedR, s.posessedG, s.posessedB, s.posessedA);
        w.venardRemorse = s.venardRemorse; w.venardIntro = s.venardIntro; w.venardEncounterEnded = s.venardEncounterEnded; w.askSellerAboutJewelBlade = s.askSellerAboutJewelBlade; w.learnedAboutHellsGarden = s.learnedAboutHellsGarden;
        w.fireFixing = s.fireFixing; w.roadtorag_instructions = s.roadtorag_instructions; w.VolsenFell = s.VolsenFell; w.gardenEntrancePlayed = s.gardenEntrancePlayed; w.gardenerSummoned = s.gardenerSummoned;
        w.vaseMoved = s.vaseMoved; w.treeGameEnded = s.treeGameEnded; w.gardenerIntroPlayed = s.gardenerIntroPlayed; w.gardenerChoiceMade = s.gardenerChoiceMade; w.gardenerJoins = s.gardenerJoins; w.ponterDead = s.ponterDead;
        w.playerEnslaved = s.playerEnslaved; w.gardenerStandard = s.gardenerStandard; w.appleCutscenePlayed = s.appleCutscenePlayed; w.appleTaken = s.appleTaken; w.blossomDarkness = s.blossomDarkness; w.disableTrees = s.disableTrees;
        w.lavaEntryCutscenePlayed = s.lavaEntryCutscenePlayed;
        w.dungeon_state = s.dungeon_state; w.forest_state = s.forest_state; w.fisher_state = s.fisher_state; w.ponter_state = s.ponter_state; w.mayhem_state = s.mayhem_state; w.ahorgis_state = s.ahorgis_state;
        w.raggenfall_state = s.raggenfall_state; w.deserter_state = s.deserter_state; w.killedGaren = s.killedGaren;
        w.RestoreChests(s.openChests);

        var p = PlayerData.Instance;
        var pd = save.playerDTO;
        p.playerName = pd.playerName; p.strength = pd.strength; p.intelligence = pd.intelligence; p.dexterity = pd.dexterity; p.persuasion = pd.persuasion; p.defense = pd.defense; p.vitality = pd.vitality;
        p.money = pd.money;
        p.playerItems.Clear();
        foreach (var it in pd.items) p.playerItems.Add(new Item(it.itemName, it.count));
        p.equipment.Clear();
        foreach(var eq in pd.equipment)
        {
            var e = new Equipment(eq.name, eq.type, eq.stats)
            {
                equipped = eq.equipped
            };
            p.equipment.Add(e);
        }
    }

}
