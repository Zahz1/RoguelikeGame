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
    private KeyCode damageCode = KeyCode.J;

    //Used for key rebinding
    private KeyCode detectedKey;
    private KeyCode debugConsoleKey = KeyCode.BackQuote;

    //Stores read inputs
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public float horizontalMouse {get; private set; }
    public float verticalMouse {get; private set; }
    public bool sprint {get; private set; }
    public bool sprintStop {get; private set; }
    public bool jump {get; private set; }
    public bool attack {get; private set; }
    public bool primaryAttack {get; private set; }
    public bool secondaryAttack {get; private set; }
    public bool specialAttack {get; private set; }
    public bool kill {get; private set; }
    public bool menu {get; private set; }
    public bool menuOpen {get; set; }
    public bool interact {get; private set; }
    public bool damageSelf {get; private set; }
    public bool openDebugConsole {get; private set; }

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
        kill = Input.GetKeyDown(killSelf);
        damageSelf = Input.GetKeyDown(damageCode);

        //Opening and closing debug console
        openDebugConsole = Input.GetKeyDown(debugConsoleKey);
        
        if(openDebugConsole){
            Debug.Log(true);
            GameEvents.Instance.DisplayDebugConsoleEnter();
        }
        

        menu = Input.GetKeyDown(menuKeyCode);
        if(menu)
        {
            //Menu not open, open menu
            if (!menuOpen)
            {
                GameEvents.Instance.MenuTriggerEnter();           
            }
            //Menu open, close menu
            else
            {
                GameEvents.Instance.MenuTriggerExit();
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
