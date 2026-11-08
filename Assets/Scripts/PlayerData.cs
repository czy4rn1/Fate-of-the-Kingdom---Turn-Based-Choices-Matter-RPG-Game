using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance {get; private set;}
    public string playerName;
    public short strength;
    public short intelligence;
    public short persuasion;
    public short dexterity;
    public short vitality;
    public short defense;

    public List<Item> playerItems;
    public List<Equipment> equipment;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerItems = new List<Item>();
            equipment = new List<Equipment>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(string itemName, int amount = 1)
    {
        Item existingItem = playerItems.Find(item => item.itemName == itemName);
        if (existingItem != null)
        {
            existingItem.count += amount;
        }
        else
        {
            playerItems.Add(new Item(itemName, amount));
            Item item = playerItems.Find(item => item.itemName == itemName);
            Debug.Log($"{item.itemName}, {item.count}");
        }
    }
    public void RemoveItem(string itemName, int amount = 1)
    {
        Item existingItem = playerItems.Find(item => item.itemName == itemName);
        if (existingItem != null) {
            existingItem.count -= amount;
            if (existingItem.count<=0) playerItems.Remove(existingItem);
        }
    }
    
    /*public IEnumerator AddExperience(int experience)
    {
        while(experience > 0)
        {
            exp++;
            allExp++;
            experience--;
            if (exp >= expToNextLvl) {
                 lvl++;
                 expToNextLvl = (int)(lvl * lvl_wall * 1.3f);
                 exp = 0;
            }
            yield return null; 
        }
    }*/

    public void AddEquipment(string name, string type, byte[] stats)
    {
        equipment.Add(new Equipment(name, type, stats));
    }
}
