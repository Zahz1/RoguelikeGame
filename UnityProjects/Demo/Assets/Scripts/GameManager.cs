using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } } 
    void Awake()
    {
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

    private bool gameEnded = false;
    private bool playerDied = false;
    private bool playerWon = false;

    private void Start()
    {
        animator = GameObject.Find("Player").GetComponentInChildren<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameEvents.Instance.OnMenuTriggerEnter += DisplayMenu;
        GameEvents.Instance.OnMenuTriggerExit += CloseMenu;
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
                Invoke("DisplayDiedEndGameUI", 2f);
            }
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        PauseController.GameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void DisplayMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        PauseController.GameIsPaused = true;
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
        PauseController.GameIsPaused = !PauseController.GameIsPaused;
    }
}
