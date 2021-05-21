using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private CharacterController controller;
    private InputListener inputController;
    public Transform camera;

    private float speed = 5f;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool _isGrounded;

    private Vector3 moveVertical;
    private Vector3 direction;
    private Vector3 moveDirection;
    private float targetAngle;
    private float angle;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private bool sprintRecovery = true;
    private bool jumpRecovery = true;

    //Modifiers
    private float sprintModifier = 2f;
    private float walkModifier = 0.5f;
    private float speedModifier = 1f;
    private float jumpHeight = 0.5f;
    private float gravity = 1.25f;
    private float jumpSpeed = 0.5f;
    private float fallAcceleration = 0.5f;
    private int maxJumps = 1;
    private int jumpCounter;

    //Movement type checks
    private bool isSprinting = false;
    private bool sprintStop = false;
    private bool isJumping = false;
    private bool checkJump = false;
    private bool jumpClimbing = false;
    private bool jumpDescending = false;

    private float startHeight;
    private float newHeight;

    private Vector3 cameraStartingPostion = new Vector3(0f, 0f, 0f);

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputController = GetComponent<InputListener>();
    }
    
    void Start()
    {

       // camera.transform.rotation = ;
    }

    void FixedUpdate()
    {
        //Jumping
        moveVertical = new Vector3(0f, moveVertical.y, 0f);
        if (checkJump)
        {
            isJumping = true;
            jumpRecovery = false;
            moveVertical.y = jumpSpeed;
        }
        else if (_isGrounded)
        {
            isJumping = false;
            moveVertical.y = 0f;
            if (!jumpRecovery)
            {
                StartCoroutine(activateJump());
            }
        }
        else
        {
            moveVertical.y -= gravity * Time.deltaTime;
        }
        controller.Move(moveVertical);
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = checkGrounded();
        isSprinting = inputController.getSprint() && sprintRecovery;
        checkJump = inputController.getJump() && _isGrounded && jumpRecovery;
        sprintStop = inputController.getSprintStop();

        //Vector for direction of movement on the X and Z axis
        direction = new Vector3(inputController.getHorizontal(), 0f, inputController.getVertical());
        //If |direction| > 0, then input is detected
        if (direction.magnitude >= 0.1f)
        {
            //Direction or angle of movement based on X and Z values and position of FreeLook camera
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //Rotate player model based on angle found above
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //Move player
            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            //Player is sprinting
            if (isSprinting)
            {
                movementSpeed = speed * sprintModifier;
            }
            //Player not sprinting
            else if(!isSprinting && _isGrounded)
            {      
                movementSpeed = speed * speedModifier;
            }
            else
            {
                movementSpeed = 0f;
            }
            controller.Move(moveDir * movementSpeed * Time.deltaTime);
        }
        else
        {
            movementSpeed = 0f;
        }
        /*
        //Jump
        //If player is grounded, then set jumpCounter to maxJumps
        if (_isGrounded)
        {
            jumpCounter = maxJumps;   
        }
        //If player jumps, jump counter is decremented and new transform.y poistion
        //is calculated, if player does not jump, newHeight = 0
        if (checkJump && jumpCounter > 0)
        {
            jumpCounter--;
            newHeight = transform.position.y + jumpHeight;
        }
        else
        {
            newHeight = 0f;
        }
        if(newHeight > 0f)
        {

        }
        */
        //If in the air, movement speed is preserved but reduced over time
        if (!_isGrounded)
        {
            if (isJumping && movementSpeed > 0f)
            {
                movementSpeed -= fallAcceleration * Time.deltaTime;
            }
            else
            {
                movementSpeed = 0f;
            }
        }

        //If stop sprinting
        if (sprintStop)
        {
            StartCoroutine(activateSprint());
        }
    }

    public bool getSprinting()
    {
        return isSprinting;
    }

    public bool getJumping()
    {
        return isJumping;
    }

    public bool getSprintRecovery()
    {
        return sprintRecovery;
    }

    private IEnumerator activateSprint()
    {
        yield return new WaitForSeconds(0.5f);
        sprintRecovery = true;
    }

    private IEnumerator activateJump()
    {
        yield return new WaitForSeconds(0.1f);
        jumpRecovery = true;
    }
    /*
    private IEnumerator jump(float newHeight)
    {
        
        
    }*/

    private bool checkGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.01f, 1 << LayerMask.NameToLayer("Ground"));
    }

    public float getSpeed()
    {
        return movementSpeed;
    }

}
