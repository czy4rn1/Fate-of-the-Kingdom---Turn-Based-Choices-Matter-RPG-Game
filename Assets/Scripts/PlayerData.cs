using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance {get; private set;}
    public string playerName;
    public byte strength;
    public byte intelligence;
    public byte persuasion;
    public byte dexterity;
    public byte vitality;
    public byte defense;

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
}
