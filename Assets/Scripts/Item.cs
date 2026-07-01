using UnityEngine;
using UnityEngine.Experimental.AI;

public class Item : MonoBehaviour
{
    public string itemName = "";
    public int count = 1;

    public Item(string name, int amount = 1)
    {
        itemName = name;
        count = amount;
    }
    
}
