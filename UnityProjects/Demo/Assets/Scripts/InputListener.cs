using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    //This script is to deal with input handling and controls

    // Start is called before the first frame update
    /// <summary>
    /// In later build, save file will hold 
    /// player's key bindings and set action
    /// KeyCodes to these values rather than 
    /// defaults
    /// </summary>
    void Start()
    {

    }

    //KeyCodes to manage controls and remapping
    private KeyCode sprintKeyCode = KeyCode.LeftShift;
    private KeyCode jumpKeyCode = KeyCode.Space;
    private KeyCode primaryAttackKeyCode = KeyCode.Mouse0;
    private KeyCode secondaryAttackKeyCode = KeyCode.Mouse1;
    private KeyCode specialAttackKeyCode = KeyCode.R;

    //Used for key rebinding
    private KeyCode detectedKey;

    //Stores read inputs
    private float horizontal;
    private float vertical;
    private float horizontalMouse;
    private float verticalMouse;
    private bool sprint;
    private bool sprintStop;
    private bool jump;
    private bool attack;
    private bool primaryAttack;
    private bool secondaryAttack;
    private bool specialAttack;

    /// <summary>
    /// Listens for every action input
    /// </summary>
    /// <returns> true if action key is pressed, else false </returns>
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        horizontalMouse = Input.GetAxisRaw("Mouse X");
        verticalMouse = Input.GetAxisRaw("Mouse X");
        sprint = Input.GetKey(sprintKeyCode);
        sprintStop = Input.GetKeyUp(sprintKeyCode);
        jump = Input.GetKeyDown(jumpKeyCode);
        primaryAttack = Input.GetKey(primaryAttackKeyCode);
        secondaryAttack = Input.GetKey(secondaryAttackKeyCode);
        specialAttack = Input.GetKey(specialAttackKeyCode);
        attack = primaryAttack || secondaryAttack || specialAttack;
        //If player uses an attack, cancel sprint and set sprintStop to true
        if (attack)
        {
            sprint = false;
            sprintStop = true;
        }
    }

    /// <summary>
    /// Below methods to return boolean values
    /// of key pressed from Update()
    /// Boolean vals can only be set to true or false
    /// from this script and no other, so other scripts can 
    /// only grab the value
    /// </summary>
    public float getHorizontal()
    {
        return horizontal;
    }

    public float getVertical()
    {
        return vertical;
    }

    public float getHorizontalMouse()
    {
        return horizontalMouse;
    }

    public float getVerticalMouse()
    {
        return verticalMouse;
    }

    public bool getSprint()
    {
        return sprint;
    }

    public bool getSprintStop()
    {
        return sprintStop;
    }

    public bool getJump()
    {
        return jump;
    }

    public bool getPrimaryAttack()
    {
        return primaryAttack;
    }

    public bool getSecondaryAttack()
    {
        return secondaryAttack;
    }

    public bool getSpecialAttack()
    {
        return specialAttack;
    }

    public bool getAttack()
    {
        return attack;
    }

    //Unfinished for rebinding later
    //Need to build player controls screen and script
    //To call InputListener component, call method to rebind
    //action, detect input, use detectKey() using findKeyPress()
    //Coroutine to figure out what key is pressed
    //Then methods to modify KeyCode variables above with 
    //New KeyCode
    /*
    private void detectKey()
    {
        StartCoroutine(findKeyPress());
    }

    IEnumerator findKeyPress()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
            {
                detectKey = kcode;
            }
        }
    }
    */
    /*
    private static readonly KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
                                                 .Cast<KeyCode>()
                                                 .Where(k => ((int)k < (int)KeyCode.Mouse0))
                                                 .ToArray();

    private static KeyCode? GetCurrentKeyDown()
    {
        if (!Input.anyKey)
        {
            return null;
        }

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKey(keyCodes[i]))
            {
                return keyCodes[i];
            }
        }
        return null;
    }*/
}
