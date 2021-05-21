using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputListener))]
public class PlayerMovementControllerImproved : MonoBehaviour
{
    private Transform cameraMainTransform;
    private InputListener inputController;

    [SerializeField] private float currentSpeed;
    private float playerSpeed = 5.0f;
    private float jumpHeight = 3.0f;
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
    
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        inputController = GetComponent<InputListener>();
        cameraMainTransform = Camera.main.transform;
    }

    void Update()
    {
        groundedPlayer = checkGrounded();
        //Sprint if using sprint key and sprintRecovery == true
        sprint = inputController.getSprint() && sprintRecovery;
        sprintStop = inputController.getSprintStop();
        jump = inputController.getJump();

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        movement = new Vector2(inputController.getHorizontal(), inputController.getVertical());
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
        if (jump && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
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
        return Physics.Raycast(transform.position, Vector3.down, 0.02f, 1 << LayerMask.NameToLayer("Ground"));
    }

    public float getCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool getJump()
    {
        return jump;
    }

    public bool getGrounded()
    {
        return groundedPlayer;
    }

}
