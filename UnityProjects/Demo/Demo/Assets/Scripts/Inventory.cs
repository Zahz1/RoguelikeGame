using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();

    public void Add(Item item)
    {
        this.inventory.Add(item);
    }

    public void Remove(Item item)
    {
        this.inventory.Remove(item);
    }
}
