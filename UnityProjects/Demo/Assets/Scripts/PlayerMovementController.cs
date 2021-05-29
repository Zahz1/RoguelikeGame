using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    private Transform cameraMainTransform;
    private Transform player;
    private Animator animationController;

    [SerializeField] private float currentSpeed;
    private float playerSpeed = 3.0f;
    private float jumpHeight = 2.0f;
    private float gravityValue = -9.81f;
    private float sprintModifier = 2f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;

    //Player input
    private float horizontal;
    private float vertical;
    private bool jump;
    private bool sprint;
    private bool sprintStop;

    //Ability to sprint
    private float sprintRecoveryTime = 0.2f;
    private bool sprintRecovery = true;

    //Handles character movement and rotation
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
        
        player = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        animationController = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }


    void Update()
    {
        groundedPlayer = CheckGrounded();
        //Getting input
        //Only get input if game is not paused
        if (!PauseController.GameIsPaused)
        {
            //Sprint if using sprint key and sprintRecovery == true
            sprint = InputListener.Instance.GetSprint() && sprintRecovery;
            sprintStop = InputListener.Instance.GetSprintStop();
            jump = InputListener.Instance.GetJump() && groundedPlayer;
            horizontal = InputListener.Instance.GetHorizontal();
            vertical = InputListener.Instance.GetVertical();
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

    private bool CheckGrounded()
    {
        //return Physics.Raycast(transform.position, Vector3.down, 0.01f, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit hit;
        return Physics.SphereCast(player.position + controller.center, sphereRadius, Vector3.down, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool GetJumping()
    {
        return jump;
    }

    public bool GetGrounded()
    {
        return groundedPlayer;
    }

    public Vector3 getMoveVector() {
        return move;
    }

}
