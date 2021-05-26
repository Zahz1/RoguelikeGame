using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera camera;
    private Transform player;
    private Animator playerAnimator;
    private InputListener inputController;

    [SerializeField] private int playerHealth;
    private string playerName;
    private string characterName;

    private bool interact;
    private GameObject item;
    private GameObject interactable;
    private GameObject focus;
    private RaycastHit hit;
    private LayerMask interactionMask;

    void Awake()
    {
        camera = GetComponent<Camera>();
        playerAnimator = GetComponent<Animator>();
        inputController = GameObject.Find("GameManager").GetComponent<InputListener>();
        playerHealth = 100;
        interactionMask = LayerMask.GetMask("Interactable");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputController.getKillSelf())
        {
            playerHealth = 0;
            Debug.Log(true);
        }
        if(playerHealth <= 0)
        {
            FindObjectOfType<GameManager>().PlayerDied();
        }
        DetectInteractable();
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

    private void DetectInteractable()
    {
        RaycastHit hitInfo;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        if (Physics.Raycast(ray, out hitInfo, 5f))
        {
            interactable = hitInfo.collider.gameObject;
            if(interactable != null && !interactable.GetComponent<Interactable>().isUsed)
            {
                SetFocus(interactable);
                //Debug.Log("Interact");
            }
        }
        else
        {
            RemoveFocus();
        }
    }

    /// <summary>
    /// If focus is not null and player interacts
    /// Detect interactable tag to display logic
    /// </summary>
    private void Interact()
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
