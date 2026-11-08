using System;

public class Equipment
{
    public string name;
    public string type;
    public byte[] stats = new byte[4];
    public bool equipped;

    public Equipment(string eqName, string eqType, byte[] eq_stats)
    {
        name = eqName;
        type = eqType;
        stats = eq_stats;
        equipped = false;
    }

    public string Equip()
    {
        if (equipped) return "Already equipped!";
        Equipment eq = PlayerData.Instance.equipment.Find(o => o.type == type && o != this);
        if (eq != null)
        {
            if (eq.equipped) eq.Unequip();
        }
        equipped = true;
        PlayerData.Instance.dexterity += stats[0];
        PlayerData.Instance.strength += stats[1];
        PlayerData.Instance.intelligence += stats[2];
        PlayerData.Instance.persuasion += stats[3];
        return $"Equipped {name}!";
    }

    public string Unequip()
    {
        if (!equipped) return "Not equipped!";
        equipped = false;
        PlayerData.Instance.dexterity -= stats[0];
        PlayerData.Instance.strength -= stats[1];
        PlayerData.Instance.intelligence -= stats[2];
        PlayerData.Instance.persuasion -= stats[3];
        return $"Unequipped {name}!";
    }
}
