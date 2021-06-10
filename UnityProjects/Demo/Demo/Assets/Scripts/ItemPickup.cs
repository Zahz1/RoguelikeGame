using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    [SerializeField]
    private Inventory playerInv;

    public override void Start()
    {
        base.Start();
        base.uses = 1;
        base.interactType = InteractableType.Item;
    }

    public override void Interact()
    {
        if(playerInv != null && base.isInteractable)
        {
            playerInv.Add(this.item);
            Destroy(this.gameObject);
        }
    }

    public override bool OnTriggerEnter(Collider other)
    {
        if (base.OnTriggerEnter(other))
        {
            playerInv = base.player.GetComponent<Inventory>();
            IsInteractable();
        }
        return true;
    }

    public override void OnTriggerExit()
    {
        base.OnTriggerExit();
        playerInv = null;
    }

    public override bool IsInteractable()
    {
        if (base.IsInteractable())
        {
            base.outline.enabled = true;
            base.isInteractable = true;
            GameEvents.Instance.InteractionUITriggerEnter();
            return true;
        }
        else
        {
            base.outline.enabled = false;
            base.isInteractable = false;
            GameEvents.Instance.InteractionUITriggerExit();
        }
        return false;
    }
}

