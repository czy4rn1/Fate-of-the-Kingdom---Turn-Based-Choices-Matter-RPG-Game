using System;

public class Item
{
    public string itemName = "";
    public int count = 1;

    public Item(string name, int amount = 1)
    {
        itemName = name;
        count = amount;
    }

    public string Use()
    {
        string desc = "";
        if (count > 0)
        {
            int curStat;
            if (itemName == "Stat Boost")
            {
                curStat = Math.Clamp(PlayerData.Instance.dexterity + 5, 0, 255);
                PlayerData.Instance.dexterity = (byte)curStat;
                curStat = Math.Clamp(PlayerData.Instance.strength + 5, 0, 255);
                PlayerData.Instance.strength = (byte)curStat;
                curStat = Math.Clamp(PlayerData.Instance.intelligence + 5, 0, 255);
                PlayerData.Instance.intelligence = (byte)curStat;
                curStat = Math.Clamp(PlayerData.Instance.persuasion + 5, 0, 255);
                PlayerData.Instance.persuasion = (byte)curStat;
                desc = "Added 5 points to each stat!";
            }
            else if (itemName == "DEX Up")
            {
                curStat = Math.Clamp(PlayerData.Instance.dexterity + 5, 0, 255);
                PlayerData.Instance.dexterity = (byte)curStat;
                desc = "Added 5 points to Dexterity!";
            }
            else if (itemName == "STR Up")
            {
                curStat = Math.Clamp(PlayerData.Instance.strength + 5, 0, 255);
                PlayerData.Instance.strength = (byte)curStat;
                desc = "Added 5 points to Strength!";
            }
            else if (itemName == "INT Up")
            {
                curStat = Math.Clamp(PlayerData.Instance.intelligence + 5, 0, 255);
                PlayerData.Instance.intelligence = (byte)curStat;
                desc = "Added 5 points to Intelligence!";
            }
            else if (itemName == "PER Up")
            {
                curStat = Math.Clamp(PlayerData.Instance.persuasion + 5, 0, 255);
                PlayerData.Instance.persuasion = (byte)curStat;
                desc = "Added 5 points to Persuasion!";
            }
        }
        PlayerData.Instance.RemoveItem(itemName, 1);
        return desc;
    }
    
}
