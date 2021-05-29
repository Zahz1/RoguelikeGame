using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    
    public override string Interact()
    {
        string tag = base.Interact();
        Debug.Log("Pick up " + item.name);

        PickUp();
        return tag;

    }

    void PickUp()
    {
        Inventory.instance.Add(item);
        Destroy(gameObject);
    }
}
