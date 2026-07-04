using System.Collections;
using System.Collections.Generic;
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

    public int exp = 0;
    public int allExp = 0;
    public byte lvl = 1;
    public byte lvl_wall = 200;
    public int expToNextLvl = 200;

    public List<Item> playerItems;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerItems = new List<Item>();
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
    public IEnumerator AddExperience(int experience)
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
    }
}
