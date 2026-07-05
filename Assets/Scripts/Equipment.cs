using UnityEngine;

public class Equipment
{
    public string name;
    public string type;
    public byte[] stats = new byte[6];

    public Equipment(string eqName, string eqType, byte[] eq_stats)
    {
        name = eqName;
        type = eqType;
        stats = eq_stats;
    }
}
