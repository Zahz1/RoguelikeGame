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

public class PlayerController : MonoBehaviour
{

    private Transform player;
    private CharacterController characterController;
    private PlayerMovementController playerMovementController;
    private PlayerCombatController playerCombatController;
    private Animator animator;
    private AnimationStateController animationStateController;
    private CharacterInfo characterInfo;
    private PlayerInteractController playerInteractController;

    [SerializeField] private int playerHealth;

    private bool interact;
    private GameObject item;
    private GameObject interactable;
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
        characterInfo = GetComponent<CharacterInfo>();
        playerInteractController = GetComponent<PlayerInteractController>();
        playerHealth = 100;
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
        if (InputListener.Instance.GetKillSelf())
        {
            playerHealth = 0;
            Debug.Log(true);
        }
        if(playerHealth <= 0)
        {
            GameManager.Instance.PlayerDied();
        }    
    }

    private void SetFocus(GameObject newFocus)
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
    void Interact()
    {
        //Verify focus is not null
        if (focus != null)
        {
            //Ladder based on Interactable tags
            if(focus.tag == "HealthShrine")
            {
                this.playerHealth += 25;
            }
            focus.GetComponent<Interactable>().isUsed = true;
        }
    } 

}
