using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimationStateController))]
[RequireComponent(typeof(CharacterInfo))]
[RequireComponent(typeof(PlayerInteractController))]
[RequireComponent(typeof(Inventory))]
public class PlayerController : MonoBehaviour
{
    private int id;

    private Transform player;
    private CharacterController characterController;
    private PlayerMovementController playerMovementController;
    private PlayerCombatController playerCombatController;
    private Animator animator;
    private AnimationStateController animationStateController;
    private CharacterInfo playerStats;
    private PlayerInteractController playerInteractController;
    private Inventory playerInv;

    private bool interact;
    private Interactable interactable;
    private GameObject item;
    private GameObject focus;
    private RaycastHit hit;
    private LayerMask interactionMask;

    void Awake()
    {
        player = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
        playerMovementController = GetComponent<PlayerMovementController>();
        playerCombatController = GetComponent<PlayerCombatController>();
        animator = GetComponent<Animator>();
        animationStateController = GetComponent<AnimationStateController>();
        playerStats = GetComponent<CharacterInfo>();
        playerInteractController = GetComponent<PlayerInteractController>();
        playerInv = GetComponent<Inventory>();
        interactionMask = LayerMask.GetMask("Interactable");
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController.radius = 0.4f;
        characterController.height = 1.8f;
        characterController.center = new Vector3(0f, 0.9f, 0f);
        characterController.skinWidth = 0.01f;
        characterController.minMoveDistance = 0f;
        characterController.stepOffset = 0.3f;
        characterController.slopeLimit = 45f;
    }

    void Update()
    {
        if (InputListener.Instance.kill && playerStats.isAlive)
        {
            playerStats.Damage(playerStats.currentStats.CurrentHealth);
            Debug.Log("Player Killed Self!");
            playerMovementController.enabled = false;
            playerCombatController.enabled = false;
            animationStateController.enabled = false;
        }  
    }

    public void SetFocus(GameObject newFocus)
    {
        focus = newFocus;
        focus.GetComponent<Outline>().enabled = true;
    }

    private void RemoveFocus()
    {
        focus.GetComponent<Outline>().enabled = false;
        focus = null;
    }

    /// <summary>
    /// If focus is not null and player interacts
    /// Detect interactable tag to display logic
    /// </summary>
    private void Interact()
    {
        
    } 



}
