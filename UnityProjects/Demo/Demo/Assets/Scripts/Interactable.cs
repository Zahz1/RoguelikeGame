using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    //Required Components
    public Outline outline;
    private SphereCollider interactionBoundary;
    private Rigidbody rb;

    //Distance player needs to be to an interactable
    //to interact
    private float radius = 1.75f;

    //When player is in interacting distance, get player
    public Transform player;
    public PlayerInteractController playerInteractController;

    //Interactable properties
    new public string name;                //Interactable in-game name
    public InteractableType interactType;  //Interact type to specify behavior in PlayerInteractController                                       
    public bool isUsed = false;             //Boolean value to determine if interactable has been interacted with, thus disabling or disallowing further interaction
    public int uses;   
    private bool isPurchasable = false;     //Boolean value if interactable requires purchase
    public bool isInteractable;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        interactionBoundary = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }  

    public virtual void Start()
    {
        //Freeze rigidbody position and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //Set interaction boundary radius
        interactionBoundary.isTrigger = true;
        interactionBoundary.radius = radius;
    }

    public virtual void Update()
    {
        
    }

    //When player enters interactable trigger collider, check if remaining uses is greater than 0
    //Grab the player and its PlayerInteractController, set the PlayerInteractionController's focus
    //To this GameObject 
    public virtual bool OnTriggerEnter(Collider other)
    {
        //if GameObject colliding with trigger is tag "Player" and this.uses > 0
        //Then grab player transform and PlayerInteractionController, if interactable 
        //is useable, set PlayerInteractionController focus to this.
        if (other.tag == "Player" && uses > 0)
        {
            player = other.GetComponent<Transform>();
            playerInteractController = player.GetComponent<PlayerInteractController>();
            playerInteractController.SetFocus(this.gameObject);
            return true;
        }
        return false;
    }

    public virtual void OnTriggerExit()
    {
        playerInteractController.RemoveFocus();
        
        player = null;
        playerInteractController = null;

        outline.enabled = false;

        GameEvents.Instance.InteractionUITriggerExit();
    }

    public virtual void Interact()
    {
        this.uses--;
        this.isUsed = true;
        if(this.uses == 0)
        {
            playerInteractController.RemoveFocus();
            interactionBoundary.enabled = false;
            GameEvents.Instance.InteractionUITriggerExit();
            this.enabled = false;
        }  
    }

    public virtual bool IsInteractable()
    {
        if(!this.isUsed && playerInteractController.GetFocus() == this.gameObject) 
        {
            return true;
        }
        return false;
    }

    public string GetName() { return this.name; }
    public InteractableType GetInteractableType() { return this.interactType; }
}
