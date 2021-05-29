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
    new public string name;
    private float radius = 1.5f;
    public bool isUsed = false;
    public bool canInteract = false;


    void Awake()
    {
        transform = GetComponent<Transform>();
        outline = GetComponent<Outline>();
        interactionBoundary = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //Freeze rigidbody position and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //Set interaction boundary radius
        interactionBoundary.isTrigger = true;
        interactionBoundary.radius = radius;
    }

    //This method is meant to be overwritten by classes that 
    //Derive from Interactable
    public virtual string Interact()
    {
        //return the tag of interactable for player
        return transform.tag;
    }

    //Detect if a player object is in range to interact with interactable and 
    //Interactable has not been used
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isUsed)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        outline.enabled = false;
    }
}
