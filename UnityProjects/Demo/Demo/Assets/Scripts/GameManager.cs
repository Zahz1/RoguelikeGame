using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator playerAnimator;
    private GameObject player;
    private CharacterController characterController;
    private PlayerController playerController;
    private PlayerMovementControllerImproved playerMovementController;
    private PlayerCombatController playerCombatController;
    private InputListener inputController;

    private bool gameEnded = false;
    private bool playerDied = false;
    private bool playerWon = false;
    private bool openMenu = false;

    //UIs
    public GameObject PlayerUI;
    public GameObject DiedEndGameUI;
    public GameObject InGameMenuUI;
    private GameObject activeUI = null;

    private void Awake()
    {
        player = GameObject.Find("Player");
        characterController = player.GetComponent<CharacterController>();
        playerController = player.GetComponent<PlayerController>();
        playerMovementController = player.GetComponent<PlayerMovementControllerImproved>();
        playerCombatController = player.GetComponent<PlayerCombatController>();
        inputController = GetComponent<InputListener>();

    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //Opening and closing game menu
        openMenu = inputController.getMenu();
        //If menu is not open, and menu key is pressed, open menu
        //If menu is open, and menu key is pressed, close menu
        if (openMenu)
        {
            if (!InGameMenuUI.activeSelf)
            {
                DisplayMenu();
            }
            else if (InGameMenuUI.activeSelf)
            {
                CloseMenu();
            }
            else
            {
                Debug.Log("Open/Close menu failure!");
            }
        }

    }

    /// <summary>
    /// Very introductory version of the EndGame method
    /// Parameters can and will be added to this method to
    /// provide players additional information about the game
    /// that just ended
    /// </summary>
    private void EndGame()
    {
        if (!gameEnded)
        {
            Debug.Log("Game Over");
            gameEnded = true;
            if (playerDied)
            {
                playerAnimator.SetTrigger("isDead");
                Invoke("DisplayDiedEndGameUI", 2f);
            }
        }

    }

    /// <summary>
    /// Can be called by buttons in the UI to reload current scene
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1;
        PauseController.GameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// In the future, parameters can be added 
    /// to display additional inforamtion in this UI
    /// </summary>
    private void DisplayDiedEndGameUI()
    {
        DiedEndGameUI.SetActive(true);
    }

    /// <summary>
    /// When displaying menu
    /// Get the active UI, the disable it
    /// When the menu is closed, the active UI
    /// is then reenabled 
    /// </summary>
    private void DisplayMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        if (DiedEndGameUI.activeSelf)
        {
            activeUI = DiedEndGameUI;
        }else if (PlayerUI.activeSelf)
        {
            activeUI = PlayerUI;
        }
        Time.timeScale = 0;
        PauseController.GameIsPaused = true;
        InGameMenuUI.SetActive(true);
    }
    
    /// <summary>
    /// This is called to close the menu and go back into
    /// the game, not returning to menu or desktop
    /// </summary>
    public void CloseMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        InGameMenuUI.SetActive(false);
        if(activeUI != null)
        {
            activeUI.SetActive(true);
        }
        Time.timeScale = 1;
        //PauseController.GameIsPaused = false;
        StartCoroutine(switchPause());
        Debug.Log("Close Menu");
    }

    public void PlayerDied()
    {
        playerDied = true;
        EndGame();
    }

    IEnumerator switchPause()
    {
        yield return new WaitForSeconds(0.1f);
        PauseController.GameIsPaused = !PauseController.GameIsPaused;
    }
}
