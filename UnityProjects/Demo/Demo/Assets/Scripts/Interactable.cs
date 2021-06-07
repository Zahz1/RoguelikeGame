using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    //Required Components
    private Transform transform;
    private Outline outline;
    private SphereCollider interactionBoundary;
    private Rigidbody rb;

    //Distance player needs to be to an interactable
    //to interact
    private float radius = 1.5f;

    //When player is in interacting distance, get player
    public Transform player;

    //Interactable properties
    new public string name;                //Interactable in-game name
    [SerializeField]
    private InteractableType interactType;  //Interact type to specify behavior in PlayerInteractController
                                            /* Interactable Types, All listed are for prototyping ideas and functionalities, not indicative of final product
                                             * Health Shrine     : Heal player for X amount of health
                                             * Item              : Pickup item (Determine if like risk of rain or like Diablo or others where you don't stack items)
                                             * Barrier Shrine    : Grant user temperary respite from enemies in protective AOE sphere
                                             * Reaper Shrine 
                                             * (Name WIP)        : Summon a reaper that gets stronger the longer you take to fight it
                                             * Shop              : Shop interactables can allow for shop UIs to be displayed with items or upgrades to purchase
                                             * Chests            : Containers that contain random loot
                                             * Rune Altars       : Get runes for upgrades, or other game loop twist, further brainstorming needed
                                             * Next Level Portal : What ever the final product to transition levels becomes, uses SceneManager
                                             * Soul Altar        : If Coop is introduced, play where dead player souls can be placed to revive
                                             * -----------------------------------------------------------------------------
                                             * |FURTHER SUGGESTIONS OR ADJUSTMENTS SHOULD BE MADE IN LINES BELOW OR IN CALL|
                                             * -----------------------------------------------------------------------------
                                             * 
                                             */
    [SerializeField]
    private bool isUsed = false;            //Boolean value to determine if interactable has been interacted with, thus disabling or disallowing further interaction
    [SerializeField]
    private int uses;                       //Number of uses for interactable
    private bool canInteract = false;       //Boolean value if a player can be interacted with, such as if player can afford and if it has been used
    private bool isPurchasable = false;     //Boolean value if interactable requires purchase
    [SerializeField]
    private int cost;                       //If isPurchasable is true, assign a cost to the player
    [SerializeField]
    private int baseCost;                  //Used to determine cost calculations

    //Cooldown 
    private Coroutine cooldownRoutine;
    [SerializeField]
    private float cooldownTime;
    private float cooldownTimeRemaining;
    private bool routineActive = false;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        outline = GetComponent<Outline>();
        interactionBoundary = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }  

    private void Start()
    {
        //Freeze rigidbody position and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //Set interaction boundary radius
        interactionBoundary.isTrigger = true;
        interactionBoundary.radius = radius;
    }

    //Detect if a player object is in range to interact with interactable and 
    //Interactable has not been used
    public virtual bool OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && IsInteractable())
        {
            player = other.GetComponent<Transform>();
            return true;
        }
        return false;
    }

    public virtual void OnTriggerExit(Collider other)
    {
        player = null;
        outline.enabled = false;
        GameEvents.Instance.InteractionUITriggerExit();
    }

    //This base method only works for interactables that require standard currency
    public virtual bool IsInteractable()
    {
        return !this.isUsed;
    }

    //Will be used to dynamically determine cost of items and other interactables
    //Based on whatever factors are decided, ie. difficulty, rarity, etc.
    public virtual void SetCost()
    {

    }

    #region Getters
    public string GetName() { return this.name; }
    public InteractableType GetInteractableType() { return this.interactType; }
    public bool GetIsUsed() { return this.isUsed; }
    public bool GetCanInteract() { return this.canInteract; }
    public int GetCost() { return this.cost; }
    #endregion
}
