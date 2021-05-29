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

    #region Singleton
    private static InputListener instance;
    public static InputListener Instance { get { return instance; } }
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Debug.LogWarning("More than one instance of InputListener found!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    //KeyCodes to manage controls and remapping
    private KeyCode sprintKeyCode = KeyCode.LeftShift;
    private KeyCode jumpKeyCode = KeyCode.Space;
    private KeyCode primaryAttackKeyCode = KeyCode.Mouse0;
    private KeyCode secondaryAttackKeyCode = KeyCode.Mouse1;
    private KeyCode specialAttackKeyCode = KeyCode.R;
    private KeyCode killSelf = KeyCode.K;
    private KeyCode menuKeyCode = KeyCode.Escape;
    private KeyCode interactKeyCode = KeyCode.E;

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
    private bool kill;
    private bool menu;
    private bool menuOpen;
    private bool interact;

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
        kill = Input.GetKey(killSelf);
        
        menu = Input.GetKeyDown(menuKeyCode);
        if(menu)
        {
            //Menu not open, open menu
            if (!menuOpen)
            {
                GameEvents.Instance.MenuTriggerEnter();
                menuOpen = true;
            }
            //Menu open, close menu
            else
            {
                GameEvents.Instance.MenuTriggerExit();
                menuOpen = false;
            }
        }


        interact = Input.GetKeyDown(interactKeyCode);
        //If player uses an attack, cancel sprint and set sprintStop to true
        if (attack)
        {
            sprint = false;
            sprintStop = true;
        }
    }

    #region Input Getters
    /// <summary>
    /// Below methods to return boolean values
    /// of key pressed from Update()
    /// Boolean vals can only be set to true or false
    /// from this script and no other, so other scripts can 
    /// only grab the value
    /// </summary>
    public float GetHorizontal()
    {
        return horizontal;
    }

    public float GetVertical()
    {
        return vertical;
    }

    public float GetHorizontalMouse()
    {
        return horizontalMouse;
    }

    public float GetVerticalMouse()
    {
        return verticalMouse;
    }

    public bool GetSprint()
    {
        return sprint;
    }

    public bool GetSprintStop()
    {
        return sprintStop;
    }

    public bool GetJump()
    {
        return jump;
    }

    public bool GetPrimaryAttack()
    {
        return primaryAttack;
    }

    public bool GetSecondaryAttack()
    {
        return secondaryAttack;
    }

    public bool GetSpecialAttack()
    {
        return specialAttack;
    }

    public bool GetAttack()
    {
        return attack;
    }

    public bool GetKillSelf()
    {
        return kill;
    }

    public bool GetMenu()
    {
        return menu;
    }

    public bool GetInteract()
    {
        return interact;
    }
    #endregion

    #region potential rebinding code
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
    #endregion

}
