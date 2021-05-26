using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementControllerImproved : MonoBehaviour
{
    private Transform cameraMainTransform;
    private Transform player;
    private InputListener inputController;
    private Animator animationController;

    [SerializeField] private float currentSpeed;
    private float playerSpeed = 3.0f;
    private float jumpHeight = 2.0f;
    private float gravityValue = -9.81f;
    private float sprintModifier = 2f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;

    private float horizontal;
    private float vertical;
    private bool jump;
    private bool sprint;
    private bool sprintStop;

    private float sprintRecoveryTime = 0.2f;
    private bool sprintRecovery = true;

    private Vector2 movement;
    private Vector3 move;
    private float targetAngle;
    private float rotationSpeed = 4f;
    private Quaternion rotation;

    //For checkGround() Physics.SphereCast
    private LayerMask layerMask;
    private LayerMask groundMask;
    private LayerMask interactableMask;
    private float sphereRadius = 0.5f;
    private float maxDistance = 0.45f;
    
    private void Awake()
    {
        groundMask = LayerMask.GetMask("Ground");
        interactableMask = LayerMask.GetMask("Interactable");
        layerMask = groundMask | interactableMask;
    }

    private void Start()
    {
        player = GetComponent<Transform>();
        controller = gameObject.GetComponent<CharacterController>();
        inputController = GameObject.Find("GameManager").GetComponent<InputListener>();
        cameraMainTransform = Camera.main.transform;
        animationController = GetComponent<Animator>();
    }



    void Update()
    {
        groundedPlayer = checkGrounded();
        //Getting input
        //Only get input if game is not paused
        if (!PauseController.GameIsPaused)
        {
            //Sprint if using sprint key and sprintRecovery == true
            sprint = inputController.getSprint() && sprintRecovery;
            sprintStop = inputController.getSprintStop();
            jump = inputController.getJump() && groundedPlayer;
            horizontal = inputController.getHorizontal();
            vertical = inputController.getVertical();
        }
        
        //If player falls off the map, restart the game
        //Note, this should be changed to killing the player
        //And allowing the GameManager decide to restart the level
        //If all players are dead
        if (player.position.y < -5f)
        {
            FindObjectOfType<GameManager>().PlayerDied();
        }

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        movement = new Vector2(horizontal, vertical);
        move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;

        //Determine movement speed, idle(not moving), walking(not sprinting) or sprinting
        if (movement.magnitude == Vector2.zero.magnitude)
        {
            currentSpeed = 0f;
        }
        else if (movement.magnitude > Vector2.zero.magnitude && !sprint)
        {
            currentSpeed = playerSpeed;
        }
        else if (movement.magnitude > Vector2.zero.magnitude && sprint)
        {
            currentSpeed = playerSpeed * sprintModifier;
        }
        controller.Move(move * Time.deltaTime * currentSpeed);

        // Changes the height position of the player..
        if (jump)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            //animationController.SetTrigger("isJump");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if(movement != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    private bool checkGrounded()
    {
        //return Physics.Raycast(transform.position, Vector3.down, 0.01f, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit hit;
        return Physics.SphereCast(player.position + controller.center, sphereRadius, Vector3.down, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
    }

    public float getCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool getJumping()
    {
        return jump;
    }

    public bool getGrounded()
    {
        return groundedPlayer;
    }

}
