using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    WaitTimer waitTimer = new WaitTimer();
    public bool hasMagic = false;
    public short[] stats = new short[2]; // str, int
    public short dmg = 0;
    public bool stop = false;
    public string enemyName = "";
    public bool dmgCalculated = false;
    float divider = 0f;
    public void Action()
    {
        if (stop) {
            dmg = 0;
            stop = false;
        }
        else {
            float standardMul = UnityEngine.Random.Range(0.5f, 0.8f);
            float superMul = UnityEngine.Random.Range(1f, 1.4f);
            int dice = UnityEngine.Random.Range(1,101);
            if (dice <= 75)
            {
                if (hasMagic)
                {
                    dice = UnityEngine.Random.Range(1, 101);
                    if (dice <= 50) {
                        dmg = (short)Math.Round(stats[1]*standardMul);
                        divider = (100.0f - PlayerData.Instance.intelligence)/100.0f;
                        Debug.Log($"{divider}");
                        dmg = (short)Math.Round((float)(dmg*divider));
                    }
                    else {
                        dmg = (short)Math.Round(stats[0]*standardMul);
                        divider = (100.0f - PlayerData.Instance.strength)/100.0f;
                        Debug.Log($"{divider}");
                        dmg = (short)Math.Round((float)(dmg*divider));
                    }
                }
                else {
                    dmg = (short)Math.Round(stats[0]*standardMul);
                    divider = (100.0f - PlayerData.Instance.strength)/100.0f;
                    Debug.Log($"{divider}");
                    dmg = (short)Math.Round((float)(dmg*divider));
                }
            }
            else if (dice > 75 && dice <= 80)
            {
                if (hasMagic)
                {
                    dice = UnityEngine.Random.Range(1, 101);
                    if (dice <= 50) dmg = (short)Math.Round(stats[1]*superMul);
                    else dmg = (short)Math.Round(stats[0]*superMul);
                }
                else {
                    dmg = (short)Math.Round(stats[0]*superMul);
                }
            }
            else if (dice > 80 && dice <= 97) dmg = 0;
            else dmg = 255;
        }
        dmgCalculated = true;
    }   
}
