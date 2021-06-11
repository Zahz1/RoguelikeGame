using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } } 
    void Awake()
    {
        this.GameStage = 1;
        
        if(instance != null && instance != this)
        {
            Debug.LogWarning("More than one instance of GameManager found!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    private Animator animator;

    private bool gameEnded;
    private bool playerDied;
    private bool playerWon;

    private int GameStage;
    public int DifficultyModifier;

    private void Start()
    {
        gameEnded = false;
        playerDied = false;
        playerWon = false;
        animator = GameObject.Find("Player").GetComponentInChildren<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Subscribe methods to open and close menu events
        GameEvents.Instance.OnMenuTriggerEnter += DisplayMenu;
        GameEvents.Instance.OnMenuTriggerExit += CloseMenu;

        //Subscribe method to PlayerDied event
        GameEvents.Instance.OnPlayerDiedTriggerEnter += PlayerDied;
    }

    private void EndGame()
    {
        if (!gameEnded)
        {
            Debug.Log("Game Over");
            gameEnded = true;
            if (playerDied)
            {
                animator.SetTrigger("isDead");
            }
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        PauseController.Instance.GameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void DisplayMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        PauseController.Instance.GameIsPaused = true;
    }

    public void CloseMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        StartCoroutine(switchPause());
    }

    public void PlayerDied()
    {
        playerDied = true;
        EndGame();
    }

    IEnumerator switchPause()
    {
        yield return new WaitForSeconds(0.1f);
        PauseController.Instance.GameIsPaused = !PauseController.Instance.GameIsPaused;
    }

    public int GetGameStage(){ return this.GameStage; }
}
