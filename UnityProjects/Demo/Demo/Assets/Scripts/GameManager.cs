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

        SetGameDifficultyLevelTransitionSpeed();
        DifficultyModifier = GameDifficulty.Hard;
    }
    #endregion

    private Animator animator;

    private bool gameEnded;
    private bool playerDied;
    private bool playerWon;


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

        //Subscribe method to OnStageCompleteEvent
        GameEvents.Instance.OnStageCompleteEnter += OnStageComplete;
    }

    private void Update(){
        IncreaseGameDifficultyLevel();
    }

    private void EndGame()
    {
        if (!gameEnded)
        {
            if (playerDied)
            {
                Debug.Log("Game Over");
                gameEnded = true;
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

    

    #region - Game Difficulty -

    public int GameStage { get; private set; } //Increases after each completed level
    public int GameDifficultyLevel { get; private set; } //Increases over time and stage transitions
    private float GameDifficultyLevelTransitionSpeed; // Time until a GameDifficultyLevel increases, Dependent on DifficultyModifier
    public GameDifficulty DifficultyModifier { get; private set; } //Is set on game start up   

    private Coroutine TransitionDifficultyLevelRoutine;
    private bool TransitionDifficultyLevelRoutineIsActive;

    private void OnStageComplete(){
        this.GameStage++;
    }

    private void SetGameDifficultyLevelTransitionSpeed(){
        switch(DifficultyModifier){
            case GameDifficulty.Normal:
                GameDifficultyLevelTransitionSpeed = 240f; //Seconds
                break;
            case GameDifficulty.Hard:
                GameDifficultyLevelTransitionSpeed = 180f;
                break;
            case GameDifficulty.Nightmare:
                GameDifficultyLevelTransitionSpeed = 120f;
                break;
            case GameDifficulty.Regret:
                GameDifficultyLevelTransitionSpeed = 90f;
                break;
        }
    }

    private void IncreaseGameDifficultyLevel(){
        if(TransitionDifficultyLevelRoutineIsActive){ return; }
        TransitionDifficultyLevelRoutine = StartCoroutine(TransitionDifficultyLevel());
    }

    IEnumerator TransitionDifficultyLevel(){
        TransitionDifficultyLevelRoutineIsActive = true;
        yield return new WaitForSeconds(GameDifficultyLevelTransitionSpeed);
        GameDifficultyLevel++;
        TransitionDifficultyLevelRoutineIsActive = false;
    }

    #endregion
}
