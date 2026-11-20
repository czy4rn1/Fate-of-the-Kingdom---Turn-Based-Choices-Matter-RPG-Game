using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    WaitTimer waitTimer = new WaitTimer();
    public bool hasMagic = false;
    public byte[] stats = new byte[2]; // str, int
    public byte dmg = 0;
    public bool stop = false;
    public string enemyName = "";
    public bool dmgCalculated = false;
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
                        dmg = (byte)Math.Round(stats[1]*standardMul);
                        dmg = (byte)(float)(dmg * ((float)(100.0f - PlayerData.Instance.intelligence)/100.0f));
                    }
                    else {
                        dmg = (byte)Math.Round(stats[0]*standardMul);
                        dmg = (byte)(float)(dmg * ((float)(100.0f - PlayerData.Instance.strength)/100.0f));
                    }
                }
                else {
                    dmg = (byte)Math.Round(stats[0]*standardMul);
                    dmg = (byte)(float)(dmg * ((float)(100.0f - PlayerData.Instance.strength)/100.0f));
                }
            }
            else if (dice > 75 && dice <= 80)
            {
                if (hasMagic)
                {
                    dice = UnityEngine.Random.Range(1, 101);
                    if (dice <= 50) dmg = (byte)Math.Round(stats[1]*superMul);
                    else dmg = (byte)Math.Round(stats[0]*superMul);
                }
                else {
                    dmg = (byte)Math.Round(stats[0]*superMul);
                }
            }
            else if (dice > 80 && dice <= 97) dmg = 0;
            else dmg = 255;
        }
        dmgCalculated = true;
    }   
}
