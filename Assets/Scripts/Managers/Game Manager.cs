using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState mainMenuState;
    [SerializeField] private GameState gameplayState;
    [SerializeField] private GameState pauseState;
    [SerializeField] private GameState gameOverState;
    [SerializeField] private GameState survivedState; // Player survived the night

    [SerializeField] private GameState currentState;
    [SerializeField] private GameState lastState;

    [SerializeField] private PlayerInputActions playerInputActions;
    private static GameManager instance;

    public static GameManager Instance => instance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #region Game State logic
    public void ChangeGameState(GameState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
            lastState = currentState;
        }

        currentState = newState;
        currentState.EnterState();
    }

    public void SwitchToGameplay()
    {
        ChangeGameState(gameplayState);
    }

    public void SwitchToMenu()
    {
        ChangeGameState(mainMenuState);
    }

    public void SwitchToPause()
    {
        ChangeGameState(pauseState);
    }

    private void SwitchToGameOver()
    {
        ChangeGameState(gameOverState);
    }
    private void SwitchToWinner()
    {
        ChangeGameState(survivedState);
    }

    public void SwitchToLastState()
    {
        ChangeGameState(lastState);
    }

    #endregion
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        pauseState.onEnterState += PauseGame;
        pauseState.onExitState += ResumeGame;
    }

    private void OnDisable()
    {
        pauseState.onEnterState -= PauseGame;
        pauseState.onExitState -= ResumeGame;
    }
}
